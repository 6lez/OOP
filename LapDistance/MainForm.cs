using System.Text.Json;
using SuuntoLapDistance.Models;
using SuuntoLapDistance.Services;

namespace LapDistance
{
    /// <summary>
    /// Главная форма приложения: выбор файла, вывод общей информации
    /// о тренировке и таблицы анализа дистанции по кругам (вариант 9).
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>Загруженная тренировка (null, пока файл не выбран).</summary>
        private SuuntoWorkoutFile? _workout;

        /// <summary>Элемент выпадающего списка выбора кругов.</summary>
        private sealed record LapFilterItem(LapFilter Filter, string Title)
        {
            public override string ToString() => Title;
        }
        public MainForm()
        {
            InitializeComponent();
            ConfigureGridColumns();
        }
        // ------------------------------------------------------------------
        //  Выбор файла
        // ------------------------------------------------------------------
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            LoadWorkout(openFileDialog.FileName);
        }
        /// <summary>Читает файл, десериализует его и обновляет интерфейс.</summary>
        private void LoadWorkout(string path)
        {
            // Имя выбранного файла показываем сразу, даже если дальше будет ошибка
            lblFileName.Text = Path.GetFileName(path);
            lblFileName.ForeColor = SystemColors.ControlText;
            ClearWorkoutView();

            try
            {
                Cursor = Cursors.WaitCursor;
                _workout = WorkoutReader.Load(path);
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
                ShowError("Содержимое файла имеет некорректный формат JSON." +
                          Environment.NewLine + ex.Message);
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

            DeviceLog log = _workout.DeviceLog!;

            ShowGeneralInfo(log);
            SetupLapFilter(log);   // заполняет список и вызывает ShowLapAnalysis()
        }
        // ------------------------------------------------------------------
        //  Общая информация о тренировке
        // ------------------------------------------------------------------

        private void ShowGeneralInfo(DeviceLog log)
        {
            WorkoutHeader h = log.Header!;

            lblActivity.Text = ActivityTypes.Describe(h);
            lblDate.Text = h.StartTime?.ToString("dd.MM.yyyy HH:mm:ss") ?? "—";

            lblDuration.Text = h.Duration.HasValue
                ? LapDistanceAnalyzer.FormatDuration(h.Duration) +
                  (h.PauseDuration is > 0 ? $" (паузы {LapDistanceAnalyzer.FormatDuration(h.PauseDuration)})" : "")
                : "—";

            lblDistance.Text = h.Distance is > 0
                ? $"{h.Distance.Value:N0} м ({h.Distance.Value / 1000:0.00} км)"
                : (h.Distance.HasValue ? "0 м (дистанция не измерялась)" : "—");

            // Energy хранится в джоулях; 1 ккал = 4184 Дж
            lblEnergy.Text = h.Energy.HasValue ? $"{h.Energy.Value / 4184:N0} ккал" : "—";
            lblSteps.Text = h.StepCount.HasValue ? h.StepCount.Value.ToString("N0") : "—";

            lblAscent.Text = (h.Ascent.HasValue || h.Descent.HasValue)
                ? $"+{h.Ascent ?? 0:0.#} м / −{h.Descent ?? 0:0.#} м"
                : "—";

            // Пульс в окнах хранится в герцах; средний и максимальный берём из окна "Activity"
            WorkoutWindow? activity = log.Windows?
                .Select(w => w.Window)
                .FirstOrDefault(w => w != null && string.Equals(w.Type, WorkoutWindow.TypeActivity, StringComparison.OrdinalIgnoreCase));
            MinMaxAvg? hr = activity?.HR?.FirstOrDefault();
            lblHr.Text = hr?.Avg != null
                ? $"{hr.Avg.Value * 60:0} / {(hr.Max ?? 0) * 60:0} уд/мин"
                : "—";

            double? speed = activity?.Speed?.FirstOrDefault()?.Avg;
            if (speed == null && h.Distance is > 0 && h.Duration is > 0)
                speed = h.Distance / h.Duration;
            lblSpeed.Text = speed is > 0
                ? $"{speed.Value * 3.6:0.00} км/ч ({FormatPace(speed.Value)} /км)"
                : "—";

            DeviceInfo? dev = log.Device ?? h.Device;
            lblDevice.Text = dev != null
                ? $"Suunto {dev.Name} (S/N {dev.SerialNumber}, ПО {dev.Info?.Software ?? "?"})"
                : "—";
        }

        /// <summary>Темп (мин:сс на км) по скорости в м/с.</summary>
        private static string FormatPace(double speedMps)
        {
            if (speedMps <= 0) return "—";
            double secPerKm = 1000 / speedMps;
            var ts = TimeSpan.FromSeconds(secPerKm);
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

        /// <summary>Заполняет выпадающий список типов кругов, доступных в файле.</summary>
        private void SetupLapFilter(DeviceLog log)
        {
            bool hasManual = LapDistanceAnalyzer.HasManualLaps(log);
            bool hasAuto = LapDistanceAnalyzer.HasAutoLaps(log);

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

            DeviceLog? log = _workout?.DeviceLog;
            if (log == null) return;

            // --- Проверка наличия данных, необходимых для варианта ---
            if (log.Windows == null || log.Windows.Count == 0)
            {
                ShowWarning("В файле отсутствует раздел \"Windows\" – данные о кругах недоступны, " +
                            "анализ дистанции по кругам невозможен.");
                return;
            }

            if (!LapDistanceAnalyzer.HasManualLaps(log) && !LapDistanceAnalyzer.HasAutoLaps(log))
            {
                string activity = ActivityTypes.Describe(log.Header!);
                ShowWarning($"В выбранной тренировке ({activity}) нет кругов: раздел \"Windows\" содержит только " +
                            "сводные окна (Activity/Move) без записей типа Lap или Autolap. " +
                            "Данные для анализа дистанции по кругам отсутствуют." +
                            (log.Header?.Distance is null or 0 ? " Дистанция в этой тренировке также не измерялась." : ""));
                return;
            }

            LapFilter filter = (cmbLapFilter.SelectedItem as LapFilterItem)?.Filter ?? LapFilter.All;
            LapDistanceResult result = LapDistanceAnalyzer.Analyze(log, filter);

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
            string summary =
                $"Кругов: {s.Count}.  Суммарная дистанция: {s.TotalDistance:N0} м.  Средняя дистанция круга: {s.AverageDistance:N0} м.  " +
                $"Самый длинный круг: № {s.MaxLapNumber} – {s.MaxDistance:N0} м.  Самый короткий круг: № {s.MinLapNumber} – {s.MinDistance:N0} м.  " +
                $"Размах (макс − мин): {s.Spread:N0} м ({s.SpreadPercent:0.0} % от средней).  Ср. кв. отклонение: {s.StdDeviation:N0} м.";
            lblSummary.Text = summary;

            string info = $"Загружено кругов: {s.Count}.";
            if (filter == LapFilter.All)
                info += " Внимание: ручные круги и автокруги описывают одну и ту же дистанцию, поэтому при совместном просмотре суммы дублируются.";
            double? headerDistance = log.Header?.Distance;
            if (headerDistance is > 0 && filter != LapFilter.All && Math.Abs(headerDistance.Value - s.TotalDistance) > 1)
                info += $" Сумма кругов ({s.TotalDistance:N0} м) отличается от общей дистанции тренировки ({headerDistance.Value:N0} м).";
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

        /// <summary>Очищает все области вывода перед загрузкой нового файла.</summary>
        private void ClearWorkoutView()
        {
            _workout = null;
            foreach (Label l in new[] { lblActivity, lblDate, lblDuration, lblDistance, lblEnergy, lblSteps, lblAscent, lblHr, lblSpeed, lblDevice })
                l.Text = "—";

            dgvLaps.DataSource = null;
            lblSummary.Text = "";
            cmbLapFilter.Items.Clear();
            cmbLapFilter.Enabled = false;
            txtMessages.Clear();
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
