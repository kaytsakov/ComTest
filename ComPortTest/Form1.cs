using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComPortTest
{
    public partial class Form1 : Form
    {
        long picoSecondsMax = 0;
        long picoSecondsAvr = 0;
        long picoSecondsMin = 0;

        long CountOfCykles = 0;
        long setCountData = 0;

        string TxData="DATA";

        DateTime picoSecondsFixDataTx;
        DateTime picoSecondsFixDataRx;

        long Ticks1 = 0;
        long Ticks2 = 0;
        Stopwatch sw = new Stopwatch();
        //sw.Start();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Enabled && !serialPort1.IsOpen) serialPort1.Open();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            TxData = textBox5.Text;

            if (serialPort1.IsOpen) button1.BackColor = Color.GreenYellow;
            else button1.BackColor = Color.Red;

            if (serialPort2.IsOpen) button4.BackColor = Color.GreenYellow;
            else button4.BackColor = Color.Red;

            if (serialPort1.IsOpen && serialPort2.IsOpen && textBox1.Text != "" && textBox5.Text != "") button5.Visible = true;
            else button5.Visible = false;

            if (checkBox1.Checked == true) checkBox1.Invoke(new Action(() => checkBox1.Text = "Round-trip data transmission"));
            else checkBox1.Invoke(new Action(() => checkBox1.Text = "One-way data transmission"));

            foreach (string s in SerialPort.GetPortNames())
            {
                if (!comboBox1.Items.Contains(s))
                {
                    // Console.WriteLine(s);
                    // serialPortNames[portNamesCount]=s;
                    // portNamesCount++;
                    comboBox1.Items.Add(s);
                    comboBox3.Items.Add(s);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.Write("Port 1: ");
            Console.WriteLine(comboBox1.Text);
            serialPort1.PortName = comboBox1.Text;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.Write("Port 2: ");
            Console.WriteLine(comboBox3.Text);
            serialPort2.PortName = comboBox3.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 1:
                    serialPort1.BaudRate = 2400;
                    break;
                case 2:
                    serialPort1.BaudRate = 4800;
                    break;
                case 3:
                    serialPort1.BaudRate = 9600;
                    break;
                case 4:
                    serialPort1.BaudRate = 14400;
                    break;
                case 5:
                    serialPort1.BaudRate = 19200;
                    break;
                case 6:
                    serialPort1.BaudRate = 28800;
                    break;
                case 7:
                    serialPort1.BaudRate = 38400;
                    break;
                case 8:
                    serialPort1.BaudRate = 57600;
                    break;
                case 9:
                    serialPort1.BaudRate = 76800;
                    break;
                case 10:
                    serialPort1.BaudRate = 115200;
                    break;
                case 11:
                    serialPort1.BaudRate = 230400;
                    break;
                case 12:
                    serialPort1.BaudRate = 250000;
                    break;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.SelectedIndex)
            {
                case 1:
                    serialPort2.BaudRate = 2400;
                    break;
                case 2:
                    serialPort2.BaudRate = 4800;
                    break;
                case 3:
                    serialPort2.BaudRate = 9600;
                    break;
                case 4:
                    serialPort2.BaudRate = 14400;
                    break;
                case 5:
                    serialPort2.BaudRate = 19200;
                    break;
                case 6:
                    serialPort2.BaudRate = 28800;
                    break;
                case 7:
                    serialPort2.BaudRate = 38400;
                    break;
                case 8:
                    serialPort2.BaudRate = 57600;
                    break;
                case 9:
                    serialPort2.BaudRate = 76800;
                    break;
                case 10:
                    serialPort2.BaudRate = 115200;
                    break;
                case 11:
                    serialPort2.BaudRate = 230400;
                    break;
                case 12:
                    serialPort2.BaudRate = 250000;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
            long T1, T2;
            T1 = DateTime.Now.Ticks;
            T2 = DateTime.Now.Ticks;
            Console.WriteLine(T2 - T1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort2.IsOpen) serialPort2.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Enabled && !serialPort2.IsOpen) serialPort2.Open();
        }

        private void serialPort1_Read(object sender, SerialDataReceivedEventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                long TimeOfRxTx = 0;
                string ReadDarta;
                ReadDarta = serialPort1.ReadLine();
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                sw.Stop();
                long microseconds = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));

                TimeOfRxTx = microseconds;
                sw.Reset();
                Console.Write("Round-trip: ");
                Console.WriteLine(TimeOfRxTx);

                if (ReadDarta == TxData)
                {
                    if (picoSecondsAvr == 0 && picoSecondsMax == 0 && picoSecondsMin == 0)
                    {
                        picoSecondsAvr = TimeOfRxTx;
                        picoSecondsMax = TimeOfRxTx;
                        picoSecondsMin = TimeOfRxTx;
                    }
                    else
                    {
                        if (TimeOfRxTx > picoSecondsMax) picoSecondsMax = TimeOfRxTx;
                        if (TimeOfRxTx < picoSecondsMin && TimeOfRxTx != 0) picoSecondsMin = TimeOfRxTx;
                        picoSecondsAvr += TimeOfRxTx;
                    }
                    CountOfCykles++;
                    if (CountOfCykles < setCountData)
                    {
                        sw.Start();
                        serialPort1.WriteLine(TxData);
                    }
                    else
                    {
                        picoSecondsAvr /= setCountData;
                        textBox2.Invoke(new Action(() => textBox2.Text = picoSecondsAvr.ToString()));
                        textBox3.Invoke(new Action(() => textBox3.Text = picoSecondsMin.ToString()));
                        textBox4.Invoke(new Action(() => textBox4.Text = picoSecondsMax.ToString()));
                    }
                }
            }
        }

        private void serialPort2_Read(object sender, SerialDataReceivedEventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                long TimeOfRxTx = 0;
                string ReadDarta;
                ReadDarta = serialPort2.ReadLine();
                serialPort2.DiscardOutBuffer();
                serialPort2.DiscardInBuffer();
                sw.Stop();
                long microseconds = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));

                TimeOfRxTx = microseconds;
                sw.Reset();
                Console.Write("One-way: ");
                Console.WriteLine(TimeOfRxTx);

                if (ReadDarta == TxData)
                {
                    if (picoSecondsAvr == 0 && picoSecondsMax == 0 && picoSecondsMin == 0)
                    {
                        picoSecondsAvr = TimeOfRxTx;
                        picoSecondsMax = TimeOfRxTx;
                        picoSecondsMin = TimeOfRxTx;
                    }
                    else
                    {
                        if (TimeOfRxTx > picoSecondsMax) picoSecondsMax = TimeOfRxTx;
                        if (TimeOfRxTx < picoSecondsMin && TimeOfRxTx != 0) picoSecondsMin = TimeOfRxTx;
                        picoSecondsAvr += TimeOfRxTx;
                    }
                    CountOfCykles++;
                    if (CountOfCykles < setCountData)
                    {
                        sw.Start();
                        serialPort1.WriteLine(TxData);
                    }
                    else
                    {
                        picoSecondsAvr /= setCountData;
                        textBox2.Invoke(new Action(() => textBox2.Text = picoSecondsAvr.ToString()));
                        textBox3.Invoke(new Action(() => textBox3.Text = picoSecondsMin.ToString()));
                        textBox4.Invoke(new Action(() => textBox4.Text = picoSecondsMax.ToString()));
                    }
                }
            }
            else
            {
                //long TimeOfRxTx = 0;
                string ReadDarta;
                ReadDarta = serialPort2.ReadLine();
                serialPort2.DiscardOutBuffer();
                serialPort2.DiscardInBuffer();
                serialPort2.WriteLine(ReadDarta);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CountOfCykles = 0;
            picoSecondsAvr = 0;
            picoSecondsMax = 0;
            picoSecondsMin = 0;
            setCountData = Convert.ToInt32(textBox1.Text);
            //Console.WriteLine(setCountData);
            //picoSecondsFixDataTx = DateTime.Now;
            sw.Start();
            //Ticks1 = DateTime.Now.Ticks;
            serialPort1.WriteLine(TxData);
            //picoSecondsFixDataTx = DateTime.Now;
        }
    }
}
