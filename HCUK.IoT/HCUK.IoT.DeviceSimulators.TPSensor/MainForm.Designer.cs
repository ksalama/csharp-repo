namespace HCUK.IoT.DeviceSimulators.TPSensor
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.connectToEventHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvSensors = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpPre = new System.Windows.Forms.GroupBox();
            this.barPre = new System.Windows.Forms.TrackBar();
            this.grpFreq = new System.Windows.Forms.GroupBox();
            this.barFreq = new System.Windows.Forms.TrackBar();
            this.grpTemp = new System.Windows.Forms.GroupBox();
            this.barTemp = new System.Windows.Forms.TrackBar();
            this.picGrey = new System.Windows.Forms.PictureBox();
            this.picGreen = new System.Windows.Forms.PictureBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpPre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barPre)).BeginInit();
            this.grpFreq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barFreq)).BeginInit();
            this.grpTemp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGrey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToEventHubToolStripMenuItem,
            this.addSensorToolStripMenuItem,
            this.removeSensorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(987, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // connectToEventHubToolStripMenuItem
            // 
            this.connectToEventHubToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectToEventHubToolStripMenuItem.Name = "connectToEventHubToolStripMenuItem";
            this.connectToEventHubToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.connectToEventHubToolStripMenuItem.Text = "Connect to  Azure IoT Hub";
            this.connectToEventHubToolStripMenuItem.Click += new System.EventHandler(this.connectToEventHubToolStripMenuItem_Click);
            // 
            // addSensorToolStripMenuItem
            // 
            this.addSensorToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addSensorToolStripMenuItem.Name = "addSensorToolStripMenuItem";
            this.addSensorToolStripMenuItem.Size = new System.Drawing.Size(105, 24);
            this.addSensorToolStripMenuItem.Text = "Add Sensor";
            this.addSensorToolStripMenuItem.Click += new System.EventHandler(this.addSensorToolStripMenuItem_Click);
            // 
            // removeSensorToolStripMenuItem
            // 
            this.removeSensorToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeSensorToolStripMenuItem.Name = "removeSensorToolStripMenuItem";
            this.removeSensorToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.removeSensorToolStripMenuItem.Text = "Remove Sensor";
            this.removeSensorToolStripMenuItem.Click += new System.EventHandler(this.removeSensorToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 583);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(987, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(987, 555);
            this.splitContainer1.SplitterDistance = 472;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvSensors);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 583);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensors";
            // 
            // lvSensors
            // 
            this.lvSensors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvSensors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSensors.FullRowSelect = true;
            this.lvSensors.Location = new System.Drawing.Point(3, 22);
            this.lvSensors.MultiSelect = false;
            this.lvSensors.Name = "lvSensors";
            this.lvSensors.Size = new System.Drawing.Size(460, 558);
            this.lvSensors.TabIndex = 0;
            this.lvSensors.UseCompatibleStateImageBehavior = false;
            this.lvSensors.View = System.Windows.Forms.View.Details;
            this.lvSensors.SelectedIndexChanged += new System.EventHandler(this.lvSensors_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 116;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 119;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 101;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Status";
            this.columnHeader4.Width = 102;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grpPre);
            this.groupBox2.Controls.Add(this.grpFreq);
            this.groupBox2.Controls.Add(this.grpTemp);
            this.groupBox2.Controls.Add(this.picGrey);
            this.groupBox2.Controls.Add(this.picGreen);
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(511, 555);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensor Control Pannel";
            // 
            // grpPre
            // 
            this.grpPre.BackColor = System.Drawing.SystemColors.Control;
            this.grpPre.Controls.Add(this.barPre);
            this.grpPre.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grpPre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPre.Location = new System.Drawing.Point(392, 36);
            this.grpPre.Name = "grpPre";
            this.grpPre.Size = new System.Drawing.Size(116, 510);
            this.grpPre.TabIndex = 17;
            this.grpPre.TabStop = false;
            this.grpPre.Text = "Pressure";
            // 
            // barPre
            // 
            this.barPre.BackColor = System.Drawing.SystemColors.Control;
            this.barPre.Location = new System.Drawing.Point(23, 21);
            this.barPre.Maximum = 30;
            this.barPre.Name = "barPre";
            this.barPre.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barPre.Size = new System.Drawing.Size(45, 483);
            this.barPre.TabIndex = 2;
            this.barPre.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.barPre.Value = 10;
            this.barPre.Scroll += new System.EventHandler(this.barPre_Scroll_1);
            // 
            // grpFreq
            // 
            this.grpFreq.Controls.Add(this.barFreq);
            this.grpFreq.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grpFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFreq.Location = new System.Drawing.Point(130, 312);
            this.grpFreq.Name = "grpFreq";
            this.grpFreq.Size = new System.Drawing.Size(256, 67);
            this.grpFreq.TabIndex = 16;
            this.grpFreq.TabStop = false;
            this.grpFreq.Text = "Emission Frequency";
            // 
            // barFreq
            // 
            this.barFreq.Location = new System.Drawing.Point(6, 21);
            this.barFreq.Maximum = 50;
            this.barFreq.Minimum = 1;
            this.barFreq.Name = "barFreq";
            this.barFreq.Size = new System.Drawing.Size(244, 45);
            this.barFreq.TabIndex = 4;
            this.barFreq.TickFrequency = 5;
            this.barFreq.Value = 10;
            this.barFreq.Scroll += new System.EventHandler(this.barFreq_Scroll);
            // 
            // grpTemp
            // 
            this.grpTemp.BackColor = System.Drawing.SystemColors.Control;
            this.grpTemp.Controls.Add(this.barTemp);
            this.grpTemp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grpTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTemp.Location = new System.Drawing.Point(6, 36);
            this.grpTemp.Name = "grpTemp";
            this.grpTemp.Size = new System.Drawing.Size(116, 510);
            this.grpTemp.TabIndex = 15;
            this.grpTemp.TabStop = false;
            this.grpTemp.Text = "Temprature";
            // 
            // barTemp
            // 
            this.barTemp.BackColor = System.Drawing.SystemColors.Control;
            this.barTemp.Location = new System.Drawing.Point(23, 21);
            this.barTemp.Maximum = 30;
            this.barTemp.Name = "barTemp";
            this.barTemp.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barTemp.Size = new System.Drawing.Size(45, 483);
            this.barTemp.TabIndex = 2;
            this.barTemp.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.barTemp.Value = 10;
            this.barTemp.Scroll += new System.EventHandler(this.barTemp_Scroll);
            // 
            // picGrey
            // 
            this.picGrey.Image = global::HCUK.IoT.DeviceSimulators.TPSensor.Properties.Resources.generic_2400px;
            this.picGrey.Location = new System.Drawing.Point(129, 45);
            this.picGrey.Name = "picGrey";
            this.picGrey.Size = new System.Drawing.Size(255, 240);
            this.picGrey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGrey.TabIndex = 14;
            this.picGrey.TabStop = false;
            // 
            // picGreen
            // 
            this.picGreen.Image = global::HCUK.IoT.DeviceSimulators.TPSensor.Properties.Resources.led_circle_green_2400px;
            this.picGreen.Location = new System.Drawing.Point(129, 45);
            this.picGreen.Name = "picGreen";
            this.picGreen.Size = new System.Drawing.Size(255, 240);
            this.picGreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGreen.TabIndex = 13;
            this.picGreen.TabStop = false;
            this.picGreen.Visible = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.SystemColors.Control;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnStop.Location = new System.Drawing.Point(130, 486);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(256, 60);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnStart.Location = new System.Drawing.Point(130, 420);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(256, 60);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 605);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Machine Simulator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.grpPre.ResumeLayout(false);
            this.grpPre.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barPre)).EndInit();
            this.grpFreq.ResumeLayout(false);
            this.grpFreq.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barFreq)).EndInit();
            this.grpTemp.ResumeLayout(false);
            this.grpTemp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGrey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem connectToEventHubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSensorToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripMenuItem removeSensorToolStripMenuItem;
        private System.Windows.Forms.TrackBar barTemp;
        private System.Windows.Forms.TrackBar barFreq;
        private System.Windows.Forms.PictureBox picGreen;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox picGrey;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox grpPre;
        private System.Windows.Forms.TrackBar barPre;
        private System.Windows.Forms.GroupBox grpFreq;
        private System.Windows.Forms.GroupBox grpTemp;
        private System.Windows.Forms.ListView lvSensors;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

