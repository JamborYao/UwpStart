using Microsoft.WindowsAzure.Messaging;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWPStart
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    /// 
    sealed partial class App : Application
    {
       
        public static MobileServiceClient MobileService = new MobileServiceClient(
                "https://jamobile.azure-mobile.net/",
                "HbVzicpbRTzhPVxNHagxFvnkCIWcev26"
        );
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //  ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values["toastlog"] = "";
        }

      

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values["toastlog"] = "on launched event active!";
            InitNotificationsAsync();
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif
            var value = e.Arguments;
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                
                    rootFrame.Navigate(typeof(Pages.CSDNTool), e.Arguments);
                

            }
            if (value == "Pages2")
            {
                rootFrame.Navigate(typeof(Pages.Page2), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
        protected override void OnActivated(IActivatedEventArgs args)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values["toastlog"] = settings.Values["toastlog"]+"**************"+ "on Actived event active!";
            // logString += "on Actived event active!";
        }
            /// <summary>
            /// Invoked when Navigation to a certain page fails
            /// </summary>
            /// <param name="sender">The Frame which failed navigation</param>
            /// <param name="e">Details about the navigation failure</param>
            void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
           // settings.Values["toastlog"] = App.logString;
            deferral.Complete();
        }

        private async void InitNotificationsAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var rul = channel.Uri;



            Debugger.Break();
            var hub = new NotificationHub("uwpstart", "Endpoint=sb://jamobilehub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Lcai4r4yWy6SGWhqcfCjlhkRrK0RKL5bjM8HjD63KBA=");
            var tags = new string[2];            
            tags[0] = "MSDN";
            tags[1] = "movies";
            var result = await hub.RegisterNativeAsync(channel.Uri, tags);
           
            if (result.RegistrationId != null)
            {               
                Debug.WriteLine("Registration successful!");
            }

            channel.PushNotificationReceived += Channel_PushNotificationReceived;
        }

        private void Channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs e)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values["toastlog"] = settings.Values["toastlog"] + "**************" + "PushNotificationReceived event active!";
            //logString += "PushNotificationReceived event active!";
            String notificationContent = String.Empty;

           // var tag = e.ToastNotification.Tag;
            switch (e.NotificationType)
            {
                case PushNotificationType.Badge:
                    notificationContent = e.BadgeNotification.Content.GetXml();
                    break;

                case PushNotificationType.Tile:
                    notificationContent = e.TileNotification.Content.GetXml();
                    break;

                case PushNotificationType.Toast:
                    notificationContent = e.ToastNotification.Content.GetXml();
                    break;

                case PushNotificationType.Raw:
                    notificationContent = e.RawNotification.Content;
                    break;
            }

           // e.Cancel = true;

            Common.Notifications.UpdateTile("new threads coming2!");
        }

       
    }
}
