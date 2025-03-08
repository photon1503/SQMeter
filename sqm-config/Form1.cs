using System;
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

            string response = await _sqmSerial.SendCommandAsync("rx");

            if (response.StartsWith("r,"))
            {
                string[] split = response.Split(',');
                lblMag.Text = split[1];
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
                lblSQMOffset.Text = split[1];
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await RefreshAsync();
        }
    }
}