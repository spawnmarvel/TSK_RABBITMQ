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
using Communication.ServiceControll;


namespace Communication
{
    public partial class mainForm : Form

    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(mainForm));
        private BackgroundWorker bw;
        private static Boolean fileOK = false;
        private Producer producer;
        private ServiceHandler handler;


        public mainForm()
        {
            logger.Info("Main started");
            InitializeComponent();
            textBoxFile.Enabled = false;
            producer = new Producer();
            producer.createRabbitMqConnection();
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
                producer.createRabbitMqConnection();
                bool status = producer.getStatusRabbitMqConnection();
                Helper.followTextBoxLog(richTextBoxLogs, "Send", "Connection is " + status);
                if (status == true)
                {
                    state = work;

                    if (state == work)
                    {
                        Helper.followTextBoxLog(richTextBoxLogs, "Send", "State is work " + state);
                       
                        string msg = textBoxSend.Text;
                        progressBar.Value = 50;
                        if (msg.Length < 2)
                        {
                            Helper.followTextBoxLog(richTextBoxLogs, "Send", "Enter a new messages where char is > 2");
                            state = work;
                            producer.getIconnection().Close();
                            logger.Info("Closing connection");
                        }
                        else
                        {
                            //IModel mod = producer.getIModel();
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
                    if (producer.createRabbitMqConnection() == true)
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
            progressBar.Value = 0;

        }





        private void openFileDialog1_FileOk_2(object sender, CancelEventArgs e)
        {

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
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            logger.Info("Open file");
            LoadNewFile();
            Helper.followTextBoxLog(richTextBoxLogs, "Open file", textBoxFile.Text);
        }



        private void buttonSendFile_Click(object sender, EventArgs e)
        {
            if (fileOK == true)
            {
               
                //producer.createRabbitMqConnection();//moved to main start in form
                if(producer.getStatusRabbitMqConnection() == false)
                {
                    logger.Debug("What is connection status " + producer.getStatusRabbitMqConnection());
                    producer.createRabbitMqConnection();
                   // producer.reConnectToRabbit();
                }
                logger.Info(producer.getRmqProp());
                logger.Info("What is connection status " + producer.getStatusRabbitMqConnection());
                string file = textBoxFile.Text;
                //check that file is fileOK before sending
                string res = producer.publishFile(file);
                Helper.followTextBoxLog(richTextBoxLogs, "SendAll", res);
            }
            else
            {
                Helper.followTextBoxLog(richTextBoxLogs, "SendAll", "The file can not be empty");
            }

        }

        private void buttonSumilatePktLoss_Click(object sender, EventArgs e)
        {
            Helper.followTextBoxLog(richTextBoxLogs, "Rec1", "Simulating pkt loss, no ack is sent back to RabbitMQ");
            //recieveMsgButNotSendAck
        }

        private void buttonSimulatePktLoss_Click(object sender, EventArgs e)
        {
            Helper.followTextBoxLog(richTextBoxLogs, "Rec1NoAck", "Simulating pkt loss, no ack is sent back to RabbitMQ");
            Consumer cons = new Consumer();
            string pktWithNoAck = cons.recieveMsgButNotSendAck();
            Helper.followTextBoxLog(richTextBoxRecieve, "Rec1NoAck", pktWithNoAck);
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string status;
            handler = new ServiceHandler("Maaster", "RabbitMQ");
            backgroundWorker.RunWorkerAsync(handler);
            string rv = "background woker";
            Helper.followTextBoxLog(richTextBoxLogs, "Try stop RMQ", rv);
        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string rv = "";
            //https://msdn.microsoft.com/es-es/library/cc221403(v=vs.95).aspx
            //https://www.dotnetperls.com/backgroundworker
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; (i <= 10); i++)
            {
                rv = "Progress " + i;
                logger.Debug(rv);
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress((i * 10));
                    rv += " progress " + i;
                    logger.Debug(rv);
                }
            }
           
        }
    }
}
