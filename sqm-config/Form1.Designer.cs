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
            lblLux = new Label();
            lblPress = new Label();
            lblHum = new Label();
            lblTemp = new Label();
            lblNiter = new Label();
            lblGain = new Label();
            lblExp = new Label();
            lblDMPSAS = new Label();
            lblVIS = new Label();
            lblIR = new Label();
            label21 = new Label();
            label20 = new Label();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            lblFull = new Label();
            label10 = new Label();
            label6 = new Label();
            lblMag = new Label();
            groupBox2 = new GroupBox();
            button5 = new Button();
            label11 = new Label();
            txtDF = new TextBox();
            button4 = new Button();
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
            checkBox1 = new CheckBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
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
            Version.Location = new Point(32, 282);
            Version.Name = "Version";
            Version.Size = new Size(171, 221);
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
            groupBox1.Controls.Add(lblLux);
            groupBox1.Controls.Add(lblPress);
            groupBox1.Controls.Add(lblHum);
            groupBox1.Controls.Add(lblTemp);
            groupBox1.Controls.Add(lblNiter);
            groupBox1.Controls.Add(lblGain);
            groupBox1.Controls.Add(lblExp);
            groupBox1.Controls.Add(lblDMPSAS);
            groupBox1.Controls.Add(lblVIS);
            groupBox1.Controls.Add(lblIR);
            groupBox1.Controls.Add(label21);
            groupBox1.Controls.Add(label20);
            groupBox1.Controls.Add(label19);
            groupBox1.Controls.Add(label18);
            groupBox1.Controls.Add(label17);
            groupBox1.Controls.Add(label16);
            groupBox1.Controls.Add(label15);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(lblFull);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(lblMag);
            groupBox1.Location = new Point(29, 77);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(493, 199);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Measurement";
            // 
            // lblLux
            // 
            lblLux.AutoSize = true;
            lblLux.Font = new Font("Segoe UI", 24F);
            lblLux.Location = new Point(315, 35);
            lblLux.Name = "lblLux";
            lblLux.Size = new Size(33, 45);
            lblLux.TabIndex = 25;
            lblLux.Text = "-";
            // 
            // lblPress
            // 
            lblPress.AutoSize = true;
            lblPress.Location = new Point(415, 156);
            lblPress.Name = "lblPress";
            lblPress.Size = new Size(12, 15);
            lblPress.TabIndex = 24;
            lblPress.Text = "-";
            // 
            // lblHum
            // 
            lblHum.AutoSize = true;
            lblHum.Location = new Point(415, 141);
            lblHum.Name = "lblHum";
            lblHum.Size = new Size(12, 15);
            lblHum.TabIndex = 23;
            lblHum.Text = "-";
            // 
            // lblTemp
            // 
            lblTemp.AutoSize = true;
            lblTemp.Location = new Point(415, 126);
            lblTemp.Name = "lblTemp";
            lblTemp.Size = new Size(12, 15);
            lblTemp.TabIndex = 22;
            lblTemp.Text = "-";
            // 
            // lblNiter
            // 
            lblNiter.AutoSize = true;
            lblNiter.Location = new Point(249, 156);
            lblNiter.Name = "lblNiter";
            lblNiter.Size = new Size(12, 15);
            lblNiter.TabIndex = 21;
            lblNiter.Text = "-";
            // 
            // lblGain
            // 
            lblGain.AutoSize = true;
            lblGain.Location = new Point(249, 141);
            lblGain.Name = "lblGain";
            lblGain.Size = new Size(12, 15);
            lblGain.TabIndex = 20;
            lblGain.Text = "-";
            // 
            // lblExp
            // 
            lblExp.AutoSize = true;
            lblExp.Location = new Point(249, 126);
            lblExp.Name = "lblExp";
            lblExp.Size = new Size(12, 15);
            lblExp.TabIndex = 19;
            lblExp.Text = "-";
            // 
            // lblDMPSAS
            // 
            lblDMPSAS.AutoSize = true;
            lblDMPSAS.Location = new Point(80, 171);
            lblDMPSAS.Name = "lblDMPSAS";
            lblDMPSAS.Size = new Size(12, 15);
            lblDMPSAS.TabIndex = 18;
            lblDMPSAS.Text = "-";
            // 
            // lblVIS
            // 
            lblVIS.AutoSize = true;
            lblVIS.Location = new Point(80, 156);
            lblVIS.Name = "lblVIS";
            lblVIS.Size = new Size(12, 15);
            lblVIS.TabIndex = 17;
            lblVIS.Text = "-";
            // 
            // lblIR
            // 
            lblIR.AutoSize = true;
            lblIR.Location = new Point(80, 141);
            lblIR.Name = "lblIR";
            lblIR.Size = new Size(12, 15);
            lblIR.TabIndex = 16;
            lblIR.Text = "-";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(360, 141);
            label21.Name = "label21";
            label21.Size = new Size(49, 15);
            label21.TabIndex = 15;
            label21.Text = "🌢 Hum";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(360, 156);
            label20.Name = "label20";
            label20.Size = new Size(49, 15);
            label20.TabIndex = 14;
            label20.Text = "㍱ Press";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(360, 126);
            label19.Name = "label19";
            label19.Size = new Size(49, 15);
            label19.TabIndex = 13;
            label19.Text = "🌡Temp";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(369, 84);
            label18.Name = "label18";
            label18.Size = new Size(40, 15);
            label18.TabIndex = 12;
            label18.Text = "🔆 Lux";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(192, 156);
            label17.Name = "label17";
            label17.Size = new Size(45, 15);
            label17.TabIndex = 11;
            label17.Text = "⮔ Niter";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(192, 141);
            label16.Name = "label16";
            label16.Size = new Size(46, 15);
            label16.TabIndex = 10;
            label16.Text = "✖ Gain";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(192, 126);
            label15.Name = "label15";
            label15.Size = new Size(40, 15);
            label15.TabIndex = 9;
            label15.Text = "🕑 Exp";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(15, 171);
            label14.Name = "label14";
            label14.Size = new Size(64, 15);
            label14.TabIndex = 8;
            label14.Text = "𝚫 DMPSAS";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(15, 156);
            label13.Name = "label13";
            label13.Size = new Size(56, 15);
            label13.TabIndex = 7;
            label13.Text = "👁 Visible";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(15, 141);
            label12.Name = "label12";
            label12.Size = new Size(32, 15);
            label12.TabIndex = 6;
            label12.Text = "🔴 IR";
            // 
            // lblFull
            // 
            lblFull.AutoSize = true;
            lblFull.Location = new Point(80, 126);
            lblFull.Name = "lblFull";
            lblFull.Size = new Size(12, 15);
            lblFull.TabIndex = 5;
            lblFull.Text = "-";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(15, 126);
            label10.Name = "label10";
            label10.Size = new Size(38, 15);
            label10.TabIndex = 4;
            label10.Text = "💡Full";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(51, 84);
            label6.Name = "label6";
            label6.Size = new Size(78, 15);
            label6.TabIndex = 1;
            label6.Text = "mags/arcsec²";
            label6.Click += label6_Click;
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
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(txtDF);
            groupBox2.Controls.Add(button4);
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
            groupBox2.Location = new Point(206, 282);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(316, 221);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Config";
            // 
            // button5
            // 
            button5.Location = new Point(128, 192);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 18;
            button5.Text = "Reboot";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click_1;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(15, 116);
            label11.Name = "label11";
            label11.Size = new Size(78, 15);
            label11.TabIndex = 17;
            label11.Text = "Device Factor";
            // 
            // txtDF
            // 
            txtDF.Location = new Point(95, 113);
            txtDF.Name = "txtDF";
            txtDF.Size = new Size(69, 23);
            txtDF.TabIndex = 16;
            // 
            // button4
            // 
            button4.Location = new Point(251, 112);
            button4.Name = "button4";
            button4.Size = new Size(61, 23);
            button4.TabIndex = 15;
            button4.Text = "Write";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // chkTempOffset
            // 
            chkTempOffset.AutoSize = true;
            chkTempOffset.Location = new Point(15, 167);
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
            label9.Location = new Point(15, 87);
            label9.Name = "label9";
            label9.Size = new Size(171, 12);
            label9.TabIndex = 13;
            label9.Text = "Valid calibration values are 50.0 to 50.0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 64);
            label8.Name = "label8";
            label8.Size = new Size(72, 15);
            label8.TabIndex = 12;
            label8.Text = "Temp Offset";
            // 
            // txtTempOffset
            // 
            txtTempOffset.Location = new Point(95, 61);
            txtTempOffset.Name = "txtTempOffset";
            txtTempOffset.Size = new Size(69, 23);
            txtTempOffset.TabIndex = 11;
            // 
            // btnTempOffsetWrite
            // 
            btnTempOffsetWrite.Location = new Point(251, 60);
            btnTempOffsetWrite.Name = "btnTempOffsetWrite";
            btnTempOffsetWrite.Size = new Size(61, 23);
            btnTempOffsetWrite.TabIndex = 10;
            btnTempOffsetWrite.Text = "Write";
            btnTempOffsetWrite.UseVisualStyleBackColor = true;
            btnTempOffsetWrite.Click += btnTempOffsetWrite_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(235, 192);
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
            label7.Location = new Point(15, 46);
            label7.Name = "label7";
            label7.Size = new Size(175, 12);
            label7.TabIndex = 7;
            label7.Text = "Valid calibration values are -25.0 to 25.0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 25);
            label5.Name = "label5";
            label5.Size = new Size(68, 15);
            label5.TabIndex = 6;
            label5.Text = "SQM Offset";
            // 
            // txtSQMcal
            // 
            txtSQMcal.Location = new Point(95, 22);
            txtSQMcal.Name = "txtSQMcal";
            txtSQMcal.Size = new Size(69, 23);
            txtSQMcal.TabIndex = 5;
            // 
            // btnSQMCal
            // 
            btnSQMCal.Location = new Point(249, 21);
            btnSQMCal.Name = "btnSQMCal";
            btnSQMCal.Size = new Size(61, 23);
            btnSQMCal.TabIndex = 4;
            btnSQMCal.Text = "Write";
            btnSQMCal.UseVisualStyleBackColor = true;
            btnSQMCal.Click += btnSQMCal_Click;
            // 
            // btnReadConfig
            // 
            btnReadConfig.Location = new Point(11, 192);
            btnReadConfig.Name = "btnReadConfig";
            btnReadConfig.Size = new Size(72, 23);
            btnReadConfig.TabIndex = 3;
            btnReadConfig.Text = "Read";
            btnReadConfig.UseVisualStyleBackColor = true;
            btnReadConfig.Click += btnReadConfig_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(28, 509);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(493, 95);
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
            button3.Location = new Point(357, 610);
            button3.Name = "button3";
            button3.Size = new Size(164, 23);
            button3.TabIndex = 9;
            button3.Text = "SQM-LU Protocol Test";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(32, 607);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(188, 19);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "Verbose mode (for debugging)";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(551, 638);
            Controls.Add(checkBox1);
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
           // InitializeCOMports();
        }
        // initialize the ComboBox cmbCOMports with the COM ports
       

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
        private Button button3;
        private Label label8;
        private TextBox txtTempOffset;
        private Button btnTempOffsetWrite;
        private CheckBox chkTempOffset;
        private Label label9;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label lblFull;
        private Label label10;
        private Label lblLux;
        private Label lblPress;
        private Label lblHum;
        private Label lblTemp;
        private Label lblNiter;
        private Label lblGain;
        private Label lblExp;
        private Label lblDMPSAS;
        private Label lblVIS;
        private Label lblIR;
        private Label label21;
        private Label label20;
        private Label label19;
        private Label label18;
        private Label label17;
        private Label label16;
        private Label label15;
        private CheckBox checkBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private Label label11;
        private TextBox txtDF;
        private Button button4;
        private Button button5;
    }
}
