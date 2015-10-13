﻿using System;
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
using Newtonsoft.Json;

using System.Collections.ObjectModel;
using CSDNServices;
using Windows.UI.ViewManagement;
using Windows.Graphics.Display;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPStart.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CSDNTool : Page
    {
        private ObservableCollection<ThreadsDetail> csdnThreads;
        private DateTime StartDate;
        private DateTime EndDate;
        private Dictionary<string,string> offtopics;
        private bool internetAviable = false;
        public CSDNTool()
        {
            this.InitializeComponent();
            this.Loaded += CSDNTool_Loaded;
            csdnThreads = new ObservableCollection<ThreadsDetail>();
            RegisterTask();
         

        }
        //private void RegisterBackgroundTask(object sender, RoutedEventArgs e)
        //{
        //    RegisterBackgroundTask("UWPStart.Common.NotifyBackground",
        //        "TimeTaskNew",
        //                                                         new SystemTrigger(SystemTriggerType.TimeZoneChange, false),
        //                                                         null);

        //}
        private async void CSDNTool_Loaded(object sender, RoutedEventArgs e)
        {
            endDate.Date = (new DateTimeOffset(DateTime.Now));
            startDate.Date = (new DateTimeOffset(new DateTime(DateTime.Now.Year, 8, 30)));
            DateTimeOffset QueryStart = (DateTimeOffset)startDate.Date;
            StartDate = QueryStart.DateTime;
            DateTimeOffset QueryEnd = (DateTimeOffset)endDate.Date;
            EndDate = QueryEnd.DateTime;

            string x = await Common.HttpHelper.HttpClientGetThreads(null, null);
            if (!string.IsNullOrEmpty(x))
            {
                internetAviable = true;
                csdnThreads = JsonConvert.DeserializeObject<ObservableCollection<ThreadsDetail>>(x);
                viewThreads.ItemsSource = csdnThreads;
            }
            Common.Notifications.UpdateTile("Total " + csdnThreads.Count ?? 0 + "threads!");

        }

        private void UnregisterBackgroundTask(object sender, RoutedEventArgs e)
        {
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                cur.Value.Unregister(true);
            }
        }
        public static void RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundTrigger trigger, IBackgroundCondition condition,
            BackgroundTaskCompletedEventHandler completeHandler
            )
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);
            if (condition != null)
            {
                builder.AddCondition(condition);
                builder.CancelOnConditionLoss = true;
            }
            BackgroundTaskRegistration task = builder.Register();
            if (completeHandler != null)
            {
                task.Completed += completeHandler;
            }
        }

        private static bool TaskRequiresBackgroundAccess(string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterTask()
        {
            bool taskRegister = false;
            string myRegister = "MyTimeTask2";
            string toastBackgroundTask = "toastTask";
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                task.Value.Unregister(true);
            }
            if (taskRegister == false)
            {
                RegisterBackgroundTask(taskEntryPoint: "Tasks.NotificationBackground",
                    name: myRegister,
                    trigger: new SystemTrigger(SystemTriggerType.NetworkStateChange, false),
                    condition: null,
                    completeHandler: Task_Completed
                    );
                RegisterBackgroundTask(taskEntryPoint: "Tasks.ToastBackgroundTask",
                  name: toastBackgroundTask,
                  trigger: new ToastNotificationActionTrigger(),
                  condition: null,
                  completeHandler: ToastTask_Completed
                  );
            }
        }
        private void ToastTask_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {

        }
        private void Task_Progress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            Debug.WriteLine("my task " + args.Progress + "%" + "downloaded!");
        }

        private void Task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            Debug.WriteLine("my task complete");
            internetAviable = internetAviable == true ? false : true;
            UpdateUI();
            if (internetAviable == true)
            {
                Common.Notifications.SendToastMessage("Tips!", "Connected to fareast domain successfully!");
            }
            else
            {
                Common.Notifications.SendToastMessage("Warning!", "Please join to fareast domain to get all threads!");
                Common.Notifications.UpdateTile("please join fareast domain!");
            }
        }
        public async void UpdateUI()
        {
            if (internetAviable == false)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    csdnThreads.Clear();
                    warningText.Visibility = Visibility;
                });
            }
            else
            {
                string x = await Common.HttpHelper.HttpClientGetThreads(StartDate.ToString(), EndDate.ToString());
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    csdnThreads = JsonConvert.DeserializeObject<ObservableCollection<ThreadsDetail>>(x);
                    viewThreads.ItemsSource = csdnThreads;
                    warningText.Visibility = Visibility.Collapsed;
                });
            }
        }
        private void updateTitle_Click(object sender, RoutedEventArgs e)
        {
            // ToastNotification 
            //  string badgeXmlString = "<badge value='" + textBox.Text + "'/>";
            Common.Notifications.UpdateTile("please join fareast domain!");
        }

        private void sendToast_Click(object sender, RoutedEventArgs e)
        {
            Common.Notifications.SendToastWithActionNotification(Common.NotificationXML.ToastWithActionXML, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            base.OnNavigatedTo(e);
        }

        private void viewThreads_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("item clicked!");

        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            offtopics = new Dictionary<string, string>();
            DateTimeOffset QueryStart = (DateTimeOffset)startDate.Date;
            StartDate = QueryStart.DateTime;
            DateTimeOffset QueryEnd = (DateTimeOffset)endDate.Date;
            EndDate = QueryEnd.DateTime;
            string x = await Common.HttpHelper.HttpClientGetThreads(StartDate.ToString(), EndDate.ToString());
            csdnThreads = JsonConvert.DeserializeObject<ObservableCollection<ThreadsDetail>>(x);
            viewThreads.ItemsSource = csdnThreads;
            Common.Notifications.UpdateTile("Total " + csdnThreads.Count ?? 0 + "threads!");
            foreach (var item in csdnThreads)
            {
                if (item.CSSAction == 9)
                {
                    id = item.Id.ToString();
                    offtopics[id] = item.ThreadTitle;              
                }

            }
            NextOffTopicToast(); 

        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void viewThreads_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            var heigh = bounds.Height - 130 - 80;
            viewThreads.Height = heigh;
        }
        private string id { get; set; }
        private async void Toast_Activated(ToastNotification sender, object args)
        {


            if (((Windows.UI.Notifications.ToastActivatedEventArgs)args).Arguments == "offtopic")
            {
                ThreadsDetail item = csdnThreads.Where(c => c.Id.ToString() == id).FirstOrDefault();

                if (internetAviable == false)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        csdnThreads.Clear();
                        warningText.Visibility = Visibility;
                    });
                }
                else
                {
                    //string x = await Common.HttpHelper.HttpClientGetThreads(StartDate.ToString(), EndDate.ToString());
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        csdnThreads.Remove(item);
                        //csdnThreads = JsonConvert.DeserializeObject<ObservableCollection<ThreadsDetail>>(x);
                        viewThreads.ItemsSource = csdnThreads;
                        warningText.Visibility = Visibility.Collapsed;
                    });
                }
            }

            NextOffTopicToast();


            //throw new NotImplementedException();
        }

        public void NextOffTopicToast()
        {
            if (offtopics.Count > 0)
            {
                TypedEventHandler<ToastNotification, System.Object> toastActived = Toast_Activated;
                Common.Notifications.SendToastWithActionNotification(
                    string.Format(Common.NotificationXML.offtopic_ToastActionXML,
                    "Confirm:",
                    offtopics.First().Value + "---is a offtopic thread.",
                    offtopics.Count == 1 ? "Ignore" : "Next"
                    ), Toast_Activated);
                id = offtopics.First().Key;
                offtopics.Remove(id);
            }
        }
    }
}
