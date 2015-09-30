
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

            bool taskRegister = false;
            string myRegister = "MyTask";
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == myRegister)
                {
                    taskRegister = true;
                    break;
                }
            }
            if (taskRegister == false)
            {
                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder{
                    Name =myRegister,
                    TaskEntryPoint = "UWPStart.Common.NotifyBackground"                    
                };
                taskBuilder.SetTrigger(new ApplicationTrigger());
                BackgroundTaskRegistration task = taskBuilder.Register();
                task.Completed += Task_Completed;
            }


            await Task.Delay(TimeSpan.FromSeconds(30));
            _deferral.Complete();
        }

        private void Task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            var setting = Windows.Storage.ApplicationData.Current.LocalSettings;
            var key = sender.TaskId.ToString();
            setting.Values["taskID"] = key;
            //var message = setting.Values[key].ToString();
           
        }
    }
}
