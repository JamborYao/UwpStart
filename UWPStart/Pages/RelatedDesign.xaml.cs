using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class RelatedDesign : Page
    {
        public RelatedDesign()
        {
            this.InitializeComponent();
            this.left.ItemsSource = Enumerable.Range(1, 10).ToList();
         
        }
       
        private void full_back_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void detail_back_Click(object sender, RoutedEventArgs e)
        {
           
            this.left.Visibility = Visibility.Visible;
            this.right.Visibility = Visibility.Collapsed;
            
            this.fullPanel.Visibility = Visibility.Visible;
            this.detailPanel.Visibility = Visibility.Collapsed;
        }

        private void left_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActualWidth <= 500)
            {
                this.left.Visibility = Visibility.Collapsed;
                this.right.Visibility = Visibility.Visible;
                
                this.fullPanel.Visibility = Visibility.Collapsed;
                this.detailPanel.Visibility = Visibility.Visible;
            }
        }
    }
}
