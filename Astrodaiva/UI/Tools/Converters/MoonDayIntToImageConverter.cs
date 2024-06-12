using Astrodaiva.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools.Converters
{
    public class MoonDayIntToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && Enum.IsDefined(typeof(MoonDaySymbol), intValue))
            {
                var enumName = Enum.GetName(typeof(MoonDaySymbol), intValue);
                return enumName?.ToLower() + ".png";
            }

            return null; // Or return null if you want no image in default case
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This is One Way Conversion");
        }
    }
}
