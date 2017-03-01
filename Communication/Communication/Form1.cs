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
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(mainForm));
        private BackgroundWorker bw;


        public mainForm()
        {
            InitializeComponent();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            logger.Info("Btn send");
            int start = 1;
            int work = 2;
            int reTry = 3;
            Producer producer = new Producer();
            int state = start;
            if (state == start)
            {
                Helper.followTextBoxLog(richTextBoxLogs, "Send", "Starting, add open file and send, check for loss, cool");
                bool status = producer.getRabbitMqConnection();
                Helper.followTextBoxLog(richTextBoxLogs, "Send", "Connection is " + status);
                if (status == true)
                {
                    state = work;

                    if (state == work)
                    {
                        Helper.followTextBoxLog(richTextBoxLogs, "Send", "State is work " + state);
                        string msg = textBoxSend.Text;
                        if (msg.Length < 2)
                        {
                            Helper.followTextBoxLog(richTextBoxLogs, "Send", "Enter a new messages where char is > 2");
                            state = work;
                            producer.getIconnection().Close();
                            logger.Info("Closing connection");
                        }
                        else
                        {
                            IModel mod = producer.getIModel();
                            string sent = producer.publishMsg(msg);
                            Helper.followTextBoxLog(richTextBoxLogs, "Send", sent);
                            textBoxSend.Text = "";
                            state = start;

                        }
                    }
                }
                else
                {
                    state = reTry;
                    producer.reConnectToRabbit();
                    string info = "State is retry " + state + ". Is RabbitRunning? ";
                    Helper.followTextBoxLog(richTextBoxLogs, "Send", info);
                    if (producer.getRabbitMqConnection() == true)
                    {
                        state = work;
                        Helper.followTextBoxLog(richTextBoxLogs, "Send", "Reconnect is ok, state is work " + state);
                    }

                }

            }

        }




        private void buttonReceive_Click(object sender, EventArgs e)
        {
            string rec = "Start";
            Helper.followTextBoxLog(richTextBoxRecieve, "Rec1", rec);
            Helper.followTextBoxLog(richTextBoxLogs, "Rec1", rec);
            Consumer cons = new Consumer();
            string fromConsumer = cons.recieveMsg();
            Helper.followTextBoxLog(richTextBoxRecieve, "Rec1", fromConsumer);


        }

        private void buttonRecieveAll_Click(object sender, EventArgs e)
        {
            string rec = "Start";
            Helper.followTextBoxLog(richTextBoxRecieve, "RecAll", rec);
            Helper.followTextBoxLog(richTextBoxLogs, "RecAll", rec);
            Consumer cons = new Consumer();
            string fromConsumer = cons.recieveAllMsg();

            Helper.followTextBoxLog(richTextBoxRecieve, "RecAll", fromConsumer);

        }
    }
}
