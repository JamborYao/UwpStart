using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPStart.Tool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ParseCloud : Page
    {
        public ParseCloud()
        {
            this.InitializeComponent();
            UploadtoParse();
        }
        public async void ParseTest()
        {
            ParseClient.Initialize("oVFGM355Btjc1oETUvhz7AjvNbVJZXFD523abVig", "4FpFCQyO7YVmo2kMgrlymgDsshAvTnGAtQcy9NHl");
            ParseObject gameScore = new ParseObject("GameScore");
            gameScore["score"] = 1337;
            gameScore["playerName"] = "Sean Plott";
            await gameScore.SaveAsync();
        }

        public async void UploadtoParse()
        {

            ParseClient.Initialize("oVFGM355Btjc1oETUvhz7AjvNbVJZXFD523abVig", "4FpFCQyO7YVmo2kMgrlymgDsshAvTnGAtQcy9NHl");

            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".png");
            var pickedfile = await filePicker.PickSingleFileAsync();
            using (var randomStream = (await pickedfile.OpenReadAsync()))
            {
                using (var stream = randomStream.AsStream())
                {
                    //byte[] data = System.Text.Encoding.UTF8.GetBytes("Working at Parse is great!");
                    ParseFile file = new ParseFile("resume1.png", stream);

                    await file.SaveAsync();

                    var jobApplication = new ParseObject("JobApplication");
                    jobApplication["applicantName"] = "jambor";
                    jobApplication["applicantResumeFile"] = file;
                    await jobApplication.SaveAsync();

                }
            }

          

        }
    }
}
