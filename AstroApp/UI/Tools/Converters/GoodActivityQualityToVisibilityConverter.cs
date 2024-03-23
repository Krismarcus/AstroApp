using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    public class GoodActivityQualityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Assuming the value is of type ActivityQuality
            var activityQuality = (ActivityQuality)value;
                        
            return activityQuality == ActivityQuality.Good;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
