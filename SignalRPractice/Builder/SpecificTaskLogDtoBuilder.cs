using Common.DTO;
using Common.Model;

namespace SignalRPractice.Builder
{
    internal class SpecificTaskLogDtoBuilder
    {
        public static SpecificTaskLogDto SpecificTaskLogBuild(string message, string subscriptionId,string connectionId)
        {
            SpecificTaskLogDto specificTask = new SpecificTaskLogDto()
            {
                Id = Guid.NewGuid().ToString(),
                SubscriptionId = subscriptionId,
                CreatedDateTime = DateTime.UtcNow,
                Description = "SpecificTaskLog",
                Message = message,   
                ConnectionId= connectionId
            };
            return specificTask;
        }
        public static SpecificTaskLogDto SpecificTaskLogBuildFromJson(string message)
        {
            var receivedMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<SpecificTaskLogDto>(message);
            return receivedMessage;
        }
    }
}
