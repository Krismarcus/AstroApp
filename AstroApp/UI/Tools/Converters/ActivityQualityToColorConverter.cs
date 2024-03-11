using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
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
                    return Colors.LightGreen;
                case ActivityQuality.Bad:
                    return Colors.IndianRed;
                case ActivityQuality.Neutral:
                    return Colors.Orange; // Or any color you deem appropriate for "Neutral"
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
