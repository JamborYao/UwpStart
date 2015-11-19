using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            this.InitializeComponent();
            List<string> lists = new List<string>();
            lists.Add("test1");
            lists.Add("my new test!");
            lists.Add("An empty page that can be used on its own or navigated to within a Frame.");
            myListView.ItemsSource = lists;

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView= SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += CurrentView_BackRequested;

            if (e.Parameter is string)
            {
                showText.Text = e.Parameter.ToString();
            }
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //  Frame.CanGoBack;
            currentView.BackRequested -= CurrentView_BackRequested;
            base.OnNavigatedFrom(e);
        }
        private void CurrentView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame.GoBack();
            //throw new NotImplementedException();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            badgeDOM.LoadXml(string.Format(Common.NotificationXML.ToastInternetXML, "test111", "test112"));
            ToastNotification toast = new ToastNotification(badgeDOM);
            toast.SuppressPopup = true;
            toast.Tag = "msdn";
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            toast.Activated += Toast_Activated;
        }

        private void Toast_Activated(ToastNotification sender, object args)
        {
            throw new NotImplementedException();
        }
    }
}
