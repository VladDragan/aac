using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworksApi.TCP.SERVER;

namespace CookingFormServer
{
    public delegate void UpdateChatLog(string text);
    public delegate void UpdateListBox(ListBox box, string value, bool remove);
    public partial class Form1 : Form
    {
        Server server;

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            server = new Server("192.168.0.102","90");
            server.OnClientConnected += new OnConnectedDelegate(Server_OnClientConnected);
            server.OnClientDisconnected += new OnDisconnectedDelegate(Server_OnClientDisconnected);
            server.OnDataReceived += new OnReceivedDelegate(Server_OnDataReceived);
            server.OnServerError += new OnErrorDelegate(Server_OnServerError);

            server.Start();

        }

        public void ChangeChatLog(string text)
        {
            if (textBox1.InvokeRequired)
            {
                Invoke(new UpdateChatLog(ChangeChatLog),new object[] { text});
            }
            else
            {
                textBox1.Text += text + "\r\n";
            }
        }

        private void ChangeListBox(ListBox box, string value, bool remove)
        {
            if (box.InvokeRequired)
            {
                Invoke(new UpdateListBox(ChangeListBox), new object[] { box, value, remove });
            }
            else
            {
                if (remove)
                {
                    box.Items.Remove(value);
                }
                else
                {
                    box.Items.Add(value);
                }
              
            }
        }

        private void Server_OnServerError(object Sender, ErrorArguments R)
        {
            MessageBox.Show(R.ErrorMessage);
        }

        private void Server_OnDataReceived(object Sender, ReceivedArguments R)
        {
            ChangeChatLog(R.Name + " says: " + R.ReceivedData); ;
           // server.BroadCast(R.Name+ " says: "+R.ReceivedData);

        }

        private void Server_OnClientDisconnected(object Sender, DisconnectedArguments R)
        {
            server.BroadCast(R.Name + " disconnected");
            ChangeListBox(listBox1, R.Name, true);
            ChangeListBox(listBox2, R.Ip, true);
        }

        private void Server_OnClientConnected(object Sender, ConnectedArguments R)
        {
            server.SendTo(R.Name, "Hello client on " + R.Name + "! Please insert your order :) ");
            ChangeListBox(listBox1, R.Name, false);
            ChangeListBox(listBox2, R.Ip, false);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox2.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server.BroadCast(textBox2.Text);
            textBox2.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.SendTo(listBox1.SelectedItem.ToString(), textBox2.Text);
            textBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            server.DisconnectClient(listBox1.SelectedItem.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = listBox1.SelectedIndex;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
