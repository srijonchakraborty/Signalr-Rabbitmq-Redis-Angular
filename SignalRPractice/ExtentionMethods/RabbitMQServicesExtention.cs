using Common.Model;
using RabbitMQ.Client;
using SignalRPractice.Services.RabbitMQService.Contract;
using SignalRPractice.Services.RabbitMQService.Service;

namespace SignalRPractice.ExtentionMethods
{
    public static class RabbitMQServicesExtention
    {
        public static void AddRabbitMqServices(this IServiceCollection services, AppSettings appSettings)
        {
            if (services != null)
            {
                var customport = appSettings.RabbitMQConnection.CustomPort;
                services.AddScoped<RabbitMQ.Client.IConnection>(sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = appSettings.RabbitMQConnection.HostName,
                        Port = string.IsNullOrEmpty(customport) ? Protocols.DefaultProtocol.DefaultPort : Int32.Parse(customport),
                        UserName = appSettings.RabbitMQConnection.UserName,
                        Password = appSettings.RabbitMQConnection.Password,
                        VirtualHost= appSettings.RabbitMQConnection.VirtualHost,
                        ContinuationTimeout = new TimeSpan(0, 1, 0, 0),
                        AutomaticRecoveryEnabled = true,
                        DispatchConsumersAsync = true
                    };
                    return factory.CreateConnection();
                });
                services.AddScoped<IRabbitMQProducerService, RabbitMQProducerService>();
            }
        }
    }
}
