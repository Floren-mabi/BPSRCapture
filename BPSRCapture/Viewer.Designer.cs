namespace BPSRCapture
{
    partial class Viewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            itemListView = new ListView();
            contextMenu = new ContextMenuStrip(components);
            toolStripMenuItem_RotateRight90 = new ToolStripMenuItem();
            ToolStripMenuItem_RotateLeft90 = new ToolStripMenuItem();
            panel1 = new Panel();
            label1 = new Label();
            recentLimit = new NumericUpDown();
            recentChk = new CheckBox();
            fileSystemWatcher = new FileSystemWatcher();
            thumbsList = new ImageList(components);
            _debounceTimer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            contextMenu.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)recentLimit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(itemListView, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Size = new Size(784, 561);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // itemListView
            // 
            itemListView.ContextMenuStrip = contextMenu;
            itemListView.Dock = DockStyle.Fill;
            itemListView.Location = new Point(3, 3);
            itemListView.MultiSelect = false;
            itemListView.Name = "itemListView";
            itemListView.Size = new Size(778, 525);
            itemListView.TabIndex = 1;
            itemListView.UseCompatibleStateImageBehavior = false;
            itemListView.DoubleClick += itemListView_DoubleClick;
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_RotateRight90, ToolStripMenuItem_RotateLeft90 });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(145, 48);
            contextMenu.Opening += contextMenu_Opening;
            // 
            // toolStripMenuItem_RotateRight90
            // 
            toolStripMenuItem_RotateRight90.Name = "toolStripMenuItem_RotateRight90";
            toolStripMenuItem_RotateRight90.Size = new Size(144, 22);
            toolStripMenuItem_RotateRight90.Text = "右へ90度回転";
            toolStripMenuItem_RotateRight90.Click += toolStripMenuItem_RotateRight90_Click;
            // 
            // ToolStripMenuItem_RotateLeft90
            // 
            ToolStripMenuItem_RotateLeft90.Name = "ToolStripMenuItem_RotateLeft90";
            ToolStripMenuItem_RotateLeft90.Size = new Size(144, 22);
            ToolStripMenuItem_RotateLeft90.Text = "左へ90度回転";
            ToolStripMenuItem_RotateLeft90.Click += ToolStripMenuItem_RotateLeft90_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(recentLimit);
            panel1.Controls.Add(recentChk);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 534);
            panel1.Name = "panel1";
            panel1.Size = new Size(778, 24);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(115, 4);
            label1.Name = "label1";
            label1.Size = new Size(96, 15);
            label1.TabIndex = 3;
            label1.Text = "件のみ表示(推奨)";
            // 
            // recentLimit
            // 
            recentLimit.Location = new Point(59, 1);
            recentLimit.Name = "recentLimit";
            recentLimit.Size = new Size(50, 23);
            recentLimit.TabIndex = 2;
            recentLimit.ValueChanged += recentLimit_ValueChanged;
            // 
            // recentChk
            // 
            recentChk.AutoSize = true;
            recentChk.Location = new Point(3, 3);
            recentChk.Name = "recentChk";
            recentChk.Size = new Size(50, 19);
            recentChk.TabIndex = 1;
            recentChk.Text = "直近";
            recentChk.UseVisualStyleBackColor = true;
            recentChk.CheckedChanged += recentChk_CheckedChanged;
            // 
            // fileSystemWatcher
            // 
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.SynchronizingObject = this;
            // 
            // thumbsList
            // 
            thumbsList.ColorDepth = ColorDepth.Depth32Bit;
            thumbsList.ImageSize = new Size(16, 16);
            thumbsList.TransparentColor = Color.Transparent;
            // 
            // _debounceTimer
            // 
            _debounceTimer.Tick += _debounceTimer_Tick;
            // 
            // Viewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(tableLayoutPanel1);
            Name = "Viewer";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Load += Viewer_Load;
            Shown += Viewer_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            contextMenu.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)recentLimit).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private ListView itemListView;
        private Panel panel1;
        private FileSystemWatcher fileSystemWatcher;
        private ImageList thumbsList;
        private Label label1;
        internal NumericUpDown recentLimit;
        internal CheckBox recentChk;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem toolStripMenuItem_RotateRight90;
        private ToolStripMenuItem ToolStripMenuItem_RotateLeft90;
        private System.Windows.Forms.Timer _debounceTimer;
    }
}