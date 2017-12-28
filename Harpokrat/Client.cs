using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Harpokrat
{
    public partial class Client : Form
    {
        private static ClientCode.Client client;

        public Client()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = new ClientCode.Client();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                client.Message.Message = textBox1.Text;
                client.SendMessage();
                textBox2.Text += client.Receive();
                textBox2.Text += "\n";
            }
            catch
            {
                if (client == null)
                    MessageBox.Show("There is no open connection...");
                else
                    client.CloseSocket();
            }
        }
    }
}
