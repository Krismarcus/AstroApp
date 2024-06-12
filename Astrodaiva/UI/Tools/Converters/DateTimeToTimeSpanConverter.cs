using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools.Converters
{
    public class DateTimeToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime)value;
            return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;
            var today = DateTime.Today; // Or use another relevant date
            return new DateTime(today.Year, today.Month, today.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
