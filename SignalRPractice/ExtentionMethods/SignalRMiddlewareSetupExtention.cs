using SignalRPractice.Hubs;

namespace SignalRPractice.ExtentionMethods
{
    public static class SignalRHubSetupExtention
    {
        public static void SetupSignalRHubs(this WebApplication app)
        {
            if (app != null)
            {
                app.MapHub<NotificationHub>("/notificationhub");
                app.MapHub<SpecificNotificationHub>("/specificnotificationhub");
            }
        }
    }
}
