using LapDistance.Data;
using LapDistance.Models;
using LapDistance.Services;
using System.Data.Common;
using System.Text.Json;
using System.Windows.Forms;

namespace LapDistance
{
    /// <summary>
    /// Главная форма приложения: выбор JSON-файла, сохранение тренировки в базу данных,
    /// выбор ранее сохранённой тренировки, вывод общей информации и таблицы
    /// анализа дистанции по кругам (вариант 9), а также демонстрация обобщённого
    /// сервиса Service&lt;T&gt; и фильтрации с Predicate&lt;T&gt; (ПР3).
    ///
    /// Форма НЕ обращается к DbContext – только к методам Service&lt;Workout&gt; и Service&lt;Lap&gt;,
    /// которые передаются через конструктор (Dependency Injection).
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>Обобщённый сервис для сущности «тренировка».</summary>
        private readonly Service<Workout> _workoutService;

        /// <summary>Тот же обобщённый сервис, но для сущности «круг».</summary>
        private readonly Service<Lap> _lapService;

        /// <summary>Текущая отображаемая тренировка (из JSON или из БД).</summary>
        private Workout? _current;

        /// <summary>Откуда получена текущая тренировка: true – из базы данных, false – из JSON-файла.</summary>
        private bool _currentFromDatabase;

        /// <summary>Элемент выпадающего списка выбора кругов.</summary>
        private sealed record LapFilterItem(LapFilter Filter, string Title)
        {
            public override string ToString() => Title;
        }

        /// <summary>Элемент списка «Сущность» на вкладке ПР3.</summary>
        private sealed record EntityTypeItem(Type EntityType, string Title)
        {
            public override string ToString() => Title;
        }

        /// <summary>Конструктор. Оба сервиса подставляет контейнер DI (см. Program.cs).</summary>
        public MainForm(Service<Workout> workoutService, Service<Lap> lapService)
        {
            _workoutService = workoutService;
            _lapService = lapService;
            InitializeComponent();
            ConfigureGridColumns();
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            RefreshSavedWorkouts();
            SetupEntityTab();
            string? dbPath = SafeGetDatabasePath();
            ShowInfo("Выберите JSON-файл тренировки или загрузите ранее сохранённую тренировку из базы данных." +
                     (dbPath != null ? Environment.NewLine + "База данных: " + dbPath : ""));
        }

        // ------------------------------------------------------------------
        //  1. Загрузка JSON-файла (практическая работа № 1)
        // ------------------------------------------------------------------

        private void btnOpenFile_Click(object? sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            LoadWorkoutFromFile(openFileDialog.FileName);
        }

