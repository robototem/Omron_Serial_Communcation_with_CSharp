using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmronSerialCommWithCSharp
{
    public partial class OmronSerialComm : Form
    {
        Thread serialThread;

        public string TX;
        public string RX;
        public string FCS;
        public string RXD;

        public OmronSerialComm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; // Prevents crashing when UI is updated within another thread
        }

        private void OmronSerialComm_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Read and Write section
        /// </summary>
        private void Communicate(string command)
        {
            string BufferTX;
            string fcs_rxd;

            try
            {
                RXD = "";
                BufferTX = command + FCS + "*" + (char)13;
                textBoxSend.Text = BufferTX;
                // Send the information out the serial port
                serialPort1.WriteTimeout = 1000;
                serialPort1.ReadTimeout = 1000;
                serialPort1.Write(BufferTX);
                System.Threading.Thread.Sleep(50);
                RXD = serialPort1.ReadTo("\r");
                

                fcs_rxd = RXD.Substring(RXD.Length - 3, 2);
                if (RXD.Substring(0, 1) == "@")
                {
                    command = RXD.Substring(0, RXD.Length - 3);
                }
                else if (RXD.Substring(2, 1) == "@")
                {
                    command = RXD.Substring(2, RXD.Length - 5);
                    RXD = RXD.Substring(2, RXD.Length - 1);
                }
                GetFCS(command);
                if (FCS != fcs_rxd)
                {
                    RXD = "Communcation Error";
                }
            }
            catch (Exception ex)
            {
                richTextBoxReceived.Text = ex.Message;
            }
        }

        /// <summary>
        /// Get the FCS value based on command
        /// </summary>
        private void GetFCS(string command)
        {
            char c;
            decimal B;
            int L;
            int A;
            string TJ;
            L = command.Length;
            A = 0;
            for (int i = 0; i < command.Length; i++)
            {
                TJ = command.Substring(i, 1);
                A = Decimal.ToInt32(char.Parse(TJ)) ^ A;
            }
            FCS = A.ToString("X");
            if (FCS.Length == 1)
            {
                FCS = "0" + FCS;
            }
        }

        /// <summary>
        /// Starts communcation thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
          
                serialThread = new Thread(StartCommunacition);
                serialThread.Start();
            
        }

        /// <summary>
        /// Establish communcation
        /// </summary>
        private void StartCommunacition()
        {
            while (true)
            {
                Thread.Sleep(50);
                // Display current date and time
                labelTime.Text = System.DateTime.Now.ToString();
                // Open the serial port
                if (serialPort1.IsOpen == false)
                {
                    try
                    {
                        ComboBoxChanged();
                        serialPort1.Open();
                    }
                    catch (Exception ex)
                    {
                        richTextBoxReceived.Text = ex.Message;
                    }
                }

                int charreturn;
                // Check DM AREA
                TX = textBoxTX.Text;
                textBoxCommand.Text = TX;
                GetFCS(TX);
                textBoxFCS.Text = FCS;
                Communicate(TX);
                if (RXD.Length > 2)
                {
                    richTextBoxReceived.Text = RXD;

                    if (RXD.Substring(5, 2) == "00")
                    {
                        textBox1.Text = RXD.Substring(7, 4);
                        textBox2.Text = RXD.Substring(11, 4);
                        textBox3.Text = RXD.Substring(15, 4);
                        textBox4.Text = RXD.Substring(19, 4);
                        textBox5.Text = RXD.Substring(23, 4);
                        textBox6.Text = RXD.Substring(27, 4);
                        textBox7.Text = RXD.Substring(31, 4);
                        textBox8.Text = RXD.Substring(35, 4);
                        textBox9.Text = RXD.Substring(39, 4);
                        textBox10.Text = RXD.Substring(49, 4);
                    }
                }

                ///
                /// If you would like to activate Write command:
                ///

                //Thread.Sleep(50);
                //RX = textBoxRX.Text;
                //textBoxCommand.Text = RX;
                //GetFCS(RX);
                //textBoxFCS.Text = FCS;
                //Communicate(RX);
                //richTextBoxReceived.Text = RXD;

            }

        }

        /// <summary>
        /// Stops the thread if any
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (serialThread.IsAlive)
            {
                serialThread.Abort();
                richTextBoxReceived.Text = "Thread was being aborted.";
            }
        }

        /// <summary>
        /// Updates port properties
        /// </summary>
        private void ComboBoxChanged()
        {

            serialPort1.Close();

            RXD = "";

            serialPort1.PortName = comboBoxPortName.Text;
            serialPort1.BaudRate = int.Parse(comboBoxBaudRate.Text);
            serialPort1.DataBits = int.Parse(comboBoxDataBits.Text);

            if (comboBoxParity.Text == "Odd")
            {
                serialPort1.Parity = System.IO.Ports.Parity.Odd;
            }
            else if (comboBoxParity.Text == "Even")
            {
                serialPort1.Parity = System.IO.Ports.Parity.Even;
            }
            else if (comboBoxParity.Text == "None")
            {
                serialPort1.Parity = System.IO.Ports.Parity.None;
            }

            if (comboBoxStopBits.Text == "1")
            {
                serialPort1.StopBits = System.IO.Ports.StopBits.One;
            }
            else if (comboBoxStopBits.Text == "2")
            {
                serialPort1.StopBits = System.IO.Ports.StopBits.Two;
            }

            serialPort1.Open();
        }

        private void comboBoxPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxChanged();
        }

        private void comboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxChanged();
        }

        private void comboBoxDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxChanged();
        }

        private void comboBoxParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxChanged();
        }

        private void comboBoxStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxChanged();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
