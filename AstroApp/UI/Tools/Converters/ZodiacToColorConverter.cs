using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    public class ZodiacToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ZodiacSign))
                return null;

            var zodiac = (ZodiacSign)value;
            var colorKey = $"{zodiac}Color";
            if (Application.Current.Resources.TryGetValue(colorKey, out var color))
            {
                return color;
            }

            return Colors.Black; // Default color if not found
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Not needed
    }    
    }
}
