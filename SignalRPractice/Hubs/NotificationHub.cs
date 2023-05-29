using Microsoft.AspNetCore.SignalR;

namespace SignalRPractice.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var item = Context;
            await Clients.All.SendAsync("TPP", user, message);
        }
    }
}
