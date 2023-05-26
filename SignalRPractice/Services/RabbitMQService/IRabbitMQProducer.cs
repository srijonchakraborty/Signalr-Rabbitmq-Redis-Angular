namespace SignalRPractice.Services.RabbitMQService
{
    public interface IRabbitMQProducer
    {
        void PublishSpecificTaskMessage(string message);
    }
}
