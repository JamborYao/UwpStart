using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPStart.Common
{
    public class ValueConvert:IValueConverter
    {
        public System.Object Convert(System.Object value, Type targetType, System.Object parameter, System.String language)
        {
            return (int)((int)value+100);
        }
        public System.Object ConvertBack(System.Object value, Type targetType, System.Object parameter, System.String language)
        {
            return new object();
        }
    }
}
