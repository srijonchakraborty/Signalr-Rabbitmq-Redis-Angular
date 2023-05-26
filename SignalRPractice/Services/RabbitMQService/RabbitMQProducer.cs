using Common.Model;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace SignalRPractice.Services.RabbitMQService
{
    public class RabbitMQProducer:IRabbitMQProducer
    {
        private readonly RabbitMQ.Client.IConnection _rabbitMQConnection;
        private readonly AppSettings _appSettings;
        public RabbitMQProducer(RabbitMQ.Client.IConnection rabbitMQConnection,AppSettings appSettings)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _appSettings= appSettings;
            SetSpecificTaskConnection();
        }
        private void SetSpecificTaskConnection()
        {
            try
            {
                var exchangeName = _appSettings?.RabbitMQConnection?.SpecificTask?.ExchangeNamePub ?? "";
                var queueName = _appSettings?.RabbitMQConnection?.SpecificTask?.QueueNamePub ?? "";
                var routingKey = _appSettings?.RabbitMQConnection?.SpecificTask?.RoutingKeyPub ?? "";
                //Creating Exchange
                using (var channel = _rabbitMQConnection.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false);
                }

                //Queue
                using (var channel = _rabbitMQConnection.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false);
                }

                //Bind Queue with Exchange
                using (var channel = _rabbitMQConnection.CreateModel())
                {
                    channel.QueueBind(queueName, exchangeName, routingKey);
                }
            }
            catch (Exception ex)
            {
                //TODO : We will change it to logging
                Console.WriteLine(ex.Message);
            }
        }
        public void PublishSpecificTaskMessage(string message)
        {
            try
            {
                var exchangeName = _appSettings?.RabbitMQConnection?.SpecificTask?.ExchangeNamePub ?? "";
                var queueName = _appSettings?.RabbitMQConnection?.SpecificTask?.QueueNamePub ?? "";
                var routingKey = _appSettings?.RabbitMQConnection?.SpecificTask?.RoutingKeyPub ?? "";
                
                using (var channel = _rabbitMQConnection.CreateModel())
                {
                    channel.ConfirmSelect();
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;

                    channel.BasicPublish(exchange: exchangeName, routingKey: routingKey,
                                         basicProperties: properties, Encoding.UTF8.GetBytes(message));
                };
            }
            catch (Exception ex)
            {
                //TODO : We will change it to logging
                Console.WriteLine(ex.Message);
            }
        }
    }
}
