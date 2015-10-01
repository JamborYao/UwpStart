
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace UWPStart.Common
{
    public sealed class NotifyBackground:IBackgroundTask
    {
       
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();           


            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            _deferral.Complete();
        }

        
    }
}
