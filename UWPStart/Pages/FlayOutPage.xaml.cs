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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPStart.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlayOutPage : Page
    {
        public FlayOutPage()
        {
            this.InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Flyout 继承自 FlyoutBase
            FlyoutBase flyout = btnDemo.Flyout;

            // FlyoutBase.Hide() - 隐藏 Flyout
            flyout.Hide();
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            // FlyoutBase.ShowAttachedFlyout(FrameworkElement flyoutOwner) - 在指定的 FrameworkElement 上显示 flyout
            FlyoutBase.ShowAttachedFlyout(lastText);

            // FlyoutBase.GetAttachedFlyout(FrameworkElement element) - 获取指定 FrameworkElement 上定义的 flyout
            //FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(element);

            // FlyoutBase.ShowAt(FrameworkElement placementTarget) - 在指定的 FrameworkElement 上显示指定的 flyout
             //flyout.ShowAt(btnDemo);
           
        }

        private void Flyout_Opening(object sender, object e)
        {
            lblMsg.Text = "Flyout 打开中";
        }

        private void Flyout_Opened(object sender, object e)
        {
            lblMsg.Text = "Flyout 已打开";
        }

        private void Flyout_Closed(object sender, object e)
        {
            lblMsg.Text = "Flyout 已关闭";
        }

    }
}
