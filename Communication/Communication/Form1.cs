using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication.Utility;
using Communication.Send;
using Communication.Recieve;
using RabbitMQ.Client;


namespace Communication
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            int start = 1;
            int work = 2;
            int reTry = 3;
           
            Producer producer = new Producer();
            int state = start;
            if (state == start)
            {
                Helper.followTextBoxLog(richTextBoxLogs, "Starting");
                bool status = producer.getRabbitMqConnection();
                Helper.followTextBoxLog(richTextBoxLogs, "Connection is " + status);
                if (status == true)
                {
                    state = work;

                    if (state == work)
                    {
                        Helper.followTextBoxLog(richTextBoxLogs, "State is work " + state);
                        string msg = textBoxSend.Text;
                        if (msg.Length < 5)
                        {
                            Helper.followTextBoxLog(richTextBoxLogs, "Enter a new messages, 5 chars it to small");
                            state = work;
                        }
                        else
                        {
                            IModel mod = producer.getIModel();
                            string sent = producer.publishMsg(mod, msg);
                            Helper.followTextBoxLog(richTextBoxLogs, sent);
                            textBoxSend.Text = "";
                            state = start;
                            
                        }
                    }
                }
                else
                {
                    state = reTry;
                    producer.reConnectToRabbit();
                    string info = "State is retry " + state;
                    Helper.followTextBoxLog(richTextBoxLogs, info);
                    if (producer.getRabbitMqConnection() == true)
                    {
                        state = work;
                        Helper.followTextBoxLog(richTextBoxLogs, "Reconnect is ok, state is work " + state);
                    }

                }

            }

        }




        private void buttonReceive_Click(object sender, EventArgs e)
        {
            string rec = "Start\n";
            Helper.followTextBoxLog(richTextBoxRecieve, rec);
            Helper.followTextBoxLog(richTextBoxLogs, rec);
            Consumer cons = new Consumer();
            bool status = cons.getRabbitMqConnection();
            Helper.followTextBoxLog(richTextBoxLogs, "Rabbit con " + status);
            IModel mod = cons.getIModel();
            string queue = cons.getQueue();
            Helper.followTextBoxLog(richTextBoxLogs, "Get from " + queue);
            string fromConsumer = cons.recieveMsg(mod, queue);
            Helper.followTextBoxLog(richTextBoxRecieve, fromConsumer);
        }


    }
}
