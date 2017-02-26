using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.IO;


namespace Communication.Send
{
    class Sender
    {
        public static string test()
        {
            string info = "";

            try {
                //connection
                ConnectionFactory conFac = new ConnectionFactory();
                conFac.HostName = "localhost";
                conFac.UserName = "guest";
                conFac.Password = "guest";
                IConnection con = conFac.CreateConnection();
                //IModel represents channel to AMQP server
                IModel model = con.CreateModel();
                //From IModel we can access methods to send and receive msg and more

                //create queue:name, durable, exclusive, autodelte, arguments
                //Durability: durable(meaning messages can be recovered)
                model.QueueDeclare("oil_1", true, false, false, null);
                //create exchange
                //Topic exchanges route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange. 
                model.ExchangeDeclare("to_oil_1", ExchangeType.Topic);
                //bind the them with routing key
                model.QueueBind("oil_1", "to_oil_1", "oil_rate_values");
                info = "Success";
            }
            catch(FileNotFoundException msg)
            {
                info = "" + msg;
            }

            catch (Exception msg)
            {
                info = "" + msg;
            }

            return info;

        }

    }
}
