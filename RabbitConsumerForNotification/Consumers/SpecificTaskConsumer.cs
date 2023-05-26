using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using RabbitConsumerForNotification.Constants;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitConsumerForNotification.Consumers
{
    internal class SpecificTaskConsumer
    {
        private static IConnection _rabbitMqConnectionSub;
        private static IModel _myChannelSub;
        private static IConnection _rabbitMqConnectionPub;
        private static IModel _myChannelPub;
        private static void CreateConnection()
        {
            try
            {
                var hostName = CustomConstant.CurrentAppSettings?.RabbitMQConnection.HostName;
                var customport = CustomConstant.CurrentAppSettings?.RabbitMQConnection.CustomPort;
                var userName = CustomConstant.CurrentAppSettings?.RabbitMQConnection.UserName;
                var password = CustomConstant.CurrentAppSettings?.RabbitMQConnection.Password;
                var virtualHost = CustomConstant.CurrentAppSettings?.RabbitMQConnection.VirtualHost;
                var producerConnectionName = CustomConstant.CurrentAppSettings?.RabbitMQConnection.ProducerConnectionName;

                var factorySub = new ConnectionFactory()
                {
                    HostName = hostName,
                    Port = string.IsNullOrEmpty(customport) ? Protocols.DefaultProtocol.DefaultPort : Int32.Parse(customport),
                    UserName = userName,
                    Password = password,
                    VirtualHost = virtualHost,
                    ContinuationTimeout = new TimeSpan(0, 1, 0, 0)
                };
                factorySub.AutomaticRecoveryEnabled = true;
                factorySub.DispatchConsumersAsync = true;
                _rabbitMqConnectionSub = factorySub.CreateConnection(producerConnectionName);


                var factoryPub = new ConnectionFactory()
                {
                    HostName = hostName,
                    Port = string.IsNullOrEmpty(customport) ? Protocols.DefaultProtocol.DefaultPort : Int32.Parse(customport),
                    UserName = userName,
                    Password = password,
                    VirtualHost = virtualHost,
                    ContinuationTimeout = new TimeSpan(0, 1, 0, 0)
                };
                factoryPub.AutomaticRecoveryEnabled = true;
                factoryPub.DispatchConsumersAsync = true;
                _rabbitMqConnectionPub = factoryPub.CreateConnection(producerConnectionName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void ConnectPub()
        {
            try
            {
                var exchangeName = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.ExchangeNamePub ?? "";
                var queueName = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.QueueNamePub ?? "";
                var routingKey = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.RoutingKeyPub ?? "";

                //Creating Exchange
                using (var channel = _rabbitMqConnectionPub.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false);
                }

                //Queue
                using (var channel = _rabbitMqConnectionPub.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false);
                }

                //Bind Queue with Exchange
                using (var channel = _rabbitMqConnectionPub.CreateModel())
                {
                    channel.QueueBind(queueName, exchangeName, routingKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void ConnectSub()
        {
            try
            {
                var exchangeName = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.ExchangeNameSub ?? "";
                var queueName = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.QueueNameSub ?? "";
                var routingKey = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.RoutingKeySub ?? "";


                //Creating Exchange
                using (var channel = _rabbitMqConnectionSub.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false);
                }

                //Queue
                using (var channel = _rabbitMqConnectionSub.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false);
                }

                //Bind Queue with Exchange
                using (var channel = _rabbitMqConnectionSub.CreateModel())
                {
                    channel.QueueBind(queueName, exchangeName, routingKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void Connect()
        {
            CreateConnection();
            ConnectSub();
            ConnectPub();
        }
        internal static void StartListenToRabbitMQ()
        {
            try
            {
                var queueName = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.QueueNameSub ?? "";
                Connect();
                _myChannelSub = _rabbitMqConnectionSub.CreateModel();

                _myChannelSub.BasicQos(0, 1, false);
                var smsChannelConsumer = new AsyncEventingBasicConsumer(_myChannelSub);
                smsChannelConsumer.Received += ChannelConsumer_Received; ;
                _myChannelSub.BasicConsume(queueName, true, smsChannelConsumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static Task ChannelConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"Received:{message}");
                _myChannelSub.BasicAck(e.DeliveryTag, false);
                Thread.Sleep(5000);
                SendLogRunningTaskCompleted($"Long Running Task Done: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.FromResult(true);
        }
        private static void SendLogRunningTaskCompleted(string message)
        {
            try
            {
                var exchangeName = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.ExchangeNamePub ?? "";
                var routingKey = CustomConstant.CurrentAppSettings?.RabbitMQConnection?.SpecificTask?.RoutingKeyPub ?? "";

                using (var channel = _rabbitMqConnectionPub.CreateModel())
                {
                    channel.ConfirmSelect();
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    channel.BasicPublish(exchange: exchangeName, routingKey: routingKey,
                                         basicProperties: properties, Encoding.UTF8.GetBytes(message));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
