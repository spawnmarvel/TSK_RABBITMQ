using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Events;
using Communication.Amqp;
using System.IO;
using RabbitMQ.Client.MessagePatterns;

namespace Communication.Recieve
{
    class Consumer// : AmqpMessagingService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Consumer));
        //Persistence property on three levels
        //Queue: model.QueueDeclare("oil_2", true, false, false, null);
        //Exchange:model.ExchangeDeclare("to_oil_2", ExchangeType.Topic, true);
        //Msg:basicProp.Persistent = true;
        private ConnectionFactory conFac { get; set; }
        private IModel model { get; set; }
        private IConnection con { get; set; }
        private bool connectionStatus;
        private string queue_ = "oil_5";

        public Consumer()
        {
            //connection status in only true if try getRabbitMqConnction has no exceptions
            connectionStatus = false;
        }
        public string getQueue()
        {
            return queue_;
        }
        public IModel getIModel()
        {
            return model;
        }
        public IConnection getIconnection()
        {
            return con;
        }
        public bool getRabbitMqConnection()
        {
            logger.Info("get rabbit mq connection");
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
                // con.AutoClose = true;
                setUpInitialTopicQueue();
                connectionStatus = true;
                info = "Success";
                logger.Info("Rabbitmq connection = " + info);

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
                logger.Error(info);
            }
            catch (Exception msg)
            {
                info = "" + msg;
                logger.Error(info);
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
            logger.Info("set up initial queue");
            model = con.CreateModel();
            //create queue:name, durable, exclusive, autodelte, arguments
            //Durability: durable(meaning the queue can be recovered)
            model.QueueDeclare(queue_, true, false, false, null);
            //create exchange
            //Topic exchanges route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange. 
            //add true to declare durable exchange
            model.ExchangeDeclare("to_oil_5", ExchangeType.Topic, true);
            //bind the them with routing key
            model.QueueBind(queue_, "to_oil_5", "values");

        }

        public string recieveMsg()
        {
            int start = 1;
            int workMany = 2;
            int work = 3;
            int done = 4;
            getRabbitMqConnection();
            setUpInitialTopicQueue();
            string res = "";
            logger.Info("Recieved");
            model.BasicQos(0, 1, false); //basic quality of service
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(queue_, false, consumer);
            uint x = model.MessageCount(queue_);
            int tmp = (int)x;
            logger.Info("Count " + x);
            if (tmp > 0)
            {
                for (int y = 0; y < x; y++)
                {

                    logger.Info("In loop, x is " + x);
                    BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                    String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                    logger.Info("Msg = " + message);
                    res += message;
                    model.BasicAck(deliveryArguments.DeliveryTag, false);
                    // return message;
                }
            }
            else//x is 0
            {
                logger.Info("No loop, x is " + x);
                BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                logger.Info("Msg = " + message);
                res += message;
                model.BasicAck(deliveryArguments.DeliveryTag, false);
                tmp = -1;

            }
            //clean up
            model.Dispose();
            con.Close();
            return res;
        }

        //public string recieveMsg()
        //{
        //    logger.Info("Recieve");
        //    getRabbitMqConnection();
        //    setUpInitialTopicQueue();
        //    string info = "try get msg: ";
        //    var cons = new EventingBasicConsumer(model);
        //    uint x = model.MessageCount(queue_);

        //    info += "Queue size " + x;
        //    logger.Info(info);
        //    cons.Received += (IModel, ea) =>
        //     {
        //         var body = ea.Body;
        //         var msg = Encoding.UTF8.GetString(body);
        //         info += msg;
        //         logger.Info("Recieved = " + msg);
        //     };
        //    model.BasicConsume(queue: queue_, noAck: true, consumer: cons);
        //    model.Dispose();
        //    con.Close();

        //    return info;

        //}

    }
}
