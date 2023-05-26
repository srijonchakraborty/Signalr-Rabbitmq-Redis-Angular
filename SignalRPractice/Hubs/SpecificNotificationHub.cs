using Microsoft.AspNetCore.SignalR;

namespace SignalRPractice.Hubs
{
    public class SpecificNotificationHub : Hub
    {
        public async Task NotificationSubcribeToASpecificTask(string subscriptionId)
        {
            var item = Context;
            //add information to redis
            await Clients.All.SendAsync("Confirmed Subcription.", subscriptionId);
        }
    }
}
