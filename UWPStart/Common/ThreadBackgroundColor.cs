using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPStart.Common
{
    public class ThreadBackgroundColor : IValueConverter
    {
        public System.Object Convert(System.Object value, Type targetType, System.Object parameter, System.String language)
        {
            string color = "";
            switch ((int)value)
            {
                case 1:
                    color = "Green";
                    break;
                case 5:
                    color = "Grey";
                    break;
                default:
                    color = "Beige";
                    break;
            }

            return color;
        }
        public System.Object ConvertBack(System.Object value, Type targetType, System.Object parameter, System.String language)
        {
            return new object();
        }
    }
}
