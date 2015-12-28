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
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPStart.Tool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CopyPast : Page
    {
        public CopyPast()
        {
            this.InitializeComponent();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage data = new DataPackage();
            data.SetText(tbInput.Text);
            Clipboard.SetContent(data);

        }

        private async void Past_Click(object sender, RoutedEventArgs e)
        {
            DataPackageView dataview=  Clipboard.GetContent();
            if (dataview.Contains(StandardDataFormats.Text))
            {
                try {
                    tbInfor.Text= await dataview.GetTextAsync();
                }
                catch { }
            }
            this.Frame.Navigate(typeof(MainPage));
          
        }
    }
}
