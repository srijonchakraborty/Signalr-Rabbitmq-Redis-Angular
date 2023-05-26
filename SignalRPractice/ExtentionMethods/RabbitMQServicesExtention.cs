using Common.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Connections;
using RabbitMQ.Client;
using SignalRPractice.Hubs;
using SignalRPractice.Services.RabbitMQService;
using SignalRPractice.Services.RedisService;

namespace SignalRPractice.ExtentionMethods
{
    public static class RabbitMQServicesExtention
    {
        public static void AddRabbitMqServices(this IServiceCollection services,AppSettings appSettings)
        {
            if (services != null)
            {
                services.AddSingleton<RabbitMQ.Client.IConnection>(sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = appSettings.RabbitMQConnection.HostName,
                        Port = Int32.Parse(appSettings.RabbitMQConnection.CustomPort),
                        UserName = appSettings.RabbitMQConnection.UserName,
                        Password = appSettings.RabbitMQConnection.Password,
                        VirtualHost= appSettings.RabbitMQConnection.VirtualHost,
                        ContinuationTimeout = new TimeSpan(0, 1, 0, 0),
                        AutomaticRecoveryEnabled = true,
                        DispatchConsumersAsync = true
                    };
                    return factory.CreateConnection();
                });
                services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
            }
        }
    }
}
