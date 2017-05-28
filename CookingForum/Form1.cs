using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworksApi.TCP.CLIENT;

namespace CookingForum
{
    public delegate void UpdateText(string text);
    public partial class Form1 : Form
    {

        Client client;

        public Form1()
        {
            InitializeComponent();
        }

        private void ChangeTextBox(string text)
        {
            if (textBox1.InvokeRequired)
            {
                Invoke(new UpdateText(ChangeTextBox), new object[] { text });
            }
            else
            {
                textBox1.AppendText(text + "\r\n");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && comboBox1.Text != "" && textBox5.Text != "") 
            {
                client = new Client();
                client.ClientName = comboBox1.Text;
                client.ServerIp = textBox3.Text;
                client.ServerPort = textBox5.Text;

                client.OnClientConnected += new OnClientConnectedDelegate(client_OnClientConnected);
                client.OnClientConnecting += new OnClientConnectingDelegate(client_OnClientConnecting);
                client.OnClientDisconnected += new OnClientDisconnectedDelegate(client_OnClientDisconnected);
                client.OnClientError += new OnClientErrorDelegate(client_OnClientError);
                client.OnClientFileSending+= new OnClientFileSendingDelegate(client_OnClientFileSending);
                client.OnDataReceived += new OnClientReceivedDelegate(client_OnDataReceived);

                
                client.Connect();
                button3.Enabled = false;
            }
            else
            {
                MessageBox.Show("You must fill all the textBoxes");
            }
        }

        private void client_OnDataReceived(object Sender, ClientReceivedArguments R)
        {
            ChangeTextBox("Kitchen says: " + R.ReceivedData);
        }

        private void client_OnClientFileSending(object Sender, ClientFileSendingArguments R)
        {
            
        }

        private void client_OnClientError(object Sender, ClientErrorArguments R)
        {
            ChangeTextBox(R.ErrorMessage);
            button3.Enabled = true;
        }


        private void client_OnClientDisconnected(object Sender, ClientDisconnectedArguments R)
        {
            ChangeTextBox(R.EventMessage);
            button3.Enabled = true;
        }

        private void client_OnClientConnecting(object Sender, ClientConnectingArguments R)
        {
            ChangeTextBox(R.EventMessage);
        }

        private void client_OnClientConnected(object Sender, ClientConnectedArguments R)
        {
            ChangeTextBox(R.EventMessage);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(client !=null && client.IsConnected)
            {
                client.Send(textBox2.Text);
                ChangeTextBox("Me: " + textBox2.Text);
                textBox2.Clear();
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if  (client != null && client.IsConnected && e.KeyCode == Keys.Enter)
                {
                    client.Send(textBox2.Text);
                    ChangeTextBox("Me: " + textBox2.Text);
                    textBox2.Clear();
                }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
