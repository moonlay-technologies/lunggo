using Microsoft.Azure.NotificationHubs;

namespace Lunggo.WebAPI.ApiSrc.Notification.Model
{
    public class Notification
    {
        public static Notification Instance = new Notification();

        public NotificationHubClient Hub { get; set; }

        private Notification()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString("<your hub's DefaultFullSharedAccessSignature>",
                                                                         "<hub name>");
        }
    }
}