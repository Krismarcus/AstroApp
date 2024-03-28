using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    public class ActivityQualityToFontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ActivityQuality quality))
                return Colors.Transparent; // Fallback color

            switch (quality)
            {
                case ActivityQuality.Good:
                    return Color.FromRgb(43, 255, 0);
                case ActivityQuality.Bad:
                    return Color.FromRgb(255, 0, 0);
                case ActivityQuality.Neutral:
                    return Color.FromRgb(254, 234, 181);
                case ActivityQuality.None:
                    return Color.FromRgb(254, 234, 181);
                default:
                    return Colors.Transparent; // Fallback color
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }        
    }
}
