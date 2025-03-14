using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using sqm_config.Properties;
using ScottPlot.WinForms;
using ScottPlot.Plottables;
using ScottPlot.Colormaps;
using ScottPlot;
using static ScottPlot.Generate;

namespace sqm_config
{
    public partial class Form1 : Form
    {
        private SQMSerial _sqmSerial;
        private bool pauseReading = false;
        private System.Windows.Forms.Timer timer;
        private bool isConnected = false;

        private DataLogger streamer;

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
            Buttons(false);

            cmbCOMports.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            string savedPort = Properties.Settings.Default.SelectedCOMport;
            if (!string.IsNullOrEmpty(savedPort))
            {
                cmbCOMports.SelectedItem = savedPort;
            }

            chkAutoRefreshStartup.Checked = Properties.Settings.Default.AutoRefresh;
            chkConnectStartup.Checked = Properties.Settings.Default.AutoConnect;

            if (Properties.Settings.Default.AutoConnect)
            {
                connect();
            }

            //delay 5 sec
            System.Threading.Thread.Sleep(5000);
            if (Properties.Settings.Default.AutoRefresh)
            {
                chkRefresh.Checked = true;
            }
        }

        private void InitializeChart()
        {
            streamer = formsPlot2.Plot.Add.DataLogger();

            formsPlot2.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();
            formsPlot2.Plot.Axes.DateTimeTicksBottom();

            formsPlot2.Refresh();
        }

