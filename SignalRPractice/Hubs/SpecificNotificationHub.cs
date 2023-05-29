using Common.DTO;
using Common.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SignalRPractice.Services.RabbitMQService.Contract;
using SignalRPractice.Services.RedisService;

namespace SignalRPractice.Hubs
{
    public class SpecificNotificationHub : Hub
    {
        private readonly IRabbitMQProducerService rabbitMqService;
        private readonly IRedisService redisService;
        private AppSettings appSettings;
        public SpecificNotificationHub(IRabbitMQProducerService rabbitMqService,
                                       IRedisService redisService,
                                       IOptions<AppSettings> appSettings
            )
        {
            this.rabbitMqService = rabbitMqService;
            this.redisService = redisService;
            this.appSettings = appSettings.Value;
        }
        public async Task NotificationSubcribeToASpecificTask(string message, string subscriptionId)
        {
            SpecificTaskLogDto specificTaskLogDto = Builder.SpecificTaskLogDtoBuilder.SpecificTaskLogBuild(message, subscriptionId, Context.ConnectionId);
            var messageToPublish=Newtonsoft.Json.JsonConvert.SerializeObject(specificTaskLogDto);
            Publish(messageToPublish, subscriptionId);
           
        }
        private async Task Publish(string messageToPublish, string subscriptionId)
        {
            
            this.redisService.Set(subscriptionId, messageToPublish,
                                  this.appSettings.RedisConnection.DefaultAbsoluteExpiration,
                                 this.appSettings.RedisConnection.DefaultSlidingExpiration);
            this.rabbitMqService.PublishSpecificTaskMessageAsync(messageToPublish);
        }
    }
}
