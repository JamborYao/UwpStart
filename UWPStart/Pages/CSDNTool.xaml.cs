using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPStart.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CSDNTool : Page
    {
        public CSDNTool()
        {
            this.InitializeComponent();
            //this.Loaded += CSDNTool_Loaded;
            RegisterTask();


        }
        private void RegisterBackgroundTask(object sender, RoutedEventArgs e)
        {
            RegisterBackgroundTask("UWPStart.Common.NotifyBackground",
                                                                 "TimeTaskNew",
                                                                 new SystemTrigger(SystemTriggerType.TimeZoneChange, false),
                                                                 null);

        }
        private async void CSDNTool_Loaded(object sender, RoutedEventArgs e)
        {
            string x = await Common.HttpHelper.MyHttpGet();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            // Myreg();

            base.OnNavigatedTo(e);
        }
        private void UnregisterBackgroundTask(object sender, RoutedEventArgs e)
        {
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                cur.Value.Unregister(true);

            }
        }
        public static void RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            //if (TaskRequiresBackgroundAccess(name))
            //{
            //    await BackgroundExecutionManager.RequestAccessAsync();
            //}

            var builder = new BackgroundTaskBuilder();

            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);

                //
                // If the condition changes while the background task is executing then it will
                // be canceled.
                //
                builder.CancelOnConditionLoss = true;
            }

            BackgroundTaskRegistration task = builder.Register();



            //
            // Remove previous completion status from local settings.
            //

        }

        private static bool TaskRequiresBackgroundAccess(string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterTask()
        {
            bool taskRegister = false;
            string myRegister = "MyTimeTask1";
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                //if (task.Value.Name == myRegister)
                //{
                //    taskRegister = true;
                //    break;
                //}
                task.Value.Unregister(true);
            }
            if (taskRegister == false)
            {
                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder
                {
                    Name = myRegister,
                    TaskEntryPoint = "Tasks.NotificationBackground"
                };
                //taskBuilder.SetTrigger(new SystemTrigger(SystemTriggerType.NetworkStateChange, false));
                taskBuilder.SetTrigger(new ToastNotificationActionTrigger());
                BackgroundTaskRegistration task = taskBuilder.Register();

                task.Completed += Task_Completed;
                task.Progress += Task_Progress;
            }
        }

        private void Task_Progress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            Debug.WriteLine("my task " + args.Progress + "%" + "downloaded!");
        }

        private void Task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            Debug.WriteLine("my task complete");
            //var setting = Windows.Storage.ApplicationData.Current.LocalSettings;
            //var key = sender.TaskId.ToString();
            //setting.Values["taskID"] = key;
        }

        private void updateTitle_Click(object sender, RoutedEventArgs e)
        {                     
            // ToastNotification 
            //  string badgeXmlString = "<badge value='" + textBox.Text + "'/>";
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            try
            {
                // Create a DOM.
                badgeDOM.LoadXml(Common.NotificationXML.TileXml);

                // Load the xml string into the DOM, catching any invalid xml characters.
                //BadgeNotification badge = new BadgeNotification(badgeDOM);

                //// Create a badge notification and send it to the application’s tile.
                //BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badge);

                TileNotification tile = new TileNotification(badgeDOM);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tile);


                //OutputTextBlock.Text = badgeDOM.GetXml();
                //rootPage.NotifyUser("Badge sent", NotifyType.StatusMessage);
            }
            catch (Exception)
            {
                //OutputTextBlock.Text = string.Empty;
                //rootPage.NotifyUser("Error loading the xml, check for invalid characters in the input", NotifyType.ErrorMessage);
            }
        }

        private void sendToast_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);           
            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            for (int i = 0; i < stringElements.Length; i++)
            {
                stringElements[i].AppendChild(toastXml.CreateTextNode("Line " + i));
            }
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            // Specify the absolute path to an image
            // String imagePath = "file:///" + Path.GetFullPath("toastImageAndText.png");
            //XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            badgeDOM.LoadXml(Common.NotificationXML.ToastWithActionXML);

            ToastNotification toast = new ToastNotification(badgeDOM);
           // toast.Activated += Toast_Activated;

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        //private void Toast_Activated(ToastNotification sender, object args)
        //{
        //    //throw new NotImplementedException();
        //}
    }
}
