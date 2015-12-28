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


        }

        public async void WriteBitmap()
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            scaleFactor = 2.5;
            int widthSize = (int)(310 / scaleFactor);
            int heightSize = (int)(102 / scaleFactor);
            await renderTargetBitmap.RenderAsync(imageInnerGrid, widthSize, heightSize);

            var buffer = await renderTargetBitmap.GetPixelsAsync();
            byte[] pixels = buffer.ToArray();
            var writeableBitmap = new WriteableBitmap(renderTargetBitmap.PixelWidth, renderTargetBitmap.PixelHeight);
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