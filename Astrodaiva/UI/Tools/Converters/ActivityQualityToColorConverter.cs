using Astrodaiva.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools.Converters
{
    public class ActivityQualityToColorConverter : IValueConverter
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
                    return Color.FromRgb(255, 59, 106);
                case ActivityQuality.Neutral:
                    return Colors.Grey; // Or any color you deem appropriate for "Neutral"
                case ActivityQuality.None:
                    return Color.FromRgb(254, 234, 181);
                default:
                    return Colors.Transparent; // Fallback color
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Converting from Color to ActivityQuality is not supported.");
        }
    }
}
