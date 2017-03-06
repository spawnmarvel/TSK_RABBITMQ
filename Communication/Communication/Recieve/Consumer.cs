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
            logger.Info("Create / get rabbit mq connection");
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
            string res = "";

            try
            {
                getRabbitMqConnection();
                logger.Info("Recieve->");//


                // do a simple poll of the queue
                var data = model.BasicGet(queue_, false);
                // the message is null if the queue was empty
                if (data == null)
                {
                    res = "The queue is empty / recieveMsg";
                    return res;
                }
                // convert the message back from byte[] to a string
                var message = Encoding.UTF8.GetString(data.Body);
                // ack the message, ie. confirm that we have processed it
                // otherwise it will be requeued a bit later
                model.BasicAck(data.DeliveryTag, false);

                res += message.ToString();
                logger.Info("Res length " + res.Length);
                if (res.Length < 2)
                {
                    res = "The queue is empty / recieveMsg";
                }
                con.Close();
                logger.Info("Recieved = " + res);
                logger.Info("Closing connection");
                return res;
            }
            catch (NotSupportedException msg)
            {

                logger.Error(msg);
                logger.Info("If we get this exception (above), just re-add the RabbitMQ.Client.dll. This happens with version / github ");
                res = msg.ToString();

            }
            catch(NullReferenceException msg)
            {
                logger.Error(msg);
                res =  msg.ToString() + "\n Is RabbitMQ running?";
            }
            return res;
        }
        public string recieveAllMsg()
        {
            string res = "";

            try
            {
                getRabbitMqConnection();
                logger.Info("Recieve All->");//
                int count = 0;

                uint pktSize = model.MessageCount(queue_);
                int size = (int)pktSize;
                logger.Info("Size " + size);
                for (int i = 0; i < size; i++)
                {
                    // do a simple poll of the queue
                    var data = model.BasicGet(queue_, false);
                    // the message is null if the queue was empty
                    if (data == null)
                    {
                        res = "(Loop)The queue is empty / recieveMsg";
                    }
                    else {
                        if (count == 0)
                        {
                            res += "\n";//first line
                        }
                        // convert the message back from byte[] to a string
                        var message = Encoding.UTF8.GetString(data.Body);
                        res += message.ToString();// + "\n";
                        // ack the message, ie. confirm that we have processed it
                        // otherwise it will be requeued a bit later
                        //model.BasicAck(data.DeliveryTag, false);
                        model.BasicAck(data.DeliveryTag, false);
                    }
                }


                logger.Info("Res length " + res.Length);
                if (res.Length < 1)
                {
                    res = "The queue is empty / recieveAllMsg";
                }
                con.Close();
                logger.Info("Recieved = " + res);
                logger.Info("Closing connection");
                return res;
            }
            catch (NotSupportedException msg)
            {

                logger.Error(msg);
                logger.Info("If we get this exception (above), just re-add the RabbitMQ.Client.dll. This happens with version / github ");
                res = msg.ToString();

            }
            catch (NullReferenceException msg)
            {
                logger.Error(msg);
                res = msg.ToString() + "\n Is RabbitMQ running?";
            }
            return res;
        }



    }
}
