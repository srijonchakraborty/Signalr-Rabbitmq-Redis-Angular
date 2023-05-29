using Common.DTO;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerForNotification.Builder
{
    internal class SpecificTaskLogBuilder
    {
        public static SpecificTaskLog SpecificTaskLogBuild(string message)
        {
            SpecificTaskLog specificTask = new SpecificTaskLog() {
               Id= Guid.NewGuid().ToString(),
               SubscriptionId = Guid.NewGuid().ToString(),
               CreatedDateTime= DateTime.UtcNow,
               Description= "SpecificTaskLog",
               Message= message
            };
            return specificTask;
        }
        public static SpecificTaskLog SpecificTaskLogBuildFromJson(string message)
        {
            var receivedMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<SpecificTaskLog>(message);
            if (string.IsNullOrWhiteSpace(receivedMessage.Id))
            {
                receivedMessage.Id = Guid.NewGuid().ToString();
            }
            return receivedMessage;
        }
    }
}
