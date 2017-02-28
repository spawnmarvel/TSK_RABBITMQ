using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.IO;
using Communication.Amqp;


namespace Communication.Send
{
    class Producer : AmqpMessagingService
    {
      
        public Producer()
        {
            
        }

        public string publishMsg(IModel model,  string messages)
        {
            string info = "";
            try
            {
                IBasicProperties basicProp = model.CreateBasicProperties();
                //set persistent true, meaning the msg can be recoverd
                basicProp.Persistent = true;
                //msg
                byte[] load = Encoding.UTF8.GetBytes(messages);
                PublicationAddress adr = new PublicationAddress(ExchangeType.Topic, "to_oil_3", "values");
                model.BasicPublish(adr, basicProp, load);
                info = messages;
                model.Dispose();
            }
            catch (NullReferenceException msg)
            {
                info = "" + msg;
            }

            return "Publishes msg " + info;
        }

    }
}
