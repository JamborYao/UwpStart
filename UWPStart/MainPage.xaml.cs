using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using UWPStart.Model;
using UWPStart.ViewModels;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPStart
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ViewModel vm;
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new ViewModel();

            //MyHttpGet();

        }

        public async void MyHttpGet()
        {
            string url = "http://10.168.172.243:8080/ThreadsManagerService.svc/GetThreadsByNumber?num=1";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.UseDefaultCredentials = false;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.Credentials = new NetworkCredential("v-jayao", "Change!13", "fareast");

            var response = request.GetResponseAsync();
            await response;
            Stream stream = response.Result.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            httpContent.Text = content;
          

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            base.OnNavigatedTo(e);
            //vm.GetEngineers();
            //EngineersView.DataContext = vm.Engineers;   

        }
    }
}