        private async void connect()
        {
            if (isConnected)
            {
                _sqmSerial.Close();
                button1.Text = "Connect";
                isConnected = false;
                Buttons(false);
                return;
            }
            // get the selected COM port
            string selectedPort = cmbCOMports.SelectedItem.ToString();
            // open the selected COM port

            _sqmSerial = new SQMSerial(selectedPort);
            _sqmSerial.DataReceived += UpdateUI;
            // send the command to the SQM

            int errorCount = 0;
            bool success = false;

            while (!success && errorCount < 3)
            {
                txtLog.AppendText($"Sending: ix\r\n");
                string response = await _sqmSerial.SendCommandAsync("ix");

                string[] split = response.Split(',');

                if (response.StartsWith("i,"))
                {
                    lblProtocol.Text = split[1];
                    lblModel.Text = split[2];
                    lblFeature.Text = split[3];
                    lblSerial.Text = split[4];
                    success = true;
                }
                else
                {
                    errorCount++;
                    // MessageBox.Show("Failed to connect to the SQM");
                }
            }

            if (success)
            {
                isConnected = true;
                button1.Text = "Disconnect";
                Properties.Settings.Default.SelectedCOMport = selectedPort;
                Properties.Settings.Default.Save();

                // enable all buttons
                Buttons(true);

                // get data
                await RefreshAsync();
                //get config
                ReadConfig();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void Buttons(bool val)
        {
            btnReadConfig.Enabled = val;
            btnSQMCal.Enabled = val;
            btnReset.Enabled = val;
            btnTempOffsetWrite.Enabled = val;
            chkTempOffset.Enabled = val;
            chkRefresh.Enabled = val;
            button2.Enabled = val;
            button3.Enabled = val;
        }

        private async Task RefreshSQMLUAsync()
        {
            if (pauseReading)
            {
                return;
            }
            /*
             * #### Read data
            * Request: rx
            * Response: r, 10.28m,0000000000Hz,0000000002c,000005.000s, 026.2C
            * */
            string response = await _sqmSerial.SendCommandAsync("rx");
            // response
            // r, 10.28m,0000000000Hz,0000000002c,000005.000s, 026.2C
            if (response.StartsWith("r,"))
            {
                string[] split = response.Split(',');

                //remove trailing m from split[1]
                split[1] = split[1].Substring(0, split[1].Length - 1);
                lblMag.Text = split[1];
            }
        }

        private async Task RefreshAsync()
        {
            if (pauseReading)
            {
                return;
            }
            /*
             * #### Read data
            * Request: rx
            * Response: r, 10.28m,0000000000Hz,0000000002c,000005.000s, 026.2C
            * */

            string response = await _sqmSerial.SendCommandAsync("ax");

            // response
            // a,,full:12193,ir:2422,vis:9771,mag:13.805,dmpsas:0.01,integration:600.00,gain:9876.00,niter:1,lux:36.653781,temp:21.03,hum:63.12,pres:1326.49

            if (response.StartsWith("a,"))
            {
                lblFull.Text = GetSplitValue(response, "full");

                lblIR.Text = GetSplitValue(response, "ir");

                lblVIS.Text = GetSplitValue(response, "vis");

                lblDMPSAS.Text = GetSplitValue(response, "dmpsas");

                lblExp.Text = GetSplitValue(response, "integration") + " ms";

                lblGain.Text = GetSplitValue(response, "gain") + " x";

                lblNiter.Text = GetSplitValue(response, "niter");

                lblLux.Text = GetSplitValue(response, "lux");

                lblTemp.Text = GetSplitValue(response, "temp") + " �C";

                lblHum.Text = GetSplitValue(response, "hum") + " %";

                lblPress.Text = GetSplitValue(response, "pres");

                lblMag.Text = GetSplitValue(response, "mag");

                System.DateTime currentTime = System.DateTime.Now;

                double magValues = double.Parse(lblMag.Text, CultureInfo.InvariantCulture);
                streamer.Add(new Coordinates(currentTime.ToOADate(), magValues));

                if (streamer.Data.Coordinates.Count() > 18000)
                    streamer.Data.Coordinates.RemoveAt(0);
                formsPlot2.Refresh();
            }
        }

        private string GetSplitValue(string response, string key)
        {
            string[] split = response.Split(',');
            foreach (string s in split)
            {
                if (s.Contains(key))
                {
                    string[] split2 = s.Split(':');
                    return split2[1];
                }
            }
            return "";
        }

        private void UpdateUI(string data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUI(data)));
                return;
            }
            // add local time to log
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"{time} Received: {data}\r\n");
        }

        private async void ReadConfig()
        {
            txtLog.AppendText($"Sending: gx\r\n");
            string response = await _sqmSerial.SendCommandAsync("gx");

            if (response.StartsWith("g,"))
            {
                string[] split = response.Split(',');

                //remove trailing m from split[1]
                split[1] = split[1].Substring(0, split[1].Length - 1);

                txtSQMcal.Text = split[1];
                txtTempOffset.Text = split[2];
                //remove trailing \r\n from split[3]

                chkTempOffset.Checked = split[3] == "Y";
                split[4] = split[4].Substring(0, split[4].Length - 2);
                txtDF.Text = split[4];
            }
        }

        private async void btnReadConfig_Click(object sender, EventArgs e)
        {
            ReadConfig();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await RefreshAsync();
        }

        private async void btnSQMCal_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtSQMcal.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double calValue))
            {
                MessageBox.Show("Invalid calibration value");
                return;
            }

            if (calValue < -25 || calValue > 25)
            {
                MessageBox.Show("Calibration value must be between -25 and 25");
                return;
            }

            // Format the command string based on the value of calValue
            string command = calValue < 0
                ? $"zcal1{calValue.ToString("0.00", CultureInfo.InvariantCulture)}x"
                : $"zcal1{calValue.ToString("00.00", CultureInfo.InvariantCulture)}x";
            txtLog.AppendText($"Sending: {command}\r\n");
            string response = await _sqmSerial.SendCommandAsync(command);
        }

        private void chkRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRefresh.Checked)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 5000;
                timer.Tick += async (s, e) => await RefreshAsync();
                timer.Start();
            }
            else
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            //warning message
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to reset the SQM calibration values to factory defaults?", "Reset SQM", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                txtLog.AppendText($"Sending: zcalDx\r\n");
                string response = await _sqmSerial.SendCommandAsync("zcalDx");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshSQMLUAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private async void btnTempOffsetWrite_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtTempOffset.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double calValue))
            {
                MessageBox.Show("Invalid calibration value");
                return;
            }

            if (calValue < -50 || calValue > 50)
            {
                MessageBox.Show("Calibration value must be between -50 and 50");
                return;
            }

            // Format the command string based on the value of calValue
            string command = calValue < 0
                ? $"zcal2{calValue.ToString("0.00", CultureInfo.InvariantCulture)}x"
                : $"zcal2{calValue.ToString("00.00", CultureInfo.InvariantCulture)}x";
            txtLog.AppendText($"Sending: {command}\r\n");
            string response = await _sqmSerial.SendCommandAsync(command);
        }

        private async void chkTempOffset_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTempOffset.Checked)
            {
                string response = await _sqmSerial.SendCommandAsync("zcalex");
            }
            else
            {
                string response = await _sqmSerial.SendCommandAsync("zcaldx");
            }
        }

        private async void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                string response = await _sqmSerial.SendCommandAsync("vx");
            }
            else
            {
                string response = await _sqmSerial.SendCommandAsync("yx");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            // DF
            if (!double.TryParse(txtDF.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double calValue))
            {
                MessageBox.Show("Invalid calibration value");
                return;
            }

            if (calValue < 0 || calValue > 100000)
            {
                MessageBox.Show("DF value must be between 0 and 100000");
                return;
            }

            // Format the command string based on the value of calValue
            string command = calValue < 0
                ? $"zcal3{calValue.ToString("0.00", CultureInfo.InvariantCulture)}x"
                : $"zcal3{calValue.ToString("00.00", CultureInfo.InvariantCulture)}x";
            txtLog.AppendText($"Sending: {command}\r\n");
            string response = await _sqmSerial.SendCommandAsync(command);
        }

        private async void button5_Click_1(object sender, EventArgs e)
        {
            string response = await _sqmSerial.SendCommandAsync("qx");
        }

        private void chkConnectStartup_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoConnect = chkConnectStartup.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkAutoRefreshStartup_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoRefresh = chkAutoRefreshStartup.Checked;
            Properties.Settings.Default.Save();
        }
    }
}