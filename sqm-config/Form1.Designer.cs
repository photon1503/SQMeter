using System;

namespace sqm_config
{
    partial class Form1
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
            cmbCOMports = new ComboBox();
            lblPort = new Label();
            button1 = new Button();
            Version = new GroupBox();
            lblSerial = new Label();
            lblFeature = new Label();
            lblModel = new Label();
            lblProtocol = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox1 = new GroupBox();
            label6 = new Label();
            lblMag = new Label();
            groupBox2 = new GroupBox();
            label7 = new Label();
            label5 = new Label();
            txtSQMcal = new TextBox();
            btnSQMCal = new Button();
            btnReadConfig = new Button();
            txtLog = new TextBox();
            button2 = new Button();
            chkRefresh = new CheckBox();
            btnReset = new Button();
            Version.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // cmbCOMports
            // 
            cmbCOMports.FormattingEnabled = true;
            cmbCOMports.Location = new Point(97, 29);
            cmbCOMports.Name = "cmbCOMports";
            cmbCOMports.Size = new Size(121, 23);
            cmbCOMports.TabIndex = 0;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(29, 33);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(29, 15);
            lblPort.TabIndex = 1;
            lblPort.Text = "Port";
            // 
            // button1
            // 
            button1.Location = new Point(245, 28);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Version
            // 
            Version.Controls.Add(lblSerial);
            Version.Controls.Add(lblFeature);
            Version.Controls.Add(lblModel);
            Version.Controls.Add(lblProtocol);
            Version.Controls.Add(label4);
            Version.Controls.Add(label3);
            Version.Controls.Add(label2);
            Version.Controls.Add(label1);
            Version.Location = new Point(29, 189);
            Version.Name = "Version";
            Version.Size = new Size(291, 133);
            Version.TabIndex = 3;
            Version.TabStop = false;
            Version.Text = "Version";
            // 
            // lblSerial
            // 
            lblSerial.AutoSize = true;
            lblSerial.Location = new Point(67, 79);
            lblSerial.Name = "lblSerial";
            lblSerial.Size = new Size(12, 15);
            lblSerial.TabIndex = 7;
            lblSerial.Text = "-";
            // 
            // lblFeature
            // 
            lblFeature.AutoSize = true;
            lblFeature.Location = new Point(68, 64);
            lblFeature.Name = "lblFeature";
            lblFeature.Size = new Size(12, 15);
            lblFeature.TabIndex = 6;
            lblFeature.Text = "-";
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.Location = new Point(67, 49);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(12, 15);
            lblModel.TabIndex = 5;
            lblModel.Text = "-";
            // 
            // lblProtocol
            // 
            lblProtocol.AutoSize = true;
            lblProtocol.Location = new Point(67, 34);
            lblProtocol.Name = "lblProtocol";
            lblProtocol.Size = new Size(12, 15);
            lblProtocol.TabIndex = 4;
            lblProtocol.Text = "-";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 79);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 3;
            label4.Text = "Serial:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 64);
            label3.Name = "label3";
            label3.Size = new Size(49, 15);
            label3.TabIndex = 2;
            label3.Text = "Feature:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 49);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 1;
            label2.Text = "Model:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 34);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 0;
            label1.Text = "Protocol:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(lblMag);
            groupBox1.Location = new Point(35, 83);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(285, 100);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Measurement";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(201, 49);
            label6.Name = "label6";
            label6.Size = new Size(78, 15);
            label6.TabIndex = 1;
            label6.Text = "mags/arcsec²";
            // 
            // lblMag
            // 
            lblMag.AutoSize = true;
            lblMag.Font = new Font("Segoe UI", 36F);
            lblMag.Location = new Point(8, 19);
            lblMag.Name = "lblMag";
            lblMag.Size = new Size(47, 65);
            lblMag.TabIndex = 0;
            lblMag.Text = "-";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnReset);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(txtSQMcal);
            groupBox2.Controls.Add(btnSQMCal);
            groupBox2.Controls.Add(btnReadConfig);
            groupBox2.Location = new Point(326, 189);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(200, 133);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Config";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 7F);
            label7.Location = new Point(13, 87);
            label7.Name = "label7";
            label7.Size = new Size(175, 12);
            label7.TabIndex = 7;
            label7.Text = "Valid calibration values are -25.0 to 25.0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 64);
            label5.Name = "label5";
            label5.Size = new Size(68, 15);
            label5.TabIndex = 6;
            label5.Text = "SQM Offset";
            // 
            // txtSQMcal
            // 
            txtSQMcal.Location = new Point(87, 61);
            txtSQMcal.Name = "txtSQMcal";
            txtSQMcal.Size = new Size(69, 23);
            txtSQMcal.TabIndex = 5;
            // 
            // btnSQMCal
            // 
            btnSQMCal.Location = new Point(87, 24);
            btnSQMCal.Name = "btnSQMCal";
            btnSQMCal.Size = new Size(75, 23);
            btnSQMCal.TabIndex = 4;
            btnSQMCal.Text = "Write";
            btnSQMCal.UseVisualStyleBackColor = true;
            btnSQMCal.Click += btnSQMCal_Click;
            // 
            // btnReadConfig
            // 
            btnReadConfig.Location = new Point(6, 24);
            btnReadConfig.Name = "btnReadConfig";
            btnReadConfig.Size = new Size(75, 23);
            btnReadConfig.TabIndex = 3;
            btnReadConfig.Text = "Read";
            btnReadConfig.UseVisualStyleBackColor = true;
            btnReadConfig.Click += btnReadConfig_Click;
            // 
            // txtLog
            // 
            txtLog.Enabled = false;
            txtLog.Location = new Point(33, 340);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(493, 98);
            txtLog.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new Point(328, 92);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 7;
            button2.Text = "Refresh";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // chkRefresh
            // 
            chkRefresh.AutoSize = true;
            chkRefresh.Location = new Point(331, 120);
            chkRefresh.Name = "chkRefresh";
            chkRefresh.Size = new Size(116, 19);
            chkRefresh.TabIndex = 8;
            chkRefresh.Text = "Auto Refresh (5s)";
            chkRefresh.UseVisualStyleBackColor = true;
            chkRefresh.CheckedChanged += chkRefresh_CheckedChanged;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(16, 108);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 8;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(551, 450);
            Controls.Add(chkRefresh);
            Controls.Add(button2);
            Controls.Add(txtLog);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(Version);
            Controls.Add(button1);
            Controls.Add(lblPort);
            Controls.Add(cmbCOMports);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            Version.ResumeLayout(false);
            Version.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeCOMports();
        }
        // initialize the ComboBox cmbCOMports with the COM ports
        private void InitializeCOMports()
        {
            cmbCOMports.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        private ComboBox cmbCOMports;
        private Label lblPort;
        private Button button1;
        private GroupBox Version;
        private Label lblSerial;
        private Label lblFeature;
        private Label lblModel;
        private Label lblProtocol;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private GroupBox groupBox1;
        private Label label6;
        private Label lblMag;
        private GroupBox groupBox2;
        private Button btnReadConfig;
        private TextBox txtLog;
        private Button button2;
        private TextBox txtSQMcal;
        private Button btnSQMCal;
        private Label label7;
        private Label label5;
        private CheckBox chkRefresh;
        private Button btnReset;
    }
}
