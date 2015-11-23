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
                flag = 0;
            }
        }
        public string toast = string.Format(@"<toast><visual><binding template=""ToastText01""><text id=""1"">{0}</text></binding></visual></toast>", "hello world!!!");

        public string TileXml =
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

            steps = WnsNotify.SendNotifytoWNS(content: tbPush.Text, NotififyType: dpType.SelectedItem.Text, url: tbChannel.Text);            
            Session["Steps"] = steps;
            

            var kvs = steps.Select(kvp => string.Format("\"{0}\":\"{1}\"", kvp.Key, string.Join(",", kvp.Value.Replace("\"","\\\""))));
           
            string jsonResult = string.Concat("{", string.Join(",", kvs), "}");
            jsonResult = jsonResult.Replace('"', '\"');
            stepsValue.Value = jsonResult;
            
        }
       
        public Dictionary<FlowSteps, string> steps;
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
        public int flag = 0;
        protected void Step_click(object sender, EventArgs e)
        {
            

            switch (flag)
            {
                case 0:
                    lbSteps.Text = "";
                    lbSteps.Text = steps[FlowSteps.TokenUrl];
                    flag++;
                    break;
                case 1:
                    lbSteps.Text = "";
                    lbSteps.Text = steps[FlowSteps.SID];
                    flag++;
                    break;
                case 2:
                    lbSteps.Text = "";
                    lbSteps.Text = steps[FlowSteps.ClientSecret];
                    flag++;
                    break;
                case 3:
                    lbSteps.Text = "";
                    lbSteps.Text = steps[FlowSteps.Token];
                    flag++;
                    break;
                case 4:
                    lbSteps.Text = "";
                    lbSteps.Text = steps[FlowSteps.RequestURL];
                    flag++;
                    break;
                case 5:
                    lbSteps.Text = "";
                    lbSteps.Text = steps[FlowSteps.RequestContent];
                    flag++;
                    break;
                case 6:
                    lbSteps.Text = "";
                    lbSteps.Text = steps[FlowSteps.StateCode];
                    flag = 0;
                    break;

            }
        }
    }
}