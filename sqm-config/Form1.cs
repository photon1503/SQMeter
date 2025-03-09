using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;

namespace sqm_config
{
    public partial class Form1 : Form
    {
        private SQMSerial _sqmSerial;
        private bool pauseReading = false;
        private System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
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
            // a,262149,4,0.00,1,0.00,5,9876.00,21.57
            // luminosity, ir, adjustedIR,visible,adjustedVisible,full,gainscale,mag
            if (response.StartsWith("a,"))
            {
                string[] split = response.Split(',');
                lblAdvanced.Text = split[1];
                lblAdvanced.Text += Environment.NewLine;
                lblAdvanced.Text += split[2];
                lblAdvanced.Text += Environment.NewLine;
                lblAdvanced.Text += split[3];
                lblAdvanced.Text += Environment.NewLine;
                lblAdvanced.Text += split[4];
                lblAdvanced.Text += Environment.NewLine;
                lblAdvanced.Text += split[5];
                lblAdvanced.Text += Environment.NewLine;
                lblAdvanced.Text += split[6];
                lblAdvanced.Text += Environment.NewLine;
                lblAdvanced.Text += split[7].TrimStart();
                lblAdvanced.Text += Environment.NewLine;

                lblAdvanced.Text += split[8].TrimStart();
                lblMag.Text = split[9];
            }
        }

        private void UpdateUI(string data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUI(data)));
                return;
            }
            txtLog.AppendText($"Received: {data}\r\n");
        }

        private async void btnReadConfig_Click(object sender, EventArgs e)
        {
            txtLog.AppendText($"Sending: gx\r\n");
            string response = await _sqmSerial.SendCommandAsync("gx");

            if (response.StartsWith("g,"))
            {
                string[] split = response.Split(',');

                //remove trailing m from split[1]
                split[1] = split[1].Substring(0, split[1].Length - 1);

                txtSQMcal.Text = split[1];
            }
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
    }
}