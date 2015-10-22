using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace UWPStart.ThreadRelated
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class ComboBoxEnum : Page
    {
        ObservableCollection<FontFamily> fonts = new ObservableCollection<FontFamily>();
        List<KeyValuePair<string, Enum>> FieldDataTypes = new List<KeyValuePair<string, Enum>>();
        List<KeyValuePair<string, string>> test = new List<KeyValuePair<string, string>>();
        

        public ComboBoxEnum()
        {
          
            this.InitializeComponent();
            fonts.Add(new FontFamily("Arial"));
            fonts.Add(new FontFamily("Courier New"));
            fonts.Add(new FontFamily("Times New Roman"));
            FieldDataTypes =  ToKeyValuePair(typeof(Week));
            test.Add(new KeyValuePair<string, string>("1", "test1"));
            test.Add(new KeyValuePair<string, string>("2", "test2"));
            DataType = "test2";

            // FontsCombo
          //  this.FontsCombo.ItemsSource = test;
        }
        public string DataType { get; set; }
        enum Week
        {
            Monday,
            Tuesday,
            Wednesday

        }
        public static List<KeyValuePair<String, Enum>> ToKeyValuePair(Type enumType)
        {
            if (enumType == null || !enumType.GetTypeInfo().IsEnum)
                return null;

            return Enum.GetValues(enumType)
                .Cast<Enum>()
                .Select(value => new KeyValuePair<String, Enum>(value.ToString()+"Text", value))
                .ToList();
        }


    }
    public class FieldDataType
    {
    }
    public class TextField
    {
        public string DataType { get; set; }
    }
}