        /// <summary>Читает файл, десериализует его и выводит тренировку на форму.</summary>
        private void LoadWorkoutFromFile(string path)
        {
            lblFileName.Text = Path.GetFileName(path);
            lblFileName.ForeColor = SystemColors.ControlText;
            ClearWorkoutView();

            SuuntoWorkoutFile file;
            try
            {
                Cursor = Cursors.WaitCursor;
                file = WorkoutReader.Load(path);
            }
            catch (FileNotFoundException)
            {
                ShowError($"Файл не найден: {path}");
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError($"Нет доступа к файлу. {ex.Message}");
                return;
            }
            catch (IOException ex)
            {
                ShowError($"Не удалось прочитать файл. {ex.Message}");
                return;
            }
            catch (JsonException ex)
            {
                ShowError("Содержимое файла имеет некорректный формат JSON." + Environment.NewLine + ex.Message);
                return;
            }
            catch (InvalidWorkoutFileException ex)
            {
                ShowError("Файл не является файлом тренировки Suunto. " + ex.Message);
                return;
            }
            catch (Exception ex)
            {
                ShowError("Непредвиденная ошибка при загрузке файла: " + ex.Message);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            // JSON -> объект Workout (общая информация + круги для варианта 9)
            Workout workout = WorkoutMapper.FromJson(file, Path.GetFileName(path));
            ShowWorkout(workout, fromDatabase: false);
        }

        // ------------------------------------------------------------------
        //  2. Сохранение в базу данных
        // ------------------------------------------------------------------

        private void btnSaveToDb_Click(object? sender, EventArgs e)
        {
            if (_current == null || _currentFromDatabase)
                return;

            try
            {
                // Проверка дубликата – фильтрация через Predicate<Workout> (лямбда-выражение)
                DateTime start = _current.StartTime;
                int? type = _current.ActivityType;
                if (_workoutService.Any(w => w.StartTime == start && w.ActivityType == type))
                {
                    DialogResult answer = MessageBox.Show(this,
                        "Тренировка с такими же датой начала и видом активности уже есть в базе данных.\n" +
                        "Сохранить ещё одну копию?",
                        "Повторное сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer != DialogResult.Yes)
                        return;
                }

                Cursor = Cursors.WaitCursor;
                _workoutService.Add(_current);     // круги сохраняются вместе с тренировкой
                int id = _current.Id;              // Id присвоен базой данных

                // Дальнейшая работа с тренировкой – через базу данных:
                // обновляем список и показываем только что сохранённую запись уже из БД
                RefreshSavedWorkouts(selectId: id);
                LoadWorkoutFromDatabase(id);
                RefreshEntityGrid();
                ShowInfo($"Тренировка сохранена в базе данных (Id = {id}, кругов: {_current?.Laps.Count ?? 0}). " +
                         "Сейчас отображаются данные, прочитанные из базы.");
            }
            catch (Exception ex)
            {
                ShowError("Не удалось сохранить тренировку в базу данных: " + RootMessage(ex));
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // ------------------------------------------------------------------
        //  3. Просмотр сохранённой тренировки
        // ------------------------------------------------------------------

        private void btnLoadFromDb_Click(object? sender, EventArgs e)
        {
            if (cmbSavedWorkouts.SelectedItem is not Workout selected)
            {
                ShowWarning("Выберите тренировку в списке сохранённых.");
                return;
            }

            LoadWorkoutFromDatabase(selected.Id);
        }

        /// <summary>Читает тренировку из базы данных через сервис и показывает её.</summary>
        private void LoadWorkoutFromDatabase(int id)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                // Тренировка вместе с кругами (Include Laps)
                Workout? workout = _workoutService.GetById(id, w => w.Laps);
                if (workout == null)
                {
                    ShowWarning($"Тренировка с Id = {id} не найдена в базе данных.");
                    RefreshSavedWorkouts();
                    return;
                }

                // Круги – в порядке их окончания
                workout.Laps.Sort((a, b) =>
                {
                    int byTime = Nullable.Compare(a.EndTime, b.EndTime);
                    return byTime != 0 ? byTime : a.Number.CompareTo(b.Number);
                });

                lblFileName.Text = workout.FileName;
                lblFileName.ForeColor = SystemColors.ControlText;
                ClearWorkoutView();
                ShowWorkout(workout, fromDatabase: true);
            }
            catch (Exception ex)
            {
                ShowError("Не удалось прочитать тренировку из базы данных: " + RootMessage(ex));
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>Перечитывает список сохранённых тренировок в выпадающий список.</summary>
        private void RefreshSavedWorkouts(int? selectId = null)
        {
            try
            {
                List<Workout> saved = _workoutService
                    .GetAll(w => w.Laps)                       // нужны для подсчёта кругов в подписи
                    .OrderByDescending(w => w.StartTime)
                    .ToList();

                cmbSavedWorkouts.BeginUpdate();
                cmbSavedWorkouts.Items.Clear();
                foreach (Workout w in saved)
                    cmbSavedWorkouts.Items.Add(w);
                cmbSavedWorkouts.EndUpdate();

                cmbSavedWorkouts.DisplayMember = nameof(Workout.DisplayName);

                if (selectId.HasValue)
                {
                    int index = saved.FindIndex(w => w.Id == selectId.Value);
                    if (index >= 0) cmbSavedWorkouts.SelectedIndex = index;
                }
                else if (saved.Count > 0 && cmbSavedWorkouts.SelectedIndex < 0)
                {
                    cmbSavedWorkouts.SelectedIndex = 0;
                }

                bool any = saved.Count > 0;
                cmbSavedWorkouts.Enabled = any;
                btnLoadFromDb.Enabled = any;
                lblSavedCaption.Text = any ? $"Сохранённые тренировки ({saved.Count}):" : "Сохранённые тренировки (нет):";
            }
            catch (Exception ex)
            {
                ShowError("Не удалось получить список тренировок из базы данных: " + RootMessage(ex));
            }
        }

        // ------------------------------------------------------------------
        //  Отображение тренировки (общий код для JSON и БД)
        // ------------------------------------------------------------------

        private void ShowWorkout(Workout workout, bool fromDatabase)
        {
            _current = workout;
            _currentFromDatabase = fromDatabase;

            lblDataSource.Text = fromDatabase
                ? $"Источник: база данных (Id = {workout.Id})"
                : "Источник: JSON-файл (в базу не сохранено)";
            lblDataSource.ForeColor = fromDatabase ? Color.SeaGreen : Color.DarkOrange;

            // Сохранять можно только тренировку, загруженную из файла
            btnSaveToDb.Enabled = !fromDatabase;

            ShowGeneralInfo(workout);
            SetupLapFilter(workout);   // заполняет список и вызывает ShowLapAnalysis()
        }

        private void ShowGeneralInfo(Workout w)
        {
            lblActivity.Text = w.ActivityName;
            lblDate.Text = w.StartTime == DateTime.MinValue
                ? "—"
                : w.StartTime.ToString("dd.MM.yyyy HH:mm:ss") + (w.UtcOffset != null ? $" (UTC{w.UtcOffset})" : "");

            lblDuration.Text = LapDistanceAnalyzer.FormatDuration(w.Duration) +
                (w.PauseDuration is > 0 ? $" (паузы {LapDistanceAnalyzer.FormatDuration(w.PauseDuration)})" : "");

            lblDistance.Text = w.Distance > 0
                ? $"{w.Distance:N0} м ({w.Distance / 1000:0.00} км)"
                : "0 м (дистанция не измерялась)";

            // Energy хранится в джоулях; 1 ккал = 4184 Дж
            lblEnergy.Text = w.Energy.HasValue ? $"{w.Energy.Value / 4184:N0} ккал" : "—";
            lblSteps.Text = w.StepCount.HasValue ? w.StepCount.Value.ToString("N0") : "—";

            lblAscent.Text = (w.Ascent.HasValue || w.Descent.HasValue)
                ? $"+{w.Ascent ?? 0:0.#} м / −{w.Descent ?? 0:0.#} м"
                : "—";

            lblHr.Text = w.AvgHeartRate.HasValue
                ? $"{w.AvgHeartRate.Value:0} / {w.MaxHeartRate ?? 0:0} уд/мин"
                : "—";

            lblSpeed.Text = w.AvgSpeed is > 0
                ? $"{w.AvgSpeed.Value * 3.6:0.00} км/ч ({FormatPace(w.AvgSpeed.Value)} /км)"
                : "—";

            lblDevice.Text = w.DeviceName != null
                ? $"Suunto {w.DeviceName} (S/N {w.DeviceSerial}, ПО {w.DeviceSoftware ?? "?"})"
                : "—";
        }

        /// <summary>Темп (мин:сс на км) по скорости в м/с.</summary>
        private static string FormatPace(double speedMps)
        {
            if (speedMps <= 0) return "—";
            var ts = TimeSpan.FromSeconds(1000 / speedMps);
            return $"{(int)ts.TotalMinutes}:{ts.Seconds:D2}";
        }

        // ------------------------------------------------------------------
        //  Таблица анализа дистанции по кругам (вариант 9)
        // ------------------------------------------------------------------

        /// <summary>
        /// Описывает столбцы DataGridView в соответствии с показателями варианта.
        /// Столбцы задаются явно (AutoGenerateColumns = false), чтобы управлять
        /// заголовками, порядком и форматом чисел.
        /// </summary>
        private void ConfigureGridColumns()
        {
            dgvLaps.AutoGenerateColumns = false;
            dgvLaps.Columns.Clear();

            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.Number), "№ круга", 60, DataGridViewContentAlignment.MiddleCenter));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.LapType), "Тип круга", 85));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.EndTime), "Время окончания", 95, DataGridViewContentAlignment.MiddleCenter));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.DurationText), "Длительность", 85, DataGridViewContentAlignment.MiddleCenter));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.Distance), "Дистанция, м", 95, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.DeviationFromAverage), "Отклонение от средней, м", 110, DataGridViewContentAlignment.MiddleRight, "+#,0;−#,0;0"));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.DeviationFromAveragePercent), "Отклонение от средней, %", 110, DataGridViewContentAlignment.MiddleRight, "+0.0;−0.0;0.0"));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.DifferenceFromPrevious), "Разница с пред. кругом, м", 110, DataGridViewContentAlignment.MiddleRight, "+#,0;−#,0;0"));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.ShareOfTotal), "Доля от общей дистанции, %", 110, DataGridViewContentAlignment.MiddleRight, "0.0"));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.CumulativeDistance), "Нарастающим итогом, м", 110, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgvLaps.Columns.Add(TextColumn(nameof(LapDistanceRow.AverageSpeedKmh), "Ср. скорость, км/ч", 95, DataGridViewContentAlignment.MiddleRight, "0.00"));

            var note = TextColumn(nameof(LapDistanceRow.Note), "Примечание", 160);
            note.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLaps.Columns.Add(note);

            dgvLaps.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvLaps.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLaps.DefaultCellStyle.NullValue = "—";
        }

        private static DataGridViewTextBoxColumn TextColumn(string property, string header, int width,
            DataGridViewContentAlignment alignment = DataGridViewContentAlignment.MiddleLeft, string? format = null)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = header,
                Name = "col" + property,
                MinimumWidth = width,
                FillWeight = width,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            };
            col.DefaultCellStyle.Alignment = alignment;
            if (format != null)
                col.DefaultCellStyle.Format = format;
            return col;
        }

        /// <summary>Заполняет выпадающий список типов кругов, доступных в тренировке.</summary>
        private void SetupLapFilter(Workout workout)
        {
            bool hasManual = LapDistanceAnalyzer.HasManualLaps(workout.Laps);
            bool hasAuto = LapDistanceAnalyzer.HasAutoLaps(workout.Laps);

            cmbLapFilter.SelectedIndexChanged -= cmbLapFilter_SelectedIndexChanged;
            cmbLapFilter.Items.Clear();

            if (hasManual) cmbLapFilter.Items.Add(new LapFilterItem(LapFilter.Manual, "Ручные круги (Lap)"));
            if (hasAuto) cmbLapFilter.Items.Add(new LapFilterItem(LapFilter.Auto, "Автокруги (Autolap)"));
            if (hasManual && hasAuto) cmbLapFilter.Items.Add(new LapFilterItem(LapFilter.All, "Все круги (Lap + Autolap)"));

            cmbLapFilter.Enabled = cmbLapFilter.Items.Count > 0;
            if (cmbLapFilter.Items.Count > 0)
                cmbLapFilter.SelectedIndex = 0;

            cmbLapFilter.SelectedIndexChanged += cmbLapFilter_SelectedIndexChanged;

            ShowLapAnalysis();
        }

        private void cmbLapFilter_SelectedIndexChanged(object? sender, EventArgs e) => ShowLapAnalysis();

        /// <summary>Выполняет анализ и выводит таблицу и сводку.</summary>
        private void ShowLapAnalysis()
        {
            dgvLaps.DataSource = null;
            lblSummary.Text = "";

            if (_current == null) return;

            // --- Проверка наличия данных, необходимых для варианта ---
            if (_current.Laps.Count == 0)
            {
                ShowWarning($"В выбранной тренировке ({_current.ActivityName}) нет кругов: раздел \"Windows\" не содержит " +
                            "записей типа Lap или Autolap с дистанцией. Данные для анализа дистанции по кругам отсутствуют." +
                            (_current.Distance <= 0 ? " Дистанция в этой тренировке также не измерялась." : "") +
                            (_currentFromDatabase ? "" : " Общую информацию при этом можно сохранить в базу данных."));
                return;
            }

            LapFilter filter = (cmbLapFilter.SelectedItem as LapFilterItem)?.Filter ?? LapFilter.All;
            LapDistanceResult result = LapDistanceAnalyzer.Analyze(_current.Laps, filter, _current.AutoLapDistance);

            if (!result.HasData)
            {
                ShowWarning("Для выбранных кругов значение дистанции (Distance) не записано.");
                return;
            }

            // --- Вывод таблицы ---
            dgvLaps.DataSource = result.Rows;
            HighlightExtremes(result.Rows);

            // --- Сводка ---
            LapDistanceSummary s = result.Summary!;
            lblSummary.Text =
                $"Кругов: {s.Count}.  Суммарная дистанция: {s.TotalDistance:N0} м.  Средняя дистанция круга: {s.AverageDistance:N0} м.  " +
                $"Самый длинный круг: № {s.MaxLapNumber} – {s.MaxDistance:N0} м.  Самый короткий круг: № {s.MinLapNumber} – {s.MinDistance:N0} м.  " +
                $"Размах (макс − мин): {s.Spread:N0} м ({s.SpreadPercent:0.0} % от средней).  Ср. кв. отклонение: {s.StdDeviation:N0} м.";

            string info = (_currentFromDatabase ? "Данные прочитаны из базы данных. " : "Данные прочитаны из JSON-файла. ") +
                          $"Показано кругов: {s.Count}.";
            if (filter == LapFilter.All)
                info += " Внимание: ручные круги и автокруги описывают одну и ту же дистанцию, поэтому при совместном просмотре суммы дублируются.";
            if (_current.Distance > 0 && filter != LapFilter.All && Math.Abs(_current.Distance - s.TotalDistance) > 1)
                info += $" Сумма кругов ({s.TotalDistance:N0} м) отличается от общей дистанции тренировки ({_current.Distance:N0} м).";
            if (!_currentFromDatabase)
                info += " Нажмите «Сохранить в БД», чтобы сохранить тренировку.";
            ShowInfo(info);
        }

        /// <summary>Подсвечивает строки с максимальной и минимальной дистанцией.</summary>
        private void HighlightExtremes(IReadOnlyList<LapDistanceRow> rows)
        {
            for (int i = 0; i < dgvLaps.Rows.Count && i < rows.Count; i++)
            {
                if (rows[i].IsLongest)
                    dgvLaps.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(220, 245, 220);
                else if (rows[i].IsShortest)
                    dgvLaps.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(252, 226, 226);
            }
            dgvLaps.ClearSelection();
        }

        // ------------------------------------------------------------------
        //  ПР3. Обобщённый сервис Service<T> и фильтрация Predicate<T>
        // ------------------------------------------------------------------

        /// <summary>Заполняет список сущностей и настраивает вкладку при запуске.</summary>
        private void SetupEntityTab()
        {
            cmbEntityType.Items.Clear();
            cmbEntityType.Items.Add(new EntityTypeItem(typeof(Workout), "Тренировки (Service<Workout>)"));
            cmbEntityType.Items.Add(new EntityTypeItem(typeof(Lap), "Круги (Service<Lap>)"));
            cmbEntityType.SelectedIndex = 0;   // вызовет cmbEntityType_SelectedIndexChanged
        }

        /// <summary>Смена сущности: перестраиваем список фильтров, столбцы таблицы и данные.</summary>
        private void cmbEntityType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool isWorkout = SelectedEntityIsWorkout();

            cmbFilter.Items.Clear();
            if (isWorkout)
                foreach (FilterOption<Workout> f in EntityFilters.Workouts) cmbFilter.Items.Add(f);
            else
                foreach (FilterOption<Lap> f in EntityFilters.Laps) cmbFilter.Items.Add(f);
            cmbFilter.SelectedIndex = 0;

            ConfigureEntityGridColumns(isWorkout);
            RefreshEntityGrid();
        }

        private void cmbFilter_SelectedIndexChanged(object? sender, EventArgs e) => RefreshEntityGrid();

        private void btnApplyFilter_Click(object? sender, EventArgs e) => RefreshEntityGrid();

        private bool SelectedEntityIsWorkout() =>
            (cmbEntityType.SelectedItem as EntityTypeItem)?.EntityType != typeof(Lap);

        /// <summary>
        /// Получает данные через обобщённый сервис с учётом выбранного фильтра
        /// и выводит их в DataGridView.
        /// </summary>
        private void RefreshEntityGrid()
        {
            if (cmbFilter.SelectedItem == null) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                double minDistance = (double)numMinDistance.Value;
                string lambdaText;
                int total;

                if (SelectedEntityIsWorkout())
                {
                    var option = (FilterOption<Workout>)cmbFilter.SelectedItem;

                    // Итоговое условие = выбранный фильтр И дополнительное условие по дистанции.
                    // Оба условия – делегаты Predicate<Workout>, заданные лямбда-выражениями.
                    Predicate<Workout> predicate = w => option.Predicate(w) && w.Distance >= minDistance;
                    lambdaText = minDistance > 0 ? $"w => ({option.LambdaText}) && w.Distance >= {minDistance}" : option.LambdaText;

                    List<Workout> result = _workoutService.Get(predicate, w => w.Laps);   // Service<Workout>.Get(Predicate<Workout>)
                    total = _workoutService.Count();
                    dgvEntities.DataSource = result.OrderByDescending(w => w.StartTime).ToList();
                    ShowFilterResult(result.Count, total, "тренировок", lambdaText);
                }
                else
                {
                    var option = (FilterOption<Lap>)cmbFilter.SelectedItem;

                    Predicate<Lap> predicate = l => option.Predicate(l) && l.Distance >= minDistance;
                    lambdaText = minDistance > 0 ? $"l => ({option.LambdaText}) && l.Distance >= {minDistance}" : option.LambdaText;

                    List<Lap> result = _lapService.Get(predicate, l => l.Workout!);          // Service<Lap>.Get(Predicate<Lap>)
                    total = _lapService.Count();
                    dgvEntities.DataSource = result.OrderBy(l => l.WorkoutId).ThenBy(l => l.EndTime).ToList();
                    ShowFilterResult(result.Count, total, "кругов", lambdaText);
                }

                dgvEntities.ClearSelection();
            }
            catch (Exception ex)
            {
                ShowError("Не удалось получить данные через обобщённый сервис: " + RootMessage(ex));
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ShowFilterResult(int found, int total, string noun, string lambdaText)
        {
            lblFilterResult.Text = $"Найдено {found} из {total} {noun}.   Условие: {lambdaText}";
        }

        /// <summary>Кнопка «Найти по Id»: демонстрация метода GetById обобщённого сервиса.</summary>
        private void btnFindById_Click(object? sender, EventArgs e)
        {
            int id = (int)numFindId.Value;
            try
            {
                if (SelectedEntityIsWorkout())
                {
                    Workout? w = _workoutService.GetById(id, w => w.Laps);
                    dgvEntities.DataSource = w == null ? new List<Workout>() : new List<Workout> { w };
                    lblFilterResult.Text = w == null
                        ? $"Service<Workout>.GetById({id}) вернул null – тренировки с таким Id нет."
                        : $"Service<Workout>.GetById({id}): {w.ActivityName}, {w.StartTime:dd.MM.yyyy HH:mm}, {w.Distance:N0} м, кругов: {w.Laps.Count}.";
                }
                else
                {
                    Lap? l = _lapService.GetById(id, l => l.Workout!);
                    dgvEntities.DataSource = l == null ? new List<Lap>() : new List<Lap> { l };
                    lblFilterResult.Text = l == null
                        ? $"Service<Lap>.GetById({id}) вернул null – круга с таким Id нет."
                        : $"Service<Lap>.GetById({id}): круг № {l.Number} ({l.LapType}) тренировки #{l.WorkoutId}, {l.Distance:N0} м.";
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при поиске по Id: " + RootMessage(ex));
            }
        }

        /// <summary>Двойной клик по строке: открыть тренировку (для круга – его тренировку).</summary>
        private void dgvEntities_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int workoutId = dgvEntities.Rows[e.RowIndex].DataBoundItem switch
            {
                Workout w => w.Id,
                Lap l => l.WorkoutId,
                _ => 0
            };
            if (workoutId == 0) return;

            LoadWorkoutFromDatabase(workoutId);
            RefreshSavedWorkouts(selectId: workoutId);
            tabMain.SelectedTab = tabLaps;
        }

        /// <summary>Столбцы таблицы сущностей – свои для каждой сущности.</summary>
        private void ConfigureEntityGridColumns(bool isWorkout)
        {
            dgvEntities.AutoGenerateColumns = false;
            dgvEntities.DataSource = null;
            dgvEntities.Columns.Clear();

            if (isWorkout)
            {
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.Id), "Id", 45, DataGridViewContentAlignment.MiddleCenter));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.StartTime), "Дата и время", 120, DataGridViewContentAlignment.MiddleCenter, "dd.MM.yyyy HH:mm"));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.ActivityName), "Вид активности", 150));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.DurationText), "Длительность", 85, DataGridViewContentAlignment.MiddleCenter));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.Distance), "Дистанция, м", 90, DataGridViewContentAlignment.MiddleRight, "N0"));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.LapCount), "Кругов", 60, DataGridViewContentAlignment.MiddleCenter));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.StepCount), "Шагов", 70, DataGridViewContentAlignment.MiddleRight, "N0"));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.AvgHeartRate), "Пульс ср.", 70, DataGridViewContentAlignment.MiddleRight, "0"));
                dgvEntities.Columns.Add(TextColumn(nameof(Workout.Energy), "Энергия, Дж", 90, DataGridViewContentAlignment.MiddleRight, "N0"));
                var file = TextColumn(nameof(Workout.FileName), "Файл", 160);
                file.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvEntities.Columns.Add(file);
            }
            else
            {
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.Id), "Id", 45, DataGridViewContentAlignment.MiddleCenter));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.WorkoutId), "Id трен.", 60, DataGridViewContentAlignment.MiddleCenter));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.WorkoutName), "Тренировка", 160));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.Number), "№", 45, DataGridViewContentAlignment.MiddleCenter));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.LapType), "Тип", 70));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.EndTime), "Окончание", 120, DataGridViewContentAlignment.MiddleCenter, "dd.MM.yyyy HH:mm:ss"));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.Duration), "Длительность, с", 95, DataGridViewContentAlignment.MiddleRight, "0.0"));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.Distance), "Дистанция, м", 90, DataGridViewContentAlignment.MiddleRight, "N0"));
                dgvEntities.Columns.Add(TextColumn(nameof(Lap.AvgSpeedKmh), "Скорость, км/ч", 95, DataGridViewContentAlignment.MiddleRight, "0.00"));
                var hr = TextColumn(nameof(Lap.AvgHeartRate), "Пульс ср.", 80, DataGridViewContentAlignment.MiddleRight, "0");
                hr.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvEntities.Columns.Add(hr);
            }

            dgvEntities.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvEntities.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEntities.DefaultCellStyle.NullValue = "—";
        }

        // ------------------------------------------------------------------
        //  Служебные методы
        // ------------------------------------------------------------------

        /// <summary>Очищает все области вывода перед загрузкой новой тренировки.</summary>
        private void ClearWorkoutView()
        {
            _current = null;
            _currentFromDatabase = false;
            btnSaveToDb.Enabled = false;
            lblDataSource.Text = "";

            foreach (Label l in new[] { lblActivity, lblDate, lblDuration, lblDistance, lblEnergy, lblSteps, lblAscent, lblHr, lblSpeed, lblDevice })
                l.Text = "—";

            dgvLaps.DataSource = null;
            lblSummary.Text = "";
            cmbLapFilter.Items.Clear();
            cmbLapFilter.Enabled = false;
            txtMessages.Clear();
        }

        private string? SafeGetDatabasePath()
        {
            try { return _workoutService.GetDatabasePath(); }
            catch { return null; }
        }

        /// <summary>Самое вложенное сообщение исключения (у EF Core полезная причина обычно в InnerException).</summary>
        private static string RootMessage(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }

        private void ShowError(string message)
        {
            txtMessages.ForeColor = Color.Firebrick;
            txtMessages.Text = "Ошибка: " + message;
            lblFileName.ForeColor = Color.Firebrick;
        }

        private void ShowWarning(string message)
        {
            txtMessages.ForeColor = Color.DarkOrange;
            txtMessages.Text = message;
        }

        private void ShowInfo(string message)
        {
            txtMessages.ForeColor = SystemColors.ControlText;
            txtMessages.Text = message;
        }
    }
}
