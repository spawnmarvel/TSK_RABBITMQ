using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Communication.ServiceControll
{
    class ServiceHandler
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ServiceHandler));
        private string name;

        public ServiceHandler(string name, string service_name)
        {

            this.name = name;
            stopService(service_name);
        }

        /// <summary>
        /// add reference System.ServiceProcess (in Framework )
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public void stopService(string serviceName)
        {
            bool status = false;
            string result = "Empty";
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(15000);
                   
                //http://www.csharp-examples.net/restart-windows-service/
                ServiceController service = new ServiceController(serviceName);
                if (service.Status.Equals(ServiceControllerStatus.Running))
                {
                    // stop the service
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    status = true;
                    result = "stopped";
                }
            }
            catch(Exception msg)
            {
                logger.Debug(msg);
                result = " " + msg;
            }
            //return result;
        }

    }
}
