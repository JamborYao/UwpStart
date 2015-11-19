using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SendNotification;

namespace WebNotifyBackend
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string uri = "https://hk2.notify.windows.com/?token=AwYAAACviBUvGU%2bT4KEPHF%2bhODv92ZMlOSQTBAkq1Vd0Y3a2b4DnT7Z00mK%2fsAglkCRTbqCW0FW7SOjeUlpZR%2bMeGBi%2bsRL8H2JoIG2Lw22qo44QU6ZnTPg0sYqdR1hCBBtTSMI%3d";
               
                tbChannel.Text = uri;
                tbSID.Text = WNSNotification.packageSID;
                tbSecret.Text = WNSNotification.clientSecret;
                 tbPush.Text = toast;
            }
        }
        public string toast = string.Format(@"<toast><visual><binding template=""ToastText01""><text id=""1"">{0}</text></binding></visual></toast>", "hello world!!!");

        public string  TileXml =
            "<tile>"
            + "<visual version='3'>"
            + "<binding template='TileSquare150x150Text04' fallback='TileSquareText04'>"
            + "<text id='1'>http update</text>"
            + "</binding>"
            + "</visual>"
            + "</tile>";
        protected void Unnamed6_Click(object sender, EventArgs e)
        {
            WNSNotification WnsNotify = new WNSNotification();
            
            WnsNotify.SendNotifytoWNS(content:tbPush.Text, NotififyType:dpType.SelectedItem.Text,url:tbChannel.Text);
        }

        protected void dpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((sender as DropDownList).SelectedItem.Text)
            {   
                case "toast":
                    tbPush.Text = toast;
                    break;
                case "tile":
                    tbPush.Text = TileXml;
                    break;
                case "raw":
                    tbPush.Text = "";
                    break;
                case "badge":
                    tbPush.Text = "";
                    break;
            }
        }
    }
}