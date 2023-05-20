using RabbitConsumerForNotification.Constants;
using RabbitConsumerForNotification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerForNotification.Builder
{
    public static class AppSettingsBuilder
    {
        public static void AppSettingsBuild()
        {
            CustomConstant.CurrentAppSettings = new AppSettings();
            CustomConstant.CurrentAppSettings.RabbitMQConnection=new RabbitMQConnectionModel();
            CustomConstant.CurrentAppSettings.RabbitMQConnection.HostName = System.Configuration.ConfigurationManager.AppSettings["hostName"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.CustomPort = System.Configuration.ConfigurationManager.AppSettings["customport"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.UserName = System.Configuration.ConfigurationManager.AppSettings["userName"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.Password = System.Configuration.ConfigurationManager.AppSettings["password"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.VirtualHost = System.Configuration.ConfigurationManager.AppSettings["virtualHost"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.RoutingKey = System.Configuration.ConfigurationManager.AppSettings["routingKey"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.QueueName = System.Configuration.ConfigurationManager.AppSettings["queueName"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.ExchangeName = System.Configuration.ConfigurationManager.AppSettings["exchangeName"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.ProducerConnectionName = System.Configuration.ConfigurationManager.AppSettings["producerConnectionName"];
        }
    }
}
