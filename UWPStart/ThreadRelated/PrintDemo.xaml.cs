using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPStart.ThreadRelated
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrintDemo : Page
    {
        // PrintDocument - 用于提供打印功能
        private PrintDocument printDocument = null;

        // 打印预览页集合
        private List<FrameworkElement> previewPages = new List<FrameworkElement>();

        // 需要打印的页
        private FrameworkElement printPage;


        public PrintDemo()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 为打印做好准备工作
            RegisterForPrinting();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // 释放为打印所准备的资源
            UnregisterForPrinting();
        }

        private async void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            // 请求打印，即弹出打印设备列表。注：需要在这之前调用 PrintManager.GetForCurrentView();
            await PrintManager.ShowPrintUIAsync();
        }

        // 为打印做好准备工作
        private void RegisterForPrinting()
        {
            printDocument = new PrintDocument();
            // 当系统请求将打印内容显示在预览窗口时
            printDocument.Paginate += printDocument_Paginate;
            // 当系统请求将打印内容中的某一页显示在预览窗口时
            printDocument.GetPreviewPage += printDocument_GetPreviewPage;
            // 当系统请求将打印内容发送到打印机时
            printDocument.AddPages += printDocument_AddPages;

            // 单击 charm 中的“设备”后显示打印设备
            PrintManager printManager = PrintManager.GetForCurrentView();
            // 当出现打印请求时（在 PrintManager.GetForCurrentView() 之后单击“设备”弹出打印设备列表时，或通过 PrintManager.ShowPrintUIAsync() 弹出打印设备列表时）
            printManager.PrintTaskRequested += printManager_PrintTaskRequested;

            // 提供打印内容
            PreparedPrintContent();
        }

        // 释放为打印所准备的资源
        private void UnregisterForPrinting()
        {
            if (printDocument == null)
                return;

            printDocument.Paginate -= printDocument_Paginate;
            printDocument.GetPreviewPage -= printDocument_GetPreviewPage;
            printDocument.AddPages -= printDocument_AddPages;

            PrintManager printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested -= printManager_PrintTaskRequested;

            printingRoot.Children.Clear();
        }

        // 提供打印内容
        private void PreparedPrintContent()
        {
            if (printPage == null)
            {
                printPage = new ThreadRelated.Print();
                StackPanel header = (StackPanel)printPage.FindName("header");
                header.Visibility = Visibility.Visible;
            }

            // 向 printingRoot 添加一个打印内容，以便发送到打印机打印
            printingRoot.Children.Add(printPage);
            printingRoot.InvalidateMeasure();
            printingRoot.UpdateLayout();
        }

        // 当出现打印请求时，即弹出打印设备列表时
        private void printManager_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            // 创建一个打印任务
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("打印任务的标题", async sourceRequested => // 在打印设备列表中选择了某个打印机时
            {
                // 打印预览页中的选项
                PrintTaskOptionDetails optionDetails = PrintTaskOptionDetails.GetFromPrintTaskOptions(printTask.Options);
                IList<string> displayedOptions = optionDetails.DisplayedOptions;

                // 在打印预览页上添加系统内置支持的选项
                displayedOptions.Clear();
                displayedOptions.Add(StandardPrintTaskOptions.Copies);
                displayedOptions.Add(StandardPrintTaskOptions.Orientation);
                displayedOptions.Add(StandardPrintTaskOptions.MediaSize);
                displayedOptions.Add(StandardPrintTaskOptions.Collation);
                displayedOptions.Add(StandardPrintTaskOptions.Duplex);
                printTask.Options.MediaSize = PrintMediaSize.NorthAmericaLegal;

                // 在打印预览页上添加自定义选项
                PrintCustomItemListOptionDetails pageFormat = optionDetails.CreateItemListOption("CustomOption", "自定义选项");  // 第1个参数：optionId；第2个参数：displayName
                pageFormat.AddItem("item1", "选项1"); // 第1个参数：itemId；第2个参数：displayName
                pageFormat.AddItem("item2", "选项2");
                pageFormat.AddItem("item3", "选项3");
                displayedOptions.Add("CustomOption");
                optionDetails.OptionChanged += printDetailedOptions_OptionChanged; // 当打印预览页中的自定义选项的选中项发生变化时

                // 告诉打印任务需要打印的内容
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    sourceRequested.SetSource(printDocument.DocumentSource);
                });

                // 当打印任务完成时
                printTask.Completed += async (s, args) =>
                {
                    if (args.Completion == PrintTaskCompletion.Failed)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                        });
                    }
                };
            });
        }

        // 当打印预览页中的自定义选项的选中项发生变化时
        private async void printDetailedOptions_OptionChanged(PrintTaskOptionDetails sender, PrintTaskOptionChangedEventArgs args)
        {
            // 获取发生变化的自定义选项的 OptionId
            string optionId = args.OptionId as string;
            if (string.IsNullOrEmpty(optionId))
                return;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                MessageDialog messageDialog = new MessageDialog(optionId, "OptionId");
                await messageDialog.ShowAsync();

                // 获取打印预览页中的指定的自定义选项的选中项的 ItemId
                string itemId = sender.Options[optionId].Value as string;
                messageDialog = new MessageDialog(itemId, "ItemId");
                await messageDialog.ShowAsync();

                /*
                 * 此处需要根据用户的选择，来修改打印的内容，这里就不写了
                 */

                // 刷新预览页
                printDocument.InvalidatePreview();
            });
        }

        // 当系统请求将打印内容显示在预览窗口时
        private void printDocument_Paginate(object sender, PaginateEventArgs e)
        {
            previewPages.Clear(); // 打印预览页
            printingRoot.Children.Clear(); // 实际发送到打印机的内容

            RichTextBlockOverflow lastRichText;
            PrintTaskOptions printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);
            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            // 添加第一个打印预览页，以及更新 printingRoot 中的内容
            lastRichText = AddPreviewPageAndPrintPage(null, pageDescription);

            // 添加之后的全部打印预览页，以及更新 printingRoot 中的内容
            while (lastRichText.HasOverflowContent && lastRichText.Visibility == Visibility.Visible)
            {
                lastRichText = AddPreviewPageAndPrintPage(lastRichText, pageDescription);
            }

            // 设置打印页数
            printDocument.SetPreviewPageCount(previewPages.Count, PreviewPageCountType.Intermediate);
        }

        // 当系统请求将打印内容中的某一页显示在预览窗口时
        private void printDocument_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            // 指定当前的打印预览页
            printDocument.SetPreviewPage(e.PageNumber, previewPages[e.PageNumber - 1]);
        }

        // 当系统请求将打印内容发送到打印机时
        private void printDocument_AddPages(object sender, AddPagesEventArgs e)
        {
            for (int i = 0; i < previewPages.Count; i++)
            {
                // 添加到打印列表
                printDocument.AddPage(previewPages[i]);
            }

            // 打印列表中的数据已经全部准备好了
            printDocument.AddPagesComplete();
        }

        // 向 previewPages 添加一个预览页，以及向 printingRoot 添加一个打印内容，同时返回本页所对应的下一页的 RichTextBlockOverflow
        private RichTextBlockOverflow AddPreviewPageAndPrintPage(RichTextBlockOverflow lastRichText, PrintPageDescription printPageDescription)
        {
            // 需要添加到 previewPages 的预览页
            FrameworkElement previewPage;

            // 添加到 previewPages 的打印页所对应的下一页的 RichTextBlockOverflow
            RichTextBlockOverflow textOverflow;

            if (lastRichText == null) // 第一页
            {
                previewPage = printPage;
                StackPanel footer = (StackPanel)previewPage.FindName("footer"); // 第一页不显示页脚
                footer.Visibility = Visibility.Collapsed;
            }
            else // 非第一页
            {
                previewPage = new ContinuationPage(lastRichText);
            }

            // 打印页的宽和高
            previewPage.Width = printPageDescription.PageSize.Width;
            previewPage.Height = printPageDescription.PageSize.Height;

            // 打印页的页边距
            Grid printableArea = (Grid)previewPage.FindName("printableArea");
            double applicationContentMarginLeft = 0.075; // 页的左右边距，即左右边距加在一起占 15% 的空间
            double applicationContentMarginTop = 0.03; // 页的上下边距，即上下边距加在一起占 6% 的空间
            double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * applicationContentMarginLeft * 2);
            double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * applicationContentMarginTop * 2);

            // 打印区域的宽和高
            printableArea.Width = printPage.Width - marginWidth;
            printableArea.Height = printPage.Height - marginHeight;

            // 向 printingRoot 添加一个打印内容，以便发送到打印机打印
            printingRoot.Children.Add(previewPage);
            printingRoot.InvalidateMeasure();
            printingRoot.UpdateLayout();

            // 获取本页所对应的下一页的 RichTextBlockOverflow
            textOverflow = (RichTextBlockOverflow)previewPage.FindName("textOverflow");

            if (!textOverflow.HasOverflowContent && textOverflow.Visibility == Visibility.Visible)
            {
                StackPanel footer = (StackPanel)previewPage.FindName("footer"); // 最后一页才显示页脚
                footer.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            // 向 previewPages 添加新页
            previewPages.Add(previewPage);

            return textOverflow;
        }
    }
}