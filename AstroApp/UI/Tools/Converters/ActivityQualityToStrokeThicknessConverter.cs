using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    public class ActivityQualityToStrokeThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ActivityQuality activityQuality)
            {
                switch (activityQuality)
                {
                    case ActivityQuality.Good:
                    case ActivityQuality.Bad:
                        return 3; // Example thickness for "Good" or "Bad"
                    default:
                        return 1; // Default thickness
                }
            }
            return 1; // Return a default value if the conversion fails
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
