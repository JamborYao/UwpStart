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
    public sealed partial class RelatedDesign : Page
    {
        public RelatedDesign()
        {
            this.InitializeComponent();
            this.left.ItemsSource = Enumerable.Range(1, 10).ToList();
        }
        private void full_back_Click(object sender, RoutedEventArgs e)
        {
            // 对于我们的页面来说，full_back按钮是应该隐藏的，因为没有上一层页面
            // 所以这里我们忽略掉，但是按钮还是留着，你可以自己来做个隐藏的逻辑
        }

        private void detail_back_Click(object sender, RoutedEventArgs e)
        {
            // 这里就是为什么我们在VisualState里重新设置属性的原因

            // 显示左侧的list
            // 隐藏右侧内容
            this.left.Visibility = Visibility.Visible;
            this.right.Visibility = Visibility.Collapsed;

            // 隐藏副标题
            // 显示主标题
            this.fullPanel.Visibility = Visibility.Visible;
            this.detailPanel.Visibility = Visibility.Collapsed;
        }

        private void left_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 这里我们需要判断下，是否需要切换隐藏
            if (this.ActualWidth <= 500)
            {
                // 显示左侧的list
                // 隐藏右侧内容
                this.left.Visibility = Visibility.Collapsed;
                this.right.Visibility = Visibility.Visible;

                // 隐藏副标题
                // 显示主标题
                this.fullPanel.Visibility = Visibility.Collapsed;
                this.detailPanel.Visibility = Visibility.Visible;
            }
        }
    }
}
