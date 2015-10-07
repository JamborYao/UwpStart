using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace UWPStart.Common
{
    public class Notifications
    {
        public static void SendToastNotification()
        {
            
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();           
            badgeDOM.LoadXml(Common.NotificationXML.ToastWithActionXML);
            ToastNotification toast = new ToastNotification(badgeDOM);         
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        public static void SendToastMessage(string title, string message)
        {          
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            badgeDOM.LoadXml(string.Format(Common.NotificationXML.ToastInternetXML,title,message));
            ToastNotification toast = new ToastNotification(badgeDOM);        
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
