using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
                return value.ToString();

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string && targetType.IsEnum)
            {
                if (Enum.TryParse(targetType, (string)value, true, out object result))
                {
                    return result;
                }
            }
            throw new InvalidOperationException("Invalid conversion");
        }
    }
}

