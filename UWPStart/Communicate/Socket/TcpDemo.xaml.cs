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



using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Graphics.Imaging;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPStart.Communicate.Socket
{
    public sealed partial class TcpDemo : Page
    {
        /// <summary>
        /// 服务端 socket
        /// </summary>
        private StreamSocketListener _listener;

        /// <summary>
        /// 客户端 socket
        /// </summary>
        private StreamSocket _client;

        /// <summary>
        /// 客户端向服务端发送数据时的 DataWriter
        /// </summary>
        private DataWriter _writer;

        public TcpDemo()
        {
            this.InitializeComponent();

            WriteBitmap();
            Exe();

        }
        public async void Exe()
        {
            await base64stringToDib("1");
        }
        internal async Task<string> base64stringToDib(string value)
        {
            try
            {
                if (value == null)
                    return "IntPtr.Zero1";
                value = "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";

                var buffer = System.Convert.FromBase64String(value);
                try
                {
                    //await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    //    {
                    using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                    {

                        using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                        {
                            writer.WriteBytes(buffer);
                            await writer.StoreAsync();
                            var image = new BitmapImage();
                            image.SetSource(ms);
                        }
                    }
                    //});
                    return "iRet";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async void WriteBitmap()
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            scaleFactor = 2.5;
            int widthSize = (int)(3100 / scaleFactor);
            int heightSize = (int)(1020 / scaleFactor);
            await renderTargetBitmap.RenderAsync(imageInnerGrid, widthSize/10, heightSize/10);

            var buffer = await renderTargetBitmap.GetPixelsAsync();
            byte[] pixels = buffer.ToArray();
            var writeableBitmap = new WriteableBitmap(renderTargetBitmap.PixelWidth, renderTargetBitmap.PixelHeight);
            //writeableBitmap.rez
            using (Stream stream = writeableBitmap.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(pixels, 0, pixels.Length);

                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile saveFile = await applicationFolder.CreateFileAsync("snapshot22.png", CreationCollisionOption.OpenIfExists);
                // 把图片的二进制数据写入文件存储
                using (var fileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        buffer.ToArray());
                    await encoder.FlushAsync();
                }

            }

        }
        // 在服务端启动一个 socket 监听
        private async void btnStartListener_Click(object sender, RoutedEventArgs e)
        {
            // 实例化一个 socket 监听对象
            _listener = new StreamSocketListener();
            // 监听在接收到一个连接后所触发的事件
            _listener.ConnectionReceived += _listener_ConnectionReceived;

            try
            {
                // 在指定的端口上启动 socket 监听
                Random random = new Random();
                await _listener.BindServiceNameAsync(random.Next(100,200).ToString());

                lblMsg.Text += "已经在本机的 2211 端口启动了 socket(tcp) 监听";
                lblMsg.Text += Environment.NewLine;

            }
            catch (Exception ex)
            {
                SocketErrorStatus errStatus = SocketError.GetStatus(ex.HResult);

                lblMsg.Text += "errStatus: " + errStatus.ToString();
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }

        // socket 监听接收到一个连接后
        async void _listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            // 实例化一个 DataReader，用于读取数据
            DataReader reader = new DataReader(args.Socket.InputStream);

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lblMsg.Text += "服务端收到了来自: " + args.Socket.Information.RemoteHostName.RawName + ":" + args.Socket.Information.RemotePort + " 的 socket 连接";
                lblMsg.Text += Environment.NewLine;
            });

            try
            {
                while (true)
                {
                    // 自定义协议（header|body）：前4个字节代表实际数据的长度，之后的实际数据为一个字符串数据

                    // 读取 header
                    uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                    if (sizeFieldCount != sizeof(uint))
                    {
                        // 在获取到合法数据之前，socket 关闭了
                        return;
                    }

                    // 读取 body
                    uint stringLength = reader.ReadUInt32();
                    uint actualStringLength = await reader.LoadAsync(stringLength);
                    if (stringLength != actualStringLength)
                    {
                        // 在获取到合法数据之前，socket 关闭了
                        return;
                    }

                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        // 显示客户端发送过来的数据
                        lblMsg.Text += "接收到数据: " + reader.ReadString(actualStringLength);
                        lblMsg.Text += Environment.NewLine;
                    });
                }
            }
            catch (Exception ex)
            {
                var ignore = this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    SocketErrorStatus errStatus = SocketError.GetStatus(ex.HResult);

                    lblMsg.Text += "errStatus: " + errStatus.ToString();
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += ex.ToString();
                    lblMsg.Text += Environment.NewLine;
                });
            }
        }

        // 创建一个客户端 socket，并连接到服务端 socket
        private async void btnConnectListener_Click(object sender, RoutedEventArgs e)
        {
            HostName hostName;
            try
            {
                hostName = new HostName("127.0.0.1");
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;

                return;
            }

            // 实例化一个客户端 socket 对象
            _client = new StreamSocket();

            try
            {
                // 连接到指定的服务端 socket
                await _client.ConnectAsync(hostName, "2211");

                lblMsg.Text += "已经连接上了 127.0.0.1:2211";
                lblMsg.Text += Environment.NewLine;
            }
            catch (Exception ex)
            {
                SocketErrorStatus errStatus = SocketError.GetStatus(ex.HResult);

                lblMsg.Text += "errStatus: " + errStatus.ToString();
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }

        // 从客户端 socket 发送一个字符串数据到服务端 socket
        private async void btnSendData_Click(object sender, RoutedEventArgs e)
        {
            // 实例化一个 DataWriter，用于发送数据
            if (_writer == null)
                _writer = new DataWriter(_client.OutputStream);

            // 自定义协议（header|body）：前4个字节代表实际数据的长度，之后的实际数据为一个字符串数据

            string data = "hello webabcd " + DateTime.Now.ToString("hh:mm:ss");
            _writer.WriteUInt32(_writer.MeasureString(data)); // 写 header 数据
            _writer.WriteString(data); // 写 body 数据

            try
            {
                // 发送数据
                await _writer.StoreAsync();

                lblMsg.Text += "数据已发送: " + data;
                lblMsg.Text += Environment.NewLine;
            }
            catch (Exception ex)
            {
                SocketErrorStatus errStatus = SocketError.GetStatus(ex.HResult);

                lblMsg.Text += "errStatus: " + errStatus.ToString();
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }

        // 关闭客户端 socket 和服务端 socket
        private void btnCloseSocket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // _writer.DetachStream(); // 分离 DataWriter 与 Stream 的关联
              //  _writer.Dispose();

               // _client.Dispose();
                _listener.Dispose();

                lblMsg.Text += "服务端 socket 和客户端 socket 都关闭了";
                lblMsg.Text += Environment.NewLine;
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }
    }
}