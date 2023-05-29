using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using SignalRPractice.Hubs;
using System.Text;
using Common.Model;
using Common.DTO;
using SignalRPractice.Services.RedisService;
using MongoDB.Driver.Core.Connections;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRPractice.Services.RabbitMQService.Service
{
    public class RabbitMQSpecificTaskListenerService : BackgroundService
    {
        private readonly RabbitMQ.Client.IConnection connectionRabbitMQ;
        private  IModel channel;
        private readonly IHubContext<SpecificNotificationHub> hubContext;
        private readonly AppSettings appSettings;
        private readonly IRedisService redisService;
        public RabbitMQSpecificTaskListenerService(
            IServiceScopeFactory factory,
            IRedisService redisService,
            IHubContext<SpecificNotificationHub> hubContext,
            IOptions< AppSettings> appSettings)
        {
            
            var scope = factory.CreateScope();
            connectionRabbitMQ = scope.ServiceProvider.GetRequiredService<RabbitMQ.Client.IConnection>();
            this.hubContext = hubContext;
            this.appSettings= appSettings.Value;
            this.redisService= redisService;
        }
       
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this.channel.Close();
            this.connectionRabbitMQ.Close();

            await base.StopAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            ConnectSub();
            StartListenToRabbitMQ();
            return Task.CompletedTask;
        }
        private void ConnectSub()
        {
            try
            {
                var exchangeName = this.appSettings.RabbitMQConnection?.SpecificTask?.ExchangeNameSub ?? "";
                var queueName = this.appSettings.RabbitMQConnection?.SpecificTask?.QueueNameSub ?? "";
                var routingKey = this.appSettings.RabbitMQConnection?.SpecificTask?.RoutingKeySub ?? "";

                using (var channel = connectionRabbitMQ.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false);
                }

                using (var channel = connectionRabbitMQ.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false);
                }

                using (var channel = connectionRabbitMQ.CreateModel())
                {
                    channel.QueueBind(queueName, exchangeName, routingKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void StartListenToRabbitMQ()
        {
            try
            {
                var queueName = this.appSettings.RabbitMQConnection.SpecificTask.QueueNameSub;
                this.channel = connectionRabbitMQ.CreateModel();
                this.channel.BasicQos(0, 1, false);
                var rabbitChannelConsumer = new AsyncEventingBasicConsumer(this.channel);
                rabbitChannelConsumer.Received += ChannelConsumer_Received; ;
                this.channel.BasicConsume(queueName, true, rabbitChannelConsumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task ChannelConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"Received:{message}");
                await ProcessMessaage(message);
                this.channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task ProcessMessaage(string subscriptionId)
        {
            var message= await this.redisService.GetAsync<string>(subscriptionId);
            SpecificTaskLogDto specificTaskLogDto = Builder.SpecificTaskLogDtoBuilder.SpecificTaskLogBuildFromJson(message);

            var client=hubContext?.Clients?.Client(specificTaskLogDto.ConnectionId);
            if (client != null)
            {
                await hubContext?.Clients?.Client(specificTaskLogDto.ConnectionId)?
                                 .SendAsync(this.appSettings.SignalRValue.SpecificNotificationReceiveMethod,
                                            $"Done Task: SubID: {subscriptionId}");
            }
        }
    }
}
