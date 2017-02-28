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

        //Persistence property on three levels
        //Queue: model.QueueDeclare("oil_2", true, false, false, null);
        //Exchange:model.ExchangeDeclare("to_oil_2", ExchangeType.Topic, true);
        //Msg:basicProp.Persistent = true;
        private ConnectionFactory conFac { get; set; }
        private IModel model { get; set; }
        private IConnection con { get; set; }
        private bool connectionStatus;
        private string queue_ = "oil_3";

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

            }

            catch (BrokerUnreachableException msg)
            {
                info = " " + msg;
                connectionStatus = false;
            }
            //added this if rabbit dll is missing
            catch (FileNotFoundException msg)
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
            model.QueueDeclare(queue_, true, false, false, null);
            //create exchange
            //Topic exchanges route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange. 
            //add true to declare durable exchange
            model.ExchangeDeclare("to_oil_3", ExchangeType.Topic, true);
            //bind the them with routing key
            model.QueueBind(queue_, "to_oil_3", "values");
        }

        public string recieveMsg()
        {
            string res = "start, ";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue_,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                uint x = model.MessageCount(queue_);
                res += "Count " + x + ", ";
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    res += message;
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                //false here then it is no ack, if true the it is autoack
                channel.BasicConsume(queue: queue_,
                                     noAck: true,
                                     consumer: consumer);


            }
            return res;
        }

        //public string recieveMsg()
        //{
        //    getRabbitMqConnection();
        //    setUpInitialTopicQueue();
        //    string info = "try get msg: ";
        //    var cons = new EventingBasicConsumer(model);
        //    uint x = model.MessageCount(queue_);

        //    info += "Queue size " + x;
        //    cons.Received += (IModel, ea) =>
        //     {
        //         var body = ea.Body;
        //         var msg = Encoding.UTF8.GetString(body);
        //         info += msg;
        //     };
        //    model.BasicConsume(queue: queue_, noAck: true, consumer: cons);
        //    model.Dispose();
        //    con.Close();

        //    return info;

        //}

    }
}
