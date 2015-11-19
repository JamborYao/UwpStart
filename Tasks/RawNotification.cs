using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.PushNotifications;
using Windows.Storage;

namespace Tasks
{
    public sealed class RawNotificationTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            RawNotification notification = (RawNotification)taskInstance.TriggerDetails;
            settings.Values[taskInstance.Task.Name] = notification.Content;
            _deferral.Complete();


        }
    }
}
