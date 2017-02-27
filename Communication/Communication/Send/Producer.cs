using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.IO;


namespace Communication.Send
{
    class Producer
    {
        // Persistence property on three levels
        //Queue: model.QueueDeclare("oil_2", true, false, false, null);
        //Exchange:model.ExchangeDeclare("to_oil_2", ExchangeType.Topic, true);
        //Msg:basicProp.Persistent = true;
        private ConnectionFactory conFac { get; set; }
        private IModel model { get; set; }
        private IConnection con { get; set; }
        private bool connectionStatus;


        public Producer()
        {
            //connection status in only true if try getRabbitMqConnction has no exceptions
            connectionStatus = false;
        }

        public bool getRabbitMqConnection()
        {

            string info = "";
            try
            {
               
                    //connection
                    conFac = new ConnectionFactory();
                    conFac.HostName = "localhost";
                    conFac.UserName = "guest";
                    conFac.Password = "guest";
                    //automatic reconnect
                    conFac.AutomaticRecoveryEnabled = true;
                    //conFac.NetworkRecoveryInterval = TimeSpan.FromSeconds(20);
                    con = conFac.CreateConnection();
                    setUpInitialTopicQueue();
                    connectionStatus = true;
                    info = "Success";

            }

            catch (BrokerUnreachableException msg)
            {
                info = " " + msg;
                connectionStatus = false;
            }
            //added this if rabbit dll is missing
            catch(FileNotFoundException msg)
            {
                info = "" + msg;
            }
            catch (Exception msg)
            {
                info = "" + msg;
            }


            return connectionStatus;

        }
        public void reConnectToRabbit()
        {
            conFac.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            connectionStatus = false;

        }
        private void setUpInitialTopicQueue()
        {
            model = con.CreateModel();
            //create queue:name, durable, exclusive, autodelte, arguments
            //Durability: durable(meaning the queue can be recovered)
            model.QueueDeclare("oil_3", true, false, false, null);
            //create exchange
            //Topic exchanges route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange. 
            //add true to declare durable exchange
            model.ExchangeDeclare("to_oil_3", ExchangeType.Topic, true);
            //bind the them with routing key
            model.QueueBind("oil_3", "to_oil_3", "values");
        }

        public string publishMsg(string messages)
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
            }
            catch (NullReferenceException msg)
            {
                info = "" + msg;
            }

            return "Publishes msg " + info;
        }

    }
}
