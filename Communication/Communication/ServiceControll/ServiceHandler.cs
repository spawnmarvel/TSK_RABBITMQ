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

        public ServiceHandler(string name)
        {

            this.name = name;
        }

        /// <summary>
        /// add reference System.ServiceProcess (in Framework )
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool stopService(string serviceName)
        {
            bool status = false;
            try
            {
               
                ServiceController service = new ServiceController(serviceName);
                if (service.Status.Equals(ServiceControllerStatus.Running))
                {
                    // stop the service
                    service.Stop();
                    status = true;
                }
            }
            catch(Exception msg)
            {
                logger.Debug(msg);
            }
            return status;
        }

    }
}
