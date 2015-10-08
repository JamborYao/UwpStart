using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string message = Console.ReadLine();
                
                SendNotificationAsync(message);
                
            }
        }
        private static async void SendNotificationAsync(string message)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://jamobilehub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=nST5ysIRWqwR8X55dsAiUDvbMgcsjNJ7Jgyl7W5iSD0=", "uwpstart");
            var toast = string.Format(@"<toast><visual><binding template=""ToastText01""><text id=""1"">{0}</text></binding></visual></toast>", message);
            await hub.SendWindowsNativeNotificationAsync(toast);
        }
    }
}
