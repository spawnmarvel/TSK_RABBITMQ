using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Files
{
    class Read
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Read));


        public Read()
        {

        }
        public static string readFile(string filename)
        {
            logger.Info("Started read");
            string res = "";
            try
            {
                StreamReader sr = new StreamReader(filename, Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    res += line + "\n;";

                }
                sr.Dispose();

            }
            catch (FileNotFoundException msg)
            {

                res = msg.ToString();
                logger.Error(res);
            }
            catch (IOException msg)
            {
                logger.Error(msg);
                res = msg.ToString();
            }
            logger.Info(res);
            return res;

        }
    }
}
