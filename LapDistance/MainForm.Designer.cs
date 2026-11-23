namespace LapDistance
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOpenFile = new Button();
            lblFileCaption = new Label();
            lblFileName = new Label();
            openFileDialog = new OpenFileDialog();
            grpInfo = new GroupBox();
            lblDevice = new Label();
            lblHr = new Label();
            lblSteps = new Label();
            lblDistance = new Label();
            lblDate = new Label();
            lblSpeed = new Label();
            lblDeviceCaption = new Label();
            lblAscent = new Label();
            lblSpeedCaption = new Label();
            lblEnergy = new Label();
            lblDuration = new Label();
            lblHrCaption = new Label();
            lblActivity = new Label();
            lblAscentCaption = new Label();
            lblStepsCaption = new Label();
            lblDistanceCaption = new Label();
            lblEnergyCaption = new Label();
            lblDateCaption = new Label();
            lblDurationCaption = new Label();
            lblActivityCaption = new Label();
            grpMessages = new GroupBox();
            txtMessages = new TextBox();
            btnSaveToDb = new Button();
            lblDataSource = new Label();
            lblSavedCaption = new Label();
            cmbSavedWorkouts = new ComboBox();
            btnLoadFromDb = new Button();
            tabMain = new TabControl();
            tabLaps = new TabPage();
            grpLaps = new GroupBox();
            dgvLaps = new DataGridView();
            cmbLapFilter = new ComboBox();
            lblSummary = new Label();
            lblLapFilterCaption = new Label();
            tabService = new TabPage();
            dgvEntities = new DataGridView();
            numFindId = new NumericUpDown();
            lblFilterResult = new Label();
            lblFindCaption = new Label();
            btnFindById = new Button();
            btnApplyFilter = new Button();
            numMinDistance = new NumericUpDown();
            lblMinDistance = new Label();
            lblFilterCaption = new Label();
            cmbFilter = new ComboBox();
            cmbEntityType = new ComboBox();
            lblEntityCaption = new Label();
            grpInfo.SuspendLayout();
            grpMessages.SuspendLayout();
            tabMain.SuspendLayout();
            tabLaps.SuspendLayout();
            grpLaps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLaps).BeginInit();
            tabService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEntities).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFindId).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinDistance).BeginInit();
            SuspendLayout();
            // 
            // btnOpenFile
            // 
            btnOpenFile.Location = new Point(12, 12);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(210, 32);
            btnOpenFile.TabIndex = 0;
            btnOpenFile.Text = "Открыть файл тренировки";
            btnOpenFile.UseVisualStyleBackColor = true;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // lblFileCaption
            // 
            lblFileCaption.AutoSize = true;
            lblFileCaption.Location = new Point(235, 20);
            lblFileCaption.Name = "lblFileCaption";
            lblFileCaption.Size = new Size(39, 15);
            lblFileCaption.TabIndex = 1;
            lblFileCaption.Text = "Файл:";
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblFileName.ForeColor = SystemColors.GrayText;
            lblFileName.Location = new Point(280, 20);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(101, 15);
            lblFileName.TabIndex = 2;
            lblFileName.Text = "файл не выбран";
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "json";
            openFileDialog.FileName = "openFileDialog1";
            openFileDialog.Filter = "Файлы тренировок JSON (*.json)|*.json";
            openFileDialog.Title = "Выберите файл тренировки";
            // 
            // grpInfo
            // 
            grpInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpInfo.Controls.Add(lblDevice);
            grpInfo.Controls.Add(lblHr);
            grpInfo.Controls.Add(lblSteps);
            grpInfo.Controls.Add(lblDistance);
            grpInfo.Controls.Add(lblDate);
            grpInfo.Controls.Add(lblSpeed);
            grpInfo.Controls.Add(lblDeviceCaption);
            grpInfo.Controls.Add(lblAscent);
            grpInfo.Controls.Add(lblSpeedCaption);
            grpInfo.Controls.Add(lblEnergy);
            grpInfo.Controls.Add(lblDuration);
            grpInfo.Controls.Add(lblHrCaption);
            grpInfo.Controls.Add(lblActivity);
            grpInfo.Controls.Add(lblAscentCaption);
            grpInfo.Controls.Add(lblStepsCaption);
            grpInfo.Controls.Add(lblDistanceCaption);
            grpInfo.Controls.Add(lblEnergyCaption);
            grpInfo.Controls.Add(lblDateCaption);
            grpInfo.Controls.Add(lblDurationCaption);
            grpInfo.Controls.Add(lblActivityCaption);
            grpInfo.Location = new Point(12, 115);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(1060, 150);
            grpInfo.TabIndex = 3;
            grpInfo.TabStop = false;
            grpInfo.Text = "Общая информация о тренировке";
            // 
            // lblDevice
            // 
            lblDevice.AutoSize = true;
            lblDevice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDevice.Location = new Point(720, 121);
            lblDevice.Name = "lblDevice";
            lblDevice.Size = new Size(19, 15);
            lblDevice.TabIndex = 14;
            lblDevice.Text = "—";
            // 
            // lblHr
            // 
            lblHr.AutoSize = true;
            lblHr.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHr.Location = new Point(720, 97);
            lblHr.Name = "lblHr";
            lblHr.Size = new Size(19, 15);
            lblHr.TabIndex = 13;
            lblHr.Text = "—";
            // 
            // lblSteps
            // 
            lblSteps.AutoSize = true;
            lblSteps.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSteps.Location = new Point(720, 73);
            lblSteps.Name = "lblSteps";
            lblSteps.Size = new Size(19, 15);
            lblSteps.TabIndex = 12;
            lblSteps.Text = "—";
            // 
            // lblDistance
            // 
            lblDistance.AutoSize = true;
            lblDistance.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDistance.Location = new Point(720, 49);
            lblDistance.Name = "lblDistance";
            lblDistance.Size = new Size(19, 15);
            lblDistance.TabIndex = 11;
            lblDistance.Text = "—";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDate.Location = new Point(720, 25);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(19, 15);
            lblDate.TabIndex = 10;
            lblDate.Text = "—";
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSpeed.Location = new Point(190, 121);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(19, 15);
            lblSpeed.TabIndex = 9;
            lblSpeed.Text = "—";
            // 
            // lblDeviceCaption
            // 
            lblDeviceCaption.AutoSize = true;
            lblDeviceCaption.Location = new Point(540, 121);
            lblDeviceCaption.Name = "lblDeviceCaption";
            lblDeviceCaption.Size = new Size(73, 15);
            lblDeviceCaption.TabIndex = 9;
            lblDeviceCaption.Text = "Устройство:";
            // 
            // lblAscent
            // 
            lblAscent.AutoSize = true;
            lblAscent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAscent.Location = new Point(190, 97);
            lblAscent.Name = "lblAscent";
            lblAscent.Size = new Size(19, 15);
            lblAscent.TabIndex = 8;
            lblAscent.Text = "—";
            // 
            // lblSpeedCaption
            // 
            lblSpeedCaption.AutoSize = true;
            lblSpeedCaption.Location = new Point(12, 121);
            lblSpeedCaption.Name = "lblSpeedCaption";
            lblSpeedCaption.Size = new Size(147, 15);
            lblSpeedCaption.TabIndex = 4;
            lblSpeedCaption.Text = "Средняя скорость / темп:";
            // 
            // lblEnergy
            // 
            lblEnergy.AutoSize = true;
            lblEnergy.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEnergy.Location = new Point(190, 73);
            lblEnergy.Name = "lblEnergy";
            lblEnergy.Size = new Size(19, 15);
            lblEnergy.TabIndex = 7;
            lblEnergy.Text = "—";
            // 
            // lblDuration
            // 
            lblDuration.AutoSize = true;
            lblDuration.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDuration.Location = new Point(190, 49);
            lblDuration.Name = "lblDuration";
            lblDuration.Size = new Size(19, 15);
            lblDuration.TabIndex = 6;
            lblDuration.Text = "—";
            // 
            // lblHrCaption
            // 
            lblHrCaption.AutoSize = true;
            lblHrCaption.Location = new Point(540, 97);
            lblHrCaption.Name = "lblHrCaption";
            lblHrCaption.Size = new Size(142, 15);
            lblHrCaption.TabIndex = 8;
            lblHrCaption.Text = "Пульс (средний / макс.):";
            // 
            // lblActivity
            // 
            lblActivity.AutoSize = true;
            lblActivity.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblActivity.Location = new Point(190, 25);
            lblActivity.Name = "lblActivity";
            lblActivity.Size = new Size(19, 15);
            lblActivity.TabIndex = 5;
            lblActivity.Text = "—";
            // 
            // lblAscentCaption
            // 
            lblAscentCaption.AutoSize = true;
            lblAscentCaption.Location = new Point(12, 97);
            lblAscentCaption.Name = "lblAscentCaption";
            lblAscentCaption.Size = new Size(135, 15);
            lblAscentCaption.TabIndex = 3;
            lblAscentCaption.Text = "Набор / сброс высоты:";
            // 
            // lblStepsCaption
            // 
            lblStepsCaption.AutoSize = true;
            lblStepsCaption.Location = new Point(540, 73);
            lblStepsCaption.Name = "lblStepsCaption";
            lblStepsCaption.Size = new Size(113, 15);
            lblStepsCaption.TabIndex = 7;
            lblStepsCaption.Text = "Количество шагов:";
            // 
            // lblDistanceCaption
            // 
            lblDistanceCaption.AutoSize = true;
            lblDistanceCaption.Location = new Point(540, 49);
            lblDistanceCaption.Name = "lblDistanceCaption";
            lblDistanceCaption.Size = new Size(69, 15);
            lblDistanceCaption.TabIndex = 6;
            lblDistanceCaption.Text = "Дистанция:";
            // 
            // lblEnergyCaption
            // 
            lblEnergyCaption.AutoSize = true;
            lblEnergyCaption.Location = new Point(12, 73);
            lblEnergyCaption.Name = "lblEnergyCaption";
            lblEnergyCaption.Size = new Size(92, 15);
            lblEnergyCaption.TabIndex = 2;
            lblEnergyCaption.Text = "Энергозатраты:";
            // 
            // lblDateCaption
            // 
            lblDateCaption.AutoSize = true;
            lblDateCaption.Location = new Point(540, 25);
            lblDateCaption.Name = "lblDateCaption";
            lblDateCaption.Size = new Size(124, 15);
            lblDateCaption.TabIndex = 5;
            lblDateCaption.Text = "Дата и время начала:";
            // 
            // lblDurationCaption
            // 
            lblDurationCaption.AutoSize = true;
            lblDurationCaption.Location = new Point(12, 49);
            lblDurationCaption.Name = "lblDurationCaption";
            lblDurationCaption.Size = new Size(124, 15);
            lblDurationCaption.TabIndex = 1;
            lblDurationCaption.Text = "Продолжительность:";
            // 
            // lblActivityCaption
            // 
            lblActivityCaption.AutoSize = true;
            lblActivityCaption.Location = new Point(12, 25);
            lblActivityCaption.Name = "lblActivityCaption";
            lblActivityCaption.Size = new Size(95, 15);
            lblActivityCaption.TabIndex = 0;
            lblActivityCaption.Text = "Вид активности:";
            // 
            // grpMessages
            // 
            grpMessages.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpMessages.Controls.Add(txtMessages);
            grpMessages.Location = new Point(12, 703);
            grpMessages.Name = "grpMessages";
            grpMessages.Size = new Size(1060, 88);
            grpMessages.TabIndex = 5;
            grpMessages.TabStop = false;
            grpMessages.Text = "Сообщения";
            // 
            // txtMessages
            // 
            txtMessages.BackColor = SystemColors.Window;
            txtMessages.Dock = DockStyle.Fill;
            txtMessages.Location = new Point(3, 19);
            txtMessages.Multiline = true;
            txtMessages.Name = "txtMessages";
            txtMessages.ReadOnly = true;
            txtMessages.ScrollBars = ScrollBars.Vertical;
            txtMessages.Size = new Size(1054, 66);
            txtMessages.TabIndex = 0;
            txtMessages.Text = "Выберите JSON-файл тренировки с помощью кнопки «Открыть файл тренировки...».";
            // 
            // btnSaveToDb
            // 
            btnSaveToDb.Enabled = false;
            btnSaveToDb.Location = new Point(440, 12);
            btnSaveToDb.Name = "btnSaveToDb";
            btnSaveToDb.Size = new Size(140, 32);
            btnSaveToDb.TabIndex = 6;
            btnSaveToDb.Text = "Сохранить в БД";
            btnSaveToDb.UseVisualStyleBackColor = true;
            btnSaveToDb.Click += btnSaveToDb_Click;
            // 
            // lblDataSource
            // 
            lblDataSource.AutoSize = true;
            lblDataSource.Location = new Point(595, 20);
            lblDataSource.Name = "lblDataSource";
            lblDataSource.Size = new Size(0, 15);
            lblDataSource.TabIndex = 7;
            // 
            // lblSavedCaption
            // 
            lblSavedCaption.AutoSize = true;
            lblSavedCaption.Location = new Point(12, 60);
            lblSavedCaption.Name = "lblSavedCaption";
            lblSavedCaption.Size = new Size(154, 15);
            lblSavedCaption.TabIndex = 8;
            lblSavedCaption.Text = "Сохранённые тренировки:";
            // 
            // cmbSavedWorkouts
            // 
            cmbSavedWorkouts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbSavedWorkouts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSavedWorkouts.Enabled = false;
            cmbSavedWorkouts.FormattingEnabled = true;
            cmbSavedWorkouts.Location = new Point(191, 57);
            cmbSavedWorkouts.Name = "cmbSavedWorkouts";
            cmbSavedWorkouts.Size = new Size(560, 23);
            cmbSavedWorkouts.TabIndex = 9;
            // 
            // btnLoadFromDb
            // 
            btnLoadFromDb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadFromDb.Enabled = false;
            btnLoadFromDb.Location = new Point(758, 51);
            btnLoadFromDb.Name = "btnLoadFromDb";
            btnLoadFromDb.Size = new Size(150, 32);
            btnLoadFromDb.TabIndex = 10;
            btnLoadFromDb.Text = "Загрузить из БД";
            btnLoadFromDb.UseVisualStyleBackColor = true;
            btnLoadFromDb.Click += btnLoadFromDb_Click;
            // 
            // tabMain
            // 
            tabMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Controls.Add(tabLaps);
            tabMain.Controls.Add(tabService);
            tabMain.Location = new Point(12, 271);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1060, 425);
            tabMain.TabIndex = 11;
            // 
            // tabLaps
            // 
            tabLaps.Controls.Add(grpLaps);
            tabLaps.Location = new Point(4, 24);
            tabLaps.Name = "tabLaps";
            tabLaps.Padding = new Padding(3);
            tabLaps.Size = new Size(1052, 397);
            tabLaps.TabIndex = 0;
            tabLaps.Text = "Анализ дистанции по кругам";
            tabLaps.UseVisualStyleBackColor = true;
            // 
            // grpLaps
            // 
            grpLaps.Controls.Add(dgvLaps);
            grpLaps.Controls.Add(cmbLapFilter);
            grpLaps.Controls.Add(lblSummary);
            grpLaps.Controls.Add(lblLapFilterCaption);
            grpLaps.Dock = DockStyle.Fill;
            grpLaps.Location = new Point(3, 3);
            grpLaps.Name = "grpLaps";
            grpLaps.Size = new Size(1046, 391);
            grpLaps.TabIndex = 5;
            grpLaps.TabStop = false;
            grpLaps.Text = "Анализ дистанции по кругам";
            // 
            // dgvLaps
            // 
            dgvLaps.AllowUserToAddRows = false;
            dgvLaps.AllowUserToDeleteRows = false;
            dgvLaps.AllowUserToResizeColumns = false;
            dgvLaps.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLaps.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLaps.BackgroundColor = SystemColors.Window;
            dgvLaps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLaps.Location = new Point(12, 98);
            dgvLaps.MultiSelect = false;
            dgvLaps.Name = "dgvLaps";
            dgvLaps.ReadOnly = true;
            dgvLaps.RowHeadersVisible = false;
            dgvLaps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLaps.Size = new Size(1881, 644);
            dgvLaps.TabIndex = 3;
            // 
            // cmbLapFilter
            // 
            cmbLapFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLapFilter.Enabled = false;
            cmbLapFilter.FormattingEnabled = true;
            cmbLapFilter.Location = new Point(160, 23);
            cmbLapFilter.Name = "cmbLapFilter";
            cmbLapFilter.Size = new Size(121, 23);
            cmbLapFilter.TabIndex = 2;
            cmbLapFilter.SelectedIndexChanged += cmbLapFilter_SelectedIndexChanged;
            // 
            // lblSummary
            // 
            lblSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSummary.Location = new Point(12, 58);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(1881, 36);
            lblSummary.TabIndex = 1;
            lblSummary.Text = "Сводка по кругам появится после загрузки файла.";
            // 
            // lblLapFilterCaption
            // 
            lblLapFilterCaption.AutoSize = true;
            lblLapFilterCaption.Location = new Point(12, 27);
            lblLapFilterCaption.Name = "lblLapFilterCaption";
            lblLapFilterCaption.Size = new Size(134, 15);
            lblLapFilterCaption.TabIndex = 0;
            lblLapFilterCaption.Text = "Анализируемые круги:";
            // 
            // tabService
            // 
            tabService.Controls.Add(dgvEntities);
            tabService.Controls.Add(numFindId);
            tabService.Controls.Add(lblFilterResult);
            tabService.Controls.Add(lblFindCaption);
            tabService.Controls.Add(btnFindById);
            tabService.Controls.Add(btnApplyFilter);
            tabService.Controls.Add(numMinDistance);
            tabService.Controls.Add(lblMinDistance);
            tabService.Controls.Add(lblFilterCaption);
            tabService.Controls.Add(cmbFilter);
            tabService.Controls.Add(cmbEntityType);
            tabService.Controls.Add(lblEntityCaption);
            tabService.Location = new Point(4, 24);
            tabService.Name = "tabService";
            tabService.Padding = new Padding(3);
            tabService.Size = new Size(1052, 397);
            tabService.TabIndex = 1;
            tabService.Text = "Обобщённый сервис Service<T> и фильтрация";
            tabService.UseVisualStyleBackColor = true;
            // 
            // dgvEntities
            // 
            dgvEntities.AllowUserToAddRows = false;
            dgvEntities.AllowUserToDeleteRows = false;
            dgvEntities.AllowUserToResizeRows = false;
            dgvEntities.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEntities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEntities.BackgroundColor = SystemColors.Window;
            dgvEntities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEntities.Location = new Point(8, 76);
            dgvEntities.MultiSelect = false;
            dgvEntities.Name = "dgvEntities";
            dgvEntities.ReadOnly = true;
            dgvEntities.RowHeadersVisible = false;
            dgvEntities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEntities.Size = new Size(1036, 312);
            dgvEntities.TabIndex = 12;
            dgvEntities.CellDoubleClick += dgvEntities_CellDoubleClick;
            // 
            // numFindId
            // 
            numFindId.Location = new Point(90, 42);
            numFindId.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numFindId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFindId.Name = "numFindId";
            numFindId.Size = new Size(70, 23);
            numFindId.TabIndex = 11;
            numFindId.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFilterResult
            // 
            lblFilterResult.AutoSize = true;
            lblFilterResult.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblFilterResult.Location = new Point(275, 46);
            lblFilterResult.Name = "lblFilterResult";
            lblFilterResult.Size = new Size(0, 15);
            lblFilterResult.TabIndex = 10;
            // 
            // lblFindCaption
            // 
            lblFindCaption.AutoSize = true;
            lblFindCaption.Location = new Point(8, 46);
            lblFindCaption.Name = "lblFindCaption";
            lblFindCaption.Size = new Size(74, 15);
            lblFindCaption.TabIndex = 9;
            lblFindCaption.Text = "Найти по Id:";
            // 
            // btnFindById
            // 
            btnFindById.Location = new Point(168, 40);
            btnFindById.Name = "btnFindById";
            btnFindById.Size = new Size(90, 27);
            btnFindById.TabIndex = 8;
            btnFindById.Text = "Найти";
            btnFindById.UseVisualStyleBackColor = true;
            btnFindById.Click += btnFindById_Click;
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.Location = new Point(951, 5);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(95, 27);
            btnApplyFilter.TabIndex = 7;
            btnApplyFilter.Text = "Применить";
            btnApplyFilter.UseVisualStyleBackColor = true;
            btnApplyFilter.Click += btnApplyFilter_Click;
            // 
            // numMinDistance
            // 
            numMinDistance.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numMinDistance.Location = new Point(857, 8);
            numMinDistance.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMinDistance.Name = "numMinDistance";
            numMinDistance.Size = new Size(80, 23);
            numMinDistance.TabIndex = 6;
            numMinDistance.TextAlign = HorizontalAlignment.Right;
            numMinDistance.ThousandsSeparator = true;
            // 
            // lblMinDistance
            // 
            lblMinDistance.AutoSize = true;
            lblMinDistance.Location = new Point(706, 12);
            lblMinDistance.Name = "lblMinDistance";
            lblMinDistance.Size = new Size(145, 15);
            lblMinDistance.TabIndex = 5;
            lblMinDistance.Text = "и дистанция не менее, м:";
            // 
            // lblFilterCaption
            // 
            lblFilterCaption.AutoSize = true;
            lblFilterCaption.Location = new Point(291, 12);
            lblFilterCaption.Name = "lblFilterCaption";
            lblFilterCaption.Size = new Size(133, 15);
            lblFilterCaption.TabIndex = 4;
            lblFilterCaption.Text = "Фильтр (Predicate<T>):";
            // 
            // cmbFilter
            // 
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(430, 8);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(270, 23);
            cmbFilter.TabIndex = 2;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            // 
            // cmbEntityType
            // 
            cmbEntityType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEntityType.FormattingEnabled = true;
            cmbEntityType.Location = new Point(75, 8);
            cmbEntityType.Name = "cmbEntityType";
            cmbEntityType.Size = new Size(210, 23);
            cmbEntityType.TabIndex = 1;
            cmbEntityType.SelectedIndexChanged += cmbEntityType_SelectedIndexChanged;
            // 
            // lblEntityCaption
            // 
            lblEntityCaption.AutoSize = true;
            lblEntityCaption.Location = new Point(8, 12);
            lblEntityCaption.Name = "lblEntityCaption";
            lblEntityCaption.Size = new Size(66, 15);
            lblEntityCaption.TabIndex = 0;
            lblEntityCaption.Text = "Сущность:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 841);
            Controls.Add(tabMain);
            Controls.Add(btnLoadFromDb);
            Controls.Add(cmbSavedWorkouts);
            Controls.Add(lblSavedCaption);
            Controls.Add(lblDataSource);
            Controls.Add(btnSaveToDb);
            Controls.Add(grpMessages);
            Controls.Add(grpInfo);
            Controls.Add(lblFileName);
            Controls.Add(lblFileCaption);
            Controls.Add(btnOpenFile);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Анализ дистанции по кругам";
            Load += MainForm_Load;
            grpInfo.ResumeLayout(false);
            grpInfo.PerformLayout();
            grpMessages.ResumeLayout(false);
            grpMessages.PerformLayout();
            tabMain.ResumeLayout(false);
            tabLaps.ResumeLayout(false);
            grpLaps.ResumeLayout(false);
            grpLaps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLaps).EndInit();
            tabService.ResumeLayout(false);
            tabService.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEntities).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFindId).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinDistance).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOpenFile;
        private Label lblFileCaption;
        private Label lblFileName;
        private OpenFileDialog openFileDialog;
        private GroupBox grpInfo;
        private Label lblDevice;
        private Label lblHr;
        private Label lblSteps;
        private Label lblDistance;
        private Label lblDate;
        private Label lblSpeed;
        private Label lblDeviceCaption;
        private Label lblAscent;
        private Label lblSpeedCaption;
        private Label lblEnergy;
        private Label lblDuration;
        private Label lblHrCaption;
        private Label lblActivity;
        private Label lblAscentCaption;
        private Label lblStepsCaption;
        private Label lblDistanceCaption;
        private Label lblEnergyCaption;
        private Label lblDateCaption;
        private Label lblDurationCaption;
        private Label lblActivityCaption;
        private GroupBox grpMessages;
        private TextBox txtMessages;
        private Button btnSaveToDb;
        private Label lblDataSource;
        private Label lblSavedCaption;
        private ComboBox cmbSavedWorkouts;
        private Button btnLoadFromDb;
        private TabControl tabMain;
        private TabPage tabLaps;
        private TabPage tabService;
        private GroupBox grpLaps;
        private DataGridView dgvLaps;
        private ComboBox cmbLapFilter;
        private Label lblSummary;
        private Label lblLapFilterCaption;
        private Label lblMinDistance;
        private Label lblFilterCaption;
        private ComboBox comboBox3;
        private ComboBox cmbFilter;
        private ComboBox cmbEntityType;
        private Label lblEntityCaption;
        private Button btnApplyFilter;
        private NumericUpDown numMinDistance;
        private NumericUpDown numFindId;
        private Label lblFilterResult;
        private Label lblFindCaption;
        private Button btnFindById;
        private DataGridView dgvEntities;
    }
}
