using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace UWPStart.Common
{
    public class Notifications
    {
        public static void SendToastWithActionNotification(string toastXML, TypedEventHandler<ToastNotification, System.Object> toastActived)
        {
            
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();           
            badgeDOM.LoadXml(toastXML);
            ToastNotification toast = new ToastNotification(badgeDOM);
            if (toastActived != null)
            {
                toast.Activated += toastActived;
            }
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        public static void SendToastMessage(string title, string message)
        {          
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            badgeDOM.LoadXml(string.Format(Common.NotificationXML.ToastInternetXML,title,message));
            ToastNotification toast = new ToastNotification(badgeDOM);  
            //toast.en       
            
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            

        }


        public static void UpdateTile(string message)
        {
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            try
            {                
                badgeDOM.LoadXml(string.Format( Common.NotificationXML.TileXml,message));
                TileNotification tile = new TileNotification(badgeDOM);
              
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tile);                
            }
            catch (Exception)
            {

            }
        }
    }
}
