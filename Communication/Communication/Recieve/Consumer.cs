using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Events;
using Communication.Amqp;

namespace Communication.Recieve
{
    class Consumer : AmqpMessagingService
    {

        public Consumer()
        {

        }

        public string recieveMsg(IModel mod, string queue)
        {
            string msg = "not get yet";
            var reciever = new EventingBasicConsumer(mod);
            reciever.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                msg = "Recieved " + message;
            };
            mod.BasicConsume(queue, noAck: true, consumer: reciever);
            mod.Dispose();


            return msg;
        }
    }
}
