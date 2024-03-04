using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    public class MoonPhaseIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && Enum.IsDefined(typeof(MoonPhaseSymbol), intValue))
            {
                return Enum.GetName(typeof(MoonPhaseSymbol), intValue);
            }
            return "None"; // Or handle unexpected values differently
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && Enum.TryParse(stringValue, out MoonPhaseSymbol result))
            {
                return (int)result;
            }
            return 0; // Default value or handle the error as needed
        }
    }
}
