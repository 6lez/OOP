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
            lblActivityCaption = new Label();
            lblDurationCaption = new Label();
            lblEnergyCaption = new Label();
            lblAscentCaption = new Label();
            lblSpeedCaption = new Label();
            lblDeviceCaption = new Label();
            lblHrCaption = new Label();
            lblStepsCaption = new Label();
            lblDistanceCaption = new Label();
            lblDateCaption = new Label();
            lblSpeed = new Label();
            lblAscent = new Label();
            lblEnergy = new Label();
            lblDuration = new Label();
            lblActivity = new Label();
            lblDevice = new Label();
            lblHr = new Label();
            lblSteps = new Label();
            lblDistance = new Label();
            lblDate = new Label();
            grpLaps = new GroupBox();
            lblLapFilterCaption = new Label();
            lblSummary = new Label();
            cmbLapFilter = new ComboBox();
            dgvLaps = new DataGridView();
            grpMessages = new GroupBox();
            txtMessages = new TextBox();
            grpInfo.SuspendLayout();
            grpLaps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLaps).BeginInit();
            grpMessages.SuspendLayout();
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
            grpInfo.Location = new Point(12, 52);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(1060, 150);
            grpInfo.TabIndex = 3;
            grpInfo.TabStop = false;
            grpInfo.Text = "Общая информация о тренировке";
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
            // lblDurationCaption
            // 
            lblDurationCaption.AutoSize = true;
            lblDurationCaption.Location = new Point(12, 49);
            lblDurationCaption.Name = "lblDurationCaption";
            lblDurationCaption.Size = new Size(124, 15);
            lblDurationCaption.TabIndex = 1;
            lblDurationCaption.Text = "Продолжительность:";
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
            // lblAscentCaption
            // 
            lblAscentCaption.AutoSize = true;
            lblAscentCaption.Location = new Point(12, 97);
            lblAscentCaption.Name = "lblAscentCaption";
            lblAscentCaption.Size = new Size(135, 15);
            lblAscentCaption.TabIndex = 3;
            lblAscentCaption.Text = "Набор / сброс высоты:";
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
            // lblDeviceCaption
            // 
            lblDeviceCaption.AutoSize = true;
            lblDeviceCaption.Location = new Point(540, 121);
            lblDeviceCaption.Name = "lblDeviceCaption";
            lblDeviceCaption.Size = new Size(73, 15);
            lblDeviceCaption.TabIndex = 9;
            lblDeviceCaption.Text = "Устройство:";
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
            // lblDateCaption
            // 
            lblDateCaption.AutoSize = true;
            lblDateCaption.Location = new Point(540, 25);
            lblDateCaption.Name = "lblDateCaption";
            lblDateCaption.Size = new Size(124, 15);
            lblDateCaption.TabIndex = 5;
            lblDateCaption.Text = "Дата и время начала:";
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
            // grpLaps
            // 
            grpLaps.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpLaps.Controls.Add(dgvLaps);
            grpLaps.Controls.Add(cmbLapFilter);
            grpLaps.Controls.Add(lblSummary);
            grpLaps.Controls.Add(lblLapFilterCaption);
            grpLaps.Location = new Point(12, 210);
            grpLaps.Name = "grpLaps";
            grpLaps.Size = new Size(1060, 395);
            grpLaps.TabIndex = 4;
            grpLaps.TabStop = false;
            grpLaps.Text = "Анализ дистанции по кругам";
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
            // lblSummary
            // 
            lblSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSummary.Location = new Point(12, 58);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(1035, 36);
            lblSummary.TabIndex = 1;
            lblSummary.Text = "Сводка по кругам появится после загрузки файла.";
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
            dgvLaps.Size = new Size(1035, 284);
            dgvLaps.TabIndex = 3;
            // 
            // grpMessages
            // 
            grpMessages.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpMessages.Controls.Add(txtMessages);
            grpMessages.Location = new Point(12, 612);
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 711);
            Controls.Add(grpMessages);
            Controls.Add(grpLaps);
            Controls.Add(grpInfo);
            Controls.Add(lblFileName);
            Controls.Add(lblFileCaption);
            Controls.Add(btnOpenFile);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Анализ дистанции по кругам";
            grpInfo.ResumeLayout(false);
            grpInfo.PerformLayout();
            grpLaps.ResumeLayout(false);
            grpLaps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLaps).EndInit();
            grpMessages.ResumeLayout(false);
            grpMessages.PerformLayout();
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
        private GroupBox grpLaps;
        private DataGridView dgvLaps;
        private ComboBox cmbLapFilter;
        private Label lblSummary;
        private Label lblLapFilterCaption;
        private GroupBox grpMessages;
        private TextBox txtMessages;
    }
}
