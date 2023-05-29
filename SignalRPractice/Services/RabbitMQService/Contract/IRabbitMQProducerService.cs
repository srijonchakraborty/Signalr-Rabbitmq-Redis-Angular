namespace SignalRPractice.Services.RabbitMQService.Contract
{
    public interface IRabbitMQProducerService
    {
        Task PublishSpecificTaskMessageAsync(string message);
    }
}
