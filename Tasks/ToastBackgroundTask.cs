using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
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
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
             _deferral.Complete();


        }
    }
}
