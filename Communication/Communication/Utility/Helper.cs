using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//added for generic
using System.Windows.Forms;

namespace Communication.Utility
{
    class Helper
    {

        public static void followTextBoxLog(RichTextBox loggerForm, string action,  string log)
        {
            string da = DateTime.Now.ToString();
            if (loggerForm.Text.Length > 0)
            {
                loggerForm.AppendText(Environment.NewLine);
            }
            loggerForm.AppendText("Log:" + da + ": " + action + ": "+ log);
            loggerForm.ScrollToCaret();
        }
    }
}
