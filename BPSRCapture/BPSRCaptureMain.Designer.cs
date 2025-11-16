namespace BPSRCapture
{
    partial class BPSRCaptureMain
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
            tableLayoutPanel1 = new TableLayoutPanel();
            openViewButton = new Button();
            textBox_savePath = new TextBox();
            button_selectPath = new Button();
            button_openFolder = new Button();
            checkBox_ctrl = new CheckBox();
            checkBox_shift = new CheckBox();
            checkBox_alt = new CheckBox();
            comboBox_key = new ComboBox();
            comboBox_adapter = new ComboBox();
            label1 = new Label();
            comboBox_filetype = new ComboBox();
            panel1 = new Panel();
            volumeMuteButton = new PictureBox();
            volumeSlider = new RenderedTrackBar();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeMuteButton).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 9;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Controls.Add(openViewButton, 8, 0);
            tableLayoutPanel1.Controls.Add(textBox_savePath, 0, 0);
            tableLayoutPanel1.Controls.Add(button_selectPath, 6, 0);
            tableLayoutPanel1.Controls.Add(button_openFolder, 7, 0);
            tableLayoutPanel1.Controls.Add(checkBox_ctrl, 3, 1);
            tableLayoutPanel1.Controls.Add(checkBox_shift, 4, 1);
            tableLayoutPanel1.Controls.Add(checkBox_alt, 5, 1);
            tableLayoutPanel1.Controls.Add(comboBox_key, 6, 1);
            tableLayoutPanel1.Controls.Add(comboBox_adapter, 2, 1);
            tableLayoutPanel1.Controls.Add(label1, 1, 1);
            tableLayoutPanel1.Controls.Add(comboBox_filetype, 4, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(814, 61);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // openViewButton
            // 
            openViewButton.Dock = DockStyle.Fill;
            openViewButton.Image = Properties.Resources.ListViewTable;
            openViewButton.Location = new Point(787, 3);
            openViewButton.Name = "openViewButton";
            openViewButton.Size = new Size(24, 24);
            openViewButton.TabIndex = 14;
            openViewButton.UseVisualStyleBackColor = true;
            openViewButton.Click += openViewButton_Click;
            // 
            // textBox_savePath
            // 
            tableLayoutPanel1.SetColumnSpan(textBox_savePath, 4);
            textBox_savePath.Dock = DockStyle.Fill;
            textBox_savePath.Location = new Point(3, 3);
            textBox_savePath.Name = "textBox_savePath";
            textBox_savePath.Size = new Size(508, 23);
            textBox_savePath.TabIndex = 0;
            // 
            // button_selectPath
            // 
            button_selectPath.Dock = DockStyle.Fill;
            button_selectPath.Image = Properties.Resources.Attach;
            button_selectPath.Location = new Point(637, 3);
            button_selectPath.Name = "button_selectPath";
            button_selectPath.Size = new Size(114, 24);
            button_selectPath.TabIndex = 1;
            button_selectPath.Text = "SSフォルダ選択";
            button_selectPath.TextImageRelation = TextImageRelation.ImageBeforeText;
            button_selectPath.UseVisualStyleBackColor = true;
            button_selectPath.Click += button_selectPath_Click;
            // 
            // button_openFolder
            // 
            button_openFolder.Dock = DockStyle.Fill;
            button_openFolder.Image = Properties.Resources.OpenFolder;
            button_openFolder.Location = new Point(757, 3);
            button_openFolder.Name = "button_openFolder";
            button_openFolder.Size = new Size(24, 24);
            button_openFolder.TabIndex = 2;
            button_openFolder.UseVisualStyleBackColor = true;
            button_openFolder.Click += button_openFolder_Click;
            // 
            // checkBox_ctrl
            // 
            checkBox_ctrl.AutoSize = true;
            checkBox_ctrl.Dock = DockStyle.Fill;
            checkBox_ctrl.Location = new Point(457, 33);
            checkBox_ctrl.Name = "checkBox_ctrl";
            checkBox_ctrl.Size = new Size(54, 25);
            checkBox_ctrl.TabIndex = 4;
            checkBox_ctrl.Text = "Ctrl";
            checkBox_ctrl.UseVisualStyleBackColor = true;
            // 
            // checkBox_shift
            // 
            checkBox_shift.AutoSize = true;
            checkBox_shift.Dock = DockStyle.Fill;
            checkBox_shift.Location = new Point(517, 33);
            checkBox_shift.Name = "checkBox_shift";
            checkBox_shift.Size = new Size(54, 25);
            checkBox_shift.TabIndex = 5;
            checkBox_shift.Text = "Shift";
            checkBox_shift.UseVisualStyleBackColor = true;
            // 
            // checkBox_alt
            // 
            checkBox_alt.AutoSize = true;
            checkBox_alt.Dock = DockStyle.Fill;
            checkBox_alt.Location = new Point(577, 33);
            checkBox_alt.Name = "checkBox_alt";
            checkBox_alt.Size = new Size(54, 25);
            checkBox_alt.TabIndex = 6;
            checkBox_alt.Text = "Alt";
            checkBox_alt.UseVisualStyleBackColor = true;
            // 
            // comboBox_key
            // 
            tableLayoutPanel1.SetColumnSpan(comboBox_key, 2);
            comboBox_key.Dock = DockStyle.Fill;
            comboBox_key.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_key.Location = new Point(637, 33);
            comboBox_key.Name = "comboBox_key";
            comboBox_key.Size = new Size(144, 23);
            comboBox_key.TabIndex = 9;
            // 
            // comboBox_adapter
            // 
            comboBox_adapter.Dock = DockStyle.Fill;
            comboBox_adapter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_adapter.FormattingEnabled = true;
            comboBox_adapter.Location = new Point(257, 33);
            comboBox_adapter.Margin = new Padding(3, 3, 10, 3);
            comboBox_adapter.Name = "comboBox_adapter";
            comboBox_adapter.Size = new Size(187, 23);
            comboBox_adapter.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Right;
            label1.Location = new Point(159, 30);
            label1.Name = "label1";
            label1.Size = new Size(92, 31);
            label1.TabIndex = 11;
            label1.Text = "グラフィックアダプタ";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox_filetype
            // 
            tableLayoutPanel1.SetColumnSpan(comboBox_filetype, 2);
            comboBox_filetype.Dock = DockStyle.Fill;
            comboBox_filetype.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_filetype.FormattingEnabled = true;
            comboBox_filetype.Items.AddRange(new object[] { "PNG", "JPEG", "WebP(Lossless)", "WebP(Lossy)", "Bitmap" });
            comboBox_filetype.Location = new Point(517, 3);
            comboBox_filetype.Name = "comboBox_filetype";
            comboBox_filetype.Size = new Size(114, 23);
            comboBox_filetype.TabIndex = 12;
            // 
            // panel1
            // 
            panel1.Controls.Add(volumeMuteButton);
            panel1.Controls.Add(volumeSlider);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 33);
            panel1.Name = "panel1";
            panel1.Size = new Size(148, 25);
            panel1.TabIndex = 13;
            // 
            // volumeMuteButton
            // 
            volumeMuteButton.Image = Properties.Resources.Volume;
            volumeMuteButton.Location = new Point(6, 3);
            volumeMuteButton.Margin = new Padding(0);
            volumeMuteButton.Name = "volumeMuteButton";
            volumeMuteButton.Size = new Size(16, 16);
            volumeMuteButton.TabIndex = 1;
            volumeMuteButton.TabStop = false;
            volumeMuteButton.Click += volumeMuteButton_Click;
            // 
            // volumeSlider
            // 
            volumeSlider.CurrentValue = 0;
            volumeSlider.Location = new Point(25, 3);
            volumeSlider.Name = "volumeSlider";
            volumeSlider.Size = new Size(120, 16);
            volumeSlider.TabIndex = 0;
            volumeSlider.Text = "renderedTrackBar1";
            volumeSlider.TickFrequency = 1;
            volumeSlider.TickStyle = TickStyle.None;
            volumeSlider.ValueChanged += volumeSlider_ValueChanged;
            // 
            // BPSRCaptureMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(814, 61);
            Controls.Add(tableLayoutPanel1);
            MaximumSize = new Size(830, 100);
            MinimumSize = new Size(830, 100);
            Name = "BPSRCaptureMain";
            Text = "BPSRきゃぷちゃするやつ";
            FormClosing += BPSRCapture_FormClosing;
            Shown += BPSRCaptureMain_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeMuteButton).EndInit();
            ResumeLayout(false);
        }

        #endregion


        private TableLayoutPanel tableLayoutPanel1;
        private TextBox textBox_savePath;
        private Button button_selectPath;
        private Button button_openFolder;
        private CheckBox checkBox_ctrl;
        private CheckBox checkBox_shift;
        private CheckBox checkBox_alt;
        private ComboBox comboBox_key;
        private ComboBox comboBox_adapter;
        private Label label1;
        private ComboBox comboBox_filetype;
        private Panel panel1;
        private RenderedTrackBar volumeSlider;
        private PictureBox volumeMuteButton;
        private Button openViewButton;
    }
}
