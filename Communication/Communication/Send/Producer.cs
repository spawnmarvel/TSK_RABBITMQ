using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.IO;
using Communication.Files;



namespace Communication.Send
{
    class Producer // : AmqpMessagingService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Producer));

        //Persistence property on three levels
        //Queue: model.QueueDeclare("oil_2", true, false, false, null);
        //Exchange:model.ExchangeDeclare("to_oil_2", ExchangeType.Topic, true);
        //Msg:basicProp.Persistent = true;
        private ConnectionFactory conFac { get; set; }
        private IModel model { get; set; }
        private IConnection con { get; set; }
        private bool connectionStatus;
        private string queue = "oil_5";

        public Producer()
        {
            //connection status in only true if try getRabbitMqConnction has no exceptions
            connectionStatus = false;
        }

        public bool getStatusRabbitMqConnection()
        {
            return connectionStatus;
        }
        public string getQueue()
        {
            return queue;
        }
        public IModel getIModel()
        {
            return model;
        }
        public IConnection getIconnection()
        {
            return con;
        }

        /// <summary>
        /// create rabbitMQ connection
        /// </summary>
        /// <returns></returns>
        public bool createRabbitMqConnection()
        {
            logger.Info("Create / Get rabbitmq connection");
            string info = "";
            try
            {

                //connection
                conFac = new ConnectionFactory();
                conFac.HostName = "localhost";
                conFac.UserName = "guest";
                conFac.Password = "guest";
                conFac.RequestedHeartbeat = 30;
                //automatic reconnect
                conFac.AutomaticRecoveryEnabled = true;
                //conFac.NetworkRecoveryInterval = TimeSpan.FromSeconds(20);
                con = conFac.CreateConnection();
                setUpInitialTopicQueue();
                connectionStatus = true;
                info = "Success";
                logger.Info("rabbitmq connection = " + info);

            }

            catch (BrokerUnreachableException msg)
            {
                info = " " + msg;
                connectionStatus = false;
                logger.Error(info);
            }
            //added this if rabbit dll is missing
            catch (FileNotFoundException msg)
            {
                info = "" + msg;
                logger.Info(info);
            }
            catch (Exception msg)
            {
                info = "" + msg;
                logger.Info(info);
            }


            return connectionStatus;

        }
        public void reConnectToRabbit()
        {
            conFac.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            connectionStatus = false;
            logger.Info("In reconnect..");

        }
        /// <summary>
        /// initialize model / channel
        /// </summary>
        private void setUpInitialTopicQueue()
        {
            logger.Info("Set up initial topic queue");
            model = con.CreateModel();
            //create queue:name, durable, exclusive, autodelte, arguments
            //Durability: durable(meaning the queue can be recovered)
            model.QueueDeclare(queue, true, false, false, null);
            //create exchange
            //Topic exchanges route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange. 
            //add true to declare durable exchange
            model.ExchangeDeclare("to_oil_5", ExchangeType.Topic, true);
            //bind the them with routing key
            model.QueueBind(queue, "to_oil_5", "values");
        }

        /// <summary>
        /// send 1 pkt to rabbitMQ
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public string publishMsg(string messages)
        {
            logger.Info("Publish");
            string info = "";
            int id = 0;
            try
            {
                IBasicProperties basicProp = model.CreateBasicProperties();
                //set persistent true, meaning the msg can be recoverd
                basicProp.Persistent = true;
                messages = messages + ";";
                byte[] load = Encoding.UTF8.GetBytes(messages);
                PublicationAddress adr = new PublicationAddress(ExchangeType.Topic, "to_oil_5", "values");
                model.BasicPublish(adr, basicProp, load);
                info = messages;
                string pub = adr.ToString();
                info += "ID: " + id + ": To " + pub;
                logger.Info("Message = " + info + "Published: " + pub);
                id++;

            }
            catch (NullReferenceException msg)
            {
                info = "" + msg;
                logger.Error(info);
            }
            //model.Dispose();
            con.Close();
            logger.Info("Closing connection");

            return "Published msg:" + info;
        }

        /// <summary>
        /// send a file to rabbitMQ
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string publishFile(string file)
        {
            logger.Info("Starting send file");
            string res = "File is sent, amount of pkt's ";
            string message = "";
            int pub = 0;
            try
            {
                string[] arr = Read.readFile(file).Split('.');
                IBasicProperties basicProp = model.CreateBasicProperties();
                //set persistent true, meaning the msg can be recoverd
                basicProp.Persistent = true;
                for (int i = 0; i < arr.Length; i++)
                {
                    // IBasicProperties basicProp = model.CreateBasicProperties();
                    //set persistent true, meaning the msg can be recoverd
                    //basicProp.Persistent = true;
                    //pub = i;
                    //message = "Id-" + pub +"-"+arr[i];
                    message = arr[i];
                    if (message.Length == 1)
                    {
                        //it empty
                    }
                    else {
                        pub = i+1;//we ditch the zero and start at 1
                       
                        message = "Id-" + pub + "-" + message;
                        logger.Debug(message + "len:" + message.Length);
                        byte[] load = Encoding.UTF8.GetBytes(message);
                        PublicationAddress adr = new PublicationAddress(ExchangeType.Topic, "to_oil_5", "values");
                        model.BasicPublish(adr, basicProp, load);
                    }
                }

            }
            catch (NullReferenceException msg)
            {
                logger.Error(msg);
                res = msg.ToString();
            }
            //res += Read.readFile(file);
            res += " = " + pub;
            logger.Info(res);
            return res;
        }

    }



}
