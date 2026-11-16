using LapDistance.Data;
using LapDistance.Models;
using LapDistance.Services;
using System.Text.Json;

namespace LapDistance
{
    /// <summary>
    /// Главная форма приложения: выбор JSON-файла, сохранение тренировки в базу данных,
    /// выбор ранее сохранённой тренировки, вывод общей информации и таблицы
    /// анализа дистанции по кругам (вариант 9).
    ///
    /// Форма НЕ обращается к DbContext – только к методам WorkoutService,
    /// который передаётся через конструктор (Dependency Injection).
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly WorkoutService _workoutService;

        /// <summary>Текущая отображаемая тренировка (из JSON или из БД).</summary>
        private Workout? _current;

        /// <summary>Откуда получена текущая тренировка: true – из базы данных, false – из JSON-файла.</summary>
        private bool _currentFromDatabase;

        /// <summary>Элемент выпадающего списка выбора кругов.</summary>
        private sealed record LapFilterItem(LapFilter Filter, string Title)
        {
            public override string ToString() => Title;
        }

        /// <summary>Конструктор. Сервис подставляет контейнер DI (см. Program.cs).</summary>
        public MainForm(WorkoutService workoutService)
        {
            _workoutService = workoutService;
            InitializeComponent();
            ConfigureGridColumns();
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            RefreshSavedWorkouts();
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
                if (_workoutService.Exists(_current))
                {
                    DialogResult answer = MessageBox.Show(this,
                        "Тренировка с такими же датой начала и видом активности уже есть в базе данных.\n" +
                        "Сохранить ещё одну копию?",
                        "Повторное сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer != DialogResult.Yes)
                        return;
                }

                Cursor = Cursors.WaitCursor;
                int id = _workoutService.Add(_current);

                // Дальнейшая работа с тренировкой – через базу данных:
                // обновляем список и показываем только что сохранённую запись уже из БД
                RefreshSavedWorkouts(selectId: id);
                LoadWorkoutFromDatabase(id);
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
                Workout? workout = _workoutService.GetById(id);
                if (workout == null)
                {
                    ShowWarning($"Тренировка с Id = {id} не найдена в базе данных.");
                    RefreshSavedWorkouts();
                    return;
                }

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
                List<Workout> saved = _workoutService.GetAll();

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