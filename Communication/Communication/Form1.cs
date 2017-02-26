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
            string send = textBoxSend.Text;
            Helper.followTextBoxLog(richTextBoxLogs, send);
            //Helper.followTextBoxLog(richTextBoxLogs, Sender.test());
          

        }

        private void buttonReceive_Click(object sender, EventArgs e)
        {
            string rec = "Will implement connection and get msg from MQ, not just simulating pressed, data -> " + textBoxSend.Text;
            Helper.followTextBoxLog(richTextBoxRecieve, rec);
            Helper.followTextBoxLog(richTextBoxLogs, rec);
        }


    }
}
