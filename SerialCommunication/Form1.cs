using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        private object radioButtenVerbonden;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void SendDigitalCommand(int pinNumber, bool isHigh)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Geen actieve seriële verbinding. Maak eerst verbinding met Arduino.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string command = $"set d{pinNumber} {(isHigh ? "high" : "low")}";
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het verzenden van commando: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelStatus.Text = "Fout bij communicatie";
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            SendDigitalCommand(2, checkBoxDigital2.Checked);
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            SendDigitalCommand(3, checkBoxDigital3.Checked);
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            SendDigitalCommand(4, checkBoxDigital4.Checked);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (serialPortArduino.IsOpen)
            {
                // Verbinding verbreken
                serialPortArduino.Close();
                buttonConnect.Text = "Connect";
                radioButtonVerbonden.Checked = false;
                labelStatus.Text = "Verbinding verbroken";
                
                // Disable en reset checkboxen
                checkBoxDigital2.Enabled = false;
                checkBoxDigital3.Enabled = false;
                checkBoxDigital4.Enabled = false;
                checkBoxDigital2.Checked = false;
                checkBoxDigital3.Checked = false;
                checkBoxDigital4.Checked = false;
            }
            else
            {
                // Verbinding maken
                try
                {
                    serialPortArduino.PortName = comboBoxPoort.SelectedItem.ToString();
                    serialPortArduino.BaudRate = int.Parse(comboBoxBaudrate.SelectedItem.ToString());
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    // Pariteit instellen
                    if (radioButtonParityEven.Checked)
                        serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked)
                        serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityMark.Checked)
                        serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked)
                        serialPortArduino.Parity = Parity.Space;
                    else
                        serialPortArduino.Parity = Parity.None;

                    // Stop bits instellen
                    if (radioButtonStopbitsOne.Checked)
                        serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked)
                        serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked)
                        serialPortArduino.StopBits = StopBits.Two;
                    else
                        serialPortArduino.StopBits = StopBits.None;

                    // Handshake instellen
                    if (radioButtonHandshakeRTS.Checked)
                        serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeXonXoff.Checked)
                        serialPortArduino.Handshake = Handshake.XOnXOff;
                    else if (radioButtonHandshakeRTSXonXoff.Checked)
                        serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else
                        serialPortArduino.Handshake = Handshake.None;

                    // RTS en DTR instellen
                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtenVerbonden. = true;
                        buttonConnect.Text = "disconnect";
                        labelStatus.Text = "status: Connected";
                    }
                    else
                    {
                        serialPortArduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij het openen van de poort: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    labelStatus.Text = "Fout bij verbinding";
                }
            }
        }
    }
}
