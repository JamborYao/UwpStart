using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class silderbar : Page
    {
        public silderbar()
        {
            this.InitializeComponent();
            GenerateCode2();
        }
        public async void GenerateCode()
        {
            byte[] blackPixel = new byte[] { 0, 0, 0, 255 };
            byte[] whitePixel = new byte[] { 255, 255, 255, 255 };

            Random random = new Random();
            Stopwatch watch = new Stopwatch();

            var buffer = new Windows.Storage.Streams.Buffer(40000);
            using (var stream = buffer.AsStream())
            {
                watch.Start();
                for (var i = 0; i < 10000; i++)
                {
                    if (random.NextDouble() >= 0.5)
                        await stream.WriteAsync(blackPixel, 0, 4);
                    else
                        await stream.WriteAsync(whitePixel, 0, 4);
                }
                watch.Stop();
                await new MessageDialog(watch.ElapsedMilliseconds.ToString()).ShowAsync();
            }
        }
        public async void GenerateCode2()
        {
            byte[] blackPixel = new byte[] { 0, 0, 0, 255 };
            byte[] whitePixel = new byte[] { 255, 255, 255, 255 };

            Random random = new Random();
            Stopwatch watch = new Stopwatch();

            var buffer = new Windows.Storage.Streams.Buffer(40000);
            using (var stream = buffer.AsStream())
            {
                watch.Start();
                await Task.Run(() =>
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        if (random.NextDouble() >= 0.5)
                            stream.Write(blackPixel, 0, 4);
                        else
                            stream.Write(whitePixel, 0, 4);
                    }
                });
                watch.Stop();
                await new MessageDialog(watch.ElapsedMilliseconds.ToString()).ShowAsync();

            }
        }
    }
}
