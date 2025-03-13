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
            lblAdvanced = new Label();
            lblAdvancedLabel = new Label();
            label6 = new Label();
            lblMag = new Label();
            groupBox2 = new GroupBox();
            chkTempOffset = new CheckBox();
            label9 = new Label();
            label8 = new Label();
            txtTempOffset = new TextBox();
            btnTempOffsetWrite = new Button();
            btnReset = new Button();
            label7 = new Label();
            label5 = new Label();
            txtSQMcal = new TextBox();
            btnSQMCal = new Button();
            btnReadConfig = new Button();
            txtLog = new TextBox();
            button2 = new Button();
            chkRefresh = new CheckBox();
            button3 = new Button();
            Version.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // cmbCOMports
            // 
            cmbCOMports.FormattingEnabled = true;
            cmbCOMports.Location = new Point(63, 13);
            cmbCOMports.Name = "cmbCOMports";
            cmbCOMports.Size = new Size(121, 23);
            cmbCOMports.TabIndex = 0;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(28, 16);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(29, 15);
            lblPort.TabIndex = 1;
            lblPort.Text = "Port";
            // 
            // button1
            // 
            button1.Location = new Point(190, 12);
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
            Version.Location = new Point(29, 325);
            Version.Name = "Version";
            Version.Size = new Size(171, 162);
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
            groupBox1.Controls.Add(lblAdvanced);
            groupBox1.Controls.Add(lblAdvancedLabel);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(lblMag);
            groupBox1.Location = new Point(29, 77);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(493, 242);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Measurement";
            // 
            // lblAdvanced
            // 
            lblAdvanced.AutoSize = true;
            lblAdvanced.Location = new Point(357, 59);
            lblAdvanced.Name = "lblAdvanced";
            lblAdvanced.Size = new Size(12, 15);
            lblAdvanced.TabIndex = 3;
            lblAdvanced.Text = "-";
            // 
            // lblAdvancedLabel
            // 
            lblAdvancedLabel.AutoSize = true;
            lblAdvancedLabel.Location = new Point(291, 59);
            lblAdvancedLabel.Name = "lblAdvancedLabel";
            lblAdvancedLabel.Size = new Size(60, 165);
            lblAdvancedLabel.TabIndex = 2;
            lblAdvancedLabel.Text = "💡Full\r\n🔴 IR\r\n👁 Visible\r\n𝚫 Dmpsas\r\n🕑 Exp\r\n✖ Gain\r\n⮔ Niter\r\n🔆 Lux\r\n🌡Temp\r\n🌢 Hum\r\n㍱ Press";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(34, 84);
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
            groupBox2.Controls.Add(chkTempOffset);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(txtTempOffset);
            groupBox2.Controls.Add(btnTempOffsetWrite);
            groupBox2.Controls.Add(btnReset);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(txtSQMcal);
            groupBox2.Controls.Add(btnSQMCal);
            groupBox2.Controls.Add(btnReadConfig);
            groupBox2.Location = new Point(206, 325);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(316, 162);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Config";
            // 
            // chkTempOffset
            // 
            chkTempOffset.AutoSize = true;
            chkTempOffset.Location = new Point(17, 117);
            chkTempOffset.Name = "chkTempOffset";
            chkTempOffset.Size = new Size(237, 19);
            chkTempOffset.TabIndex = 14;
            chkTempOffset.Text = "Enable SQM temperature compensation";
            chkTempOffset.UseVisualStyleBackColor = true;
            chkTempOffset.CheckedChanged += chkTempOffset_CheckedChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 7F);
            label9.Location = new Point(13, 102);
            label9.Name = "label9";
            label9.Size = new Size(171, 12);
            label9.TabIndex = 13;
            label9.Text = "Valid calibration values are 50.0 to 50.0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(13, 79);
            label8.Name = "label8";
            label8.Size = new Size(72, 15);
            label8.TabIndex = 12;
            label8.Text = "Temp Offset";
            // 
            // txtTempOffset
            // 
            txtTempOffset.Location = new Point(87, 76);
            txtTempOffset.Name = "txtTempOffset";
            txtTempOffset.Size = new Size(69, 23);
            txtTempOffset.TabIndex = 11;
            // 
            // btnTempOffsetWrite
            // 
            btnTempOffsetWrite.Location = new Point(249, 75);
            btnTempOffsetWrite.Name = "btnTempOffsetWrite";
            btnTempOffsetWrite.Size = new Size(61, 23);
            btnTempOffsetWrite.TabIndex = 10;
            btnTempOffsetWrite.Text = "Write";
            btnTempOffsetWrite.UseVisualStyleBackColor = true;
            btnTempOffsetWrite.Click += btnTempOffsetWrite_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(235, 132);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 8;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 7F);
            label7.Location = new Point(13, 57);
            label7.Name = "label7";
            label7.Size = new Size(175, 12);
            label7.TabIndex = 7;
            label7.Text = "Valid calibration values are -25.0 to 25.0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 34);
            label5.Name = "label5";
            label5.Size = new Size(68, 15);
            label5.TabIndex = 6;
            label5.Text = "SQM Offset";
            // 
            // txtSQMcal
            // 
            txtSQMcal.Location = new Point(87, 31);
            txtSQMcal.Name = "txtSQMcal";
            txtSQMcal.Size = new Size(69, 23);
            txtSQMcal.TabIndex = 5;
            // 
            // btnSQMCal
            // 
            btnSQMCal.Location = new Point(249, 30);
            btnSQMCal.Name = "btnSQMCal";
            btnSQMCal.Size = new Size(61, 23);
            btnSQMCal.TabIndex = 4;
            btnSQMCal.Text = "Write";
            btnSQMCal.UseVisualStyleBackColor = true;
            btnSQMCal.Click += btnSQMCal_Click;
            // 
            // btnReadConfig
            // 
            btnReadConfig.Location = new Point(13, 133);
            btnReadConfig.Name = "btnReadConfig";
            btnReadConfig.Size = new Size(72, 23);
            btnReadConfig.TabIndex = 3;
            btnReadConfig.Text = "Read";
            btnReadConfig.UseVisualStyleBackColor = true;
            btnReadConfig.Click += btnReadConfig_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(29, 493);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(493, 154);
            txtLog.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new Point(28, 48);
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
            chkRefresh.Location = new Point(109, 51);
            chkRefresh.Name = "chkRefresh";
            chkRefresh.Size = new Size(116, 19);
            chkRefresh.TabIndex = 8;
            chkRefresh.Text = "Auto Refresh (5s)";
            chkRefresh.UseVisualStyleBackColor = true;
            chkRefresh.CheckedChanged += chkRefresh_CheckedChanged;
            // 
            // button3
            // 
            button3.Location = new Point(358, 51);
            button3.Name = "button3";
            button3.Size = new Size(164, 23);
            button3.TabIndex = 9;
            button3.Text = "SQM-LU Protocol Test";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(551, 659);
            Controls.Add(button3);
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
            Text = "SQMeter";
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
        private Label lblAdvancedLabel;
        private Label lblAdvanced;
        private Button button3;
        private Label label8;
        private TextBox txtTempOffset;
        private Button btnTempOffsetWrite;
        private CheckBox chkTempOffset;
        private Label label9;
    }
}
