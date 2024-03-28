using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    public class ShadowColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
                return Colors.Transparent; // Fallback in case of incorrect binding.

            bool isProfileActivated = (bool)values[0];
            ActivityQuality activityProfile = (ActivityQuality)values[1];
            int newMoonDay = (int)values[2];

            if (!isProfileActivated)
            {
                return newMoonDay == 1 ? Colors.Blue : Colors.White;
            }
            else
            {
                switch (activityProfile)
                {
                    case ActivityQuality.Good:
                        return Colors.Green;
                    case ActivityQuality.Bad:
                        return Colors.Red;
                    default:
                        return Colors.Transparent; // Default case
                }
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
