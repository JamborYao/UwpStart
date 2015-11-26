using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI.Notifications;


namespace Tasks
{
    public sealed class ToastBackgroundTask: IBackgroundTask
    {
        //
        // Summary:
        //     执行后台任务的工作。在关联的后台任务被触发时，系统调用此方法。
        //
        // Parameters:
        //   taskInstance:
        //     后台任务的实例的接口。在关联的后台任务被触发运行时，系统创建此实例。
        public  void Run(IBackgroundTaskInstance taskInstance)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values["toastlog"] = settings.Values["toastlog"] + "**************" + "Background task!";
             _deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
            _taskInstance = taskInstance;
            //sendToast();
          
             _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(PeriodicTimerCallback), TimeSpan.FromSeconds(5));

        }
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)       { 
           // 
            // Indicate that the background task is canceled. 
            // 
            _cancelRequested = true; 
            _cancelReason = reason; 

 
        
        }

    ThreadPoolTimer _periodicTimer = null;
        volatile bool _cancelRequested = false;
        uint _progress = 0;
        IBackgroundTaskInstance _taskInstance = null;
        BackgroundTaskDeferral _deferral = null;
        BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        private void PeriodicTimerCallback(ThreadPoolTimer timer)
        {
            if ((_cancelRequested == false) && (_progress < 100))
            {
                _progress += 10;
                sendToast(_progress);
                _taskInstance.Progress = _progress;
              
            }
            else
            {
                _periodicTimer.Cancel();
              

                _deferral.Complete();
            }
        }


        private void sendToast(uint _progress)
        {
            string offtopic_ToastActionXML =
          "<toast>"
          + "  <visual>"
          + "    <binding template='ToastGeneric'>"
          + "      <text>{0}</text>"
          + "      <text>{1}</text>"
          + "      <image placement='AppLogoOverride' src='StoreLogo.png' />"
          + "    </binding>"
          + "  </visual>"
          + "  <actions>"
          + "<input id = 'message' type = 'text' placeholderContent = 'remark:' />"
          + "    <action content='Mark as Offtopic' arguments='offtopic'  activationType='foreground'/>" //background  foreground
          + "    <action content='{2}' arguments='ignore' />"
          + "  </actions>"
          + "</toast>";
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            badgeDOM.LoadXml(string.Format(offtopic_ToastActionXML, _progress+"% completed!", "testa","test"));
            DateTime starttime = DateTime.Now.AddSeconds(10);
            ToastNotification toast = new ToastNotification(badgeDOM);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            toast.Activated += Toast_Activated; 
        }



        private void Toast_Activated(ToastNotification sender, object args)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values["toastlog"] = settings.Values["toastlog"] + "**************" + "Background toast active task!";
        }
    }
}
