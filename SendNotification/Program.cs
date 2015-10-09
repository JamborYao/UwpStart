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
        private const string packageSID = "ms-app://s-1-15-2-402962206-181889125-2784327023-3017467611-124665055-2608589449-1588084589";
        private const string clientSecret = "lw6IVYDiA7R3ZJuJCyw8AmljgStCFSMi";
        static void Main(string[] args)
        {
            while (true)
            {
                string action = "";
                Console.WriteLine("Please input the team new thread belong to:" );
                string name = Console.ReadLine();
                Console.WriteLine("Please input the message you want to notify:");
                string message = Console.ReadLine();
                //Console.WriteLine("Need action? yes or no");
                //string action = Console.ReadLine();               
                SendNotificationAsync(message,name,action);               
                Console.WriteLine("Please input 'q' to exit"); 
                string exit = Console.ReadLine();
                if (exit == "q")
                {
                  Environment.Exit(0);
                }        
            }
            
        }
        private static async void SendNotificationAsync(string message,string name,string action)
        {
            action = "no";
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://jamobilehub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=nST5ysIRWqwR8X55dsAiUDvbMgcsjNJ7Jgyl7W5iSD0=", "uwpstart");
             string id=await hub.CreateRegistrationIdAsync();


            var toast = string.Format(@"<toast><visual><binding template=""ToastText01""><text id=""1"">{0}</text></binding></visual></toast>", message);
                        var tags = new string[1];
            
            tags[0] = name;

            if (action == "yes")
            {
                await hub.SendWindowsNativeNotificationAsync(string.Format( ToastWithActionXML,message), tags);
            }
            else
            {
                await hub.SendWindowsNativeNotificationAsync(toast, tags);
            }
        }
        private static  string ToastWithActionXML =
                "<toast>"
                + "  <visual>"
                + "    <binding template='ToastGeneric'>"
                + "      <text>test</text>"
                + "      <text>{0}</text>"
                + "      <image placement='AppLogoOverride' src='StoreLogo.png' />"
                + "    </binding>"
                + "  </visual>"
                + "  <actions>"
                + "<input id = 'message' type = 'text' placeholderContent = 'reply here' />"
                + "    <action content='Mark as Offtopic' arguments='check' activationType='forceground'/>"
                + "    <action content='cancel' arguments='cancel' />"
                + "  </actions>"
                //  + "  <audio src='ms - winsoundevent:Notification.Reminder'/>"
                + "</toast>";
    }
}
