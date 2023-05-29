using Common.Model;
using RabbitConsumerForNotification.Constants;
//using RabbitConsumerForNotification.Model;
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
            BuildRabbitMQBasic();
            BuildSpecificTaskRabbitMQ();
            BuildMongoConnection();
        }
        private static void BuildMongoConnection()
        {
            CustomConstant.CurrentAppSettings.MongoConnection = new MongoConnectionModel();
            CustomConstant.CurrentAppSettings.MongoConnection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["mongodbconnection"];
            CustomConstant.CurrentAppSettings.MongoConnection.InstanceName = System.Configuration.ConfigurationManager.AppSettings["mongodbInstanceName"];
        }
        private static void BuildRabbitMQBasic()
        {
            CustomConstant.CurrentAppSettings.RabbitMQConnection = new RabbitMQConnectionModel();
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
        private static void BuildSpecificTaskRabbitMQ()
        {
            //Web Pub is Console Consumers Sub and Web's Sub is Console Consumers Pub
            CustomConstant.CurrentAppSettings.RabbitMQConnection.SpecificTask=new RabbitMQTaskConnection();
            CustomConstant.CurrentAppSettings.RabbitMQConnection.SpecificTask.RoutingKeySub = 
                    System.Configuration.ConfigurationManager.AppSettings["WebSpecificTaskPubKey"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.SpecificTask.QueueNameSub =
                    System.Configuration.ConfigurationManager.AppSettings["WebSpecificTaskPubQueue"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.SpecificTask.ExchangeNameSub =
                    System.Configuration.ConfigurationManager.AppSettings["WebSpecificTaskPubExchange"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.SpecificTask.RoutingKeyPub =
                    System.Configuration.ConfigurationManager.AppSettings["WebSpecificTaskSubKey"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.SpecificTask.QueueNamePub =
                    System.Configuration.ConfigurationManager.AppSettings["WebSpecificTaskSubQueue"];
            CustomConstant.CurrentAppSettings.RabbitMQConnection.SpecificTask.ExchangeNamePub =
                    System.Configuration.ConfigurationManager.AppSettings["WebSpecificTaskSubExchange"];

        }
    }
}
