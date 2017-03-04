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
        private static Boolean fileOK = false;


        public mainForm()
        {

            InitializeComponent();
            textBoxFile.Enabled = false;
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
                Helper.followTextBoxLog(richTextBoxLogs, "Send", "Starting, add open file and send, check for loss, cool\nAlso simulate pkt loss, by ditch ack return");
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


        private void LoadNewFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                userSDelectedFilePath = ofd.FileName;
                fileOK = true;
            }


            //throw new NotImplementedException();
        }
        public string userSDelectedFilePath
        {
            get
            {
                return textBoxFile.Text;
            }
            set
            {
                textBoxFile.Text = value;
            }
        }

        private void buttonSendFile_Click(object sender, EventArgs e)
        {

        }

        private void textBoxFile_TextChanged(object sender, EventArgs e)
        {

        }


        private void openFileDialog1_FileOk_2(object sender, CancelEventArgs e)
        {

        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            logger.Info("Ope file");
            LoadNewFile();
            Helper.followTextBoxLog(richTextBoxLogs, "Open file", textBoxFile.Text);
        }

        private void buttonSendFile_Click_1(object sender, EventArgs e)
        {
            Producer producer = new Producer();
            string file = textBoxFile.Text;
            //check that file is fileOK before sending
            string res = producer.publishFile(file);
            Helper.followTextBoxLog(richTextBoxLogs, "SendAll", res);
        }
    }
}
