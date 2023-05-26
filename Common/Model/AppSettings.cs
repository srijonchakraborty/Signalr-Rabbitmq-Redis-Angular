using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    #region AppSettings
    public class AppSettings
    {
        public string[] CorsSetting { get; set; }
        public RedisConnectionModel RedisConnection { get; set; }
        public RabbitMQConnectionModel RabbitMQConnection { get; set; }
        public MongoConnectionModel MongoConnection { get; set; }
    }
    #endregion

    #region Base ConnectionStringModel
    public class ConnectionStringModel
    {
        public string ConnectionString { get; set; }
    }
    #endregion

    #region MongoConnectionModel
    public class MongoConnectionModel : ConnectionStringModel
    {

    }
    #endregion
    #region RedisConnectionModel
    public class RedisConnectionModel : ConnectionStringModel
    {
        public string InstanceName { get; set; }
        public int DefaultSlidingExpiration { get; set; }
        public int DefaultAbsoluteExpiration { get; set; }
    }
    #endregion

    #region RabbitMQConnectionModel
    public class RabbitMQConnectionModel
    {
        public string HostName { get; set; }
        public string CustomPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string ProducerConnectionName { get; set; }
        public RabbitMQTaskConnection SpecificTask { get; set; }
        
    }
    public class RabbitMQTaskConnection
    {
        public string RoutingKeyPub { get; set; }
        public string QueueNamePub { get; set; }
        public string ExchangeNamePub { get; set; }
        public string RoutingKeySub { get; set; }
        public string QueueNameSub { get; set; }
        public string ExchangeNameSub { get; set; }
        public string ProducerConnectionName { get; set; }
    }
    #endregion
}
