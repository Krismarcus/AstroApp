using Astrodaiva.Data.Models;
using Astrodaiva.UI.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools.Converters
{
    class DayBorderBackgroundColorToEclipseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Expecting the value to be an AstroEvent object
            if (value is AstroEvent astroEvent)
            {
                // Check for either SunEclipse or MoonEclipse being true
                if (astroEvent.SunEclipse || astroEvent.MoonEclipse)
                {
                    // Return a specific color if any eclipse is present
                    return Color.FromRgb(4, 39, 77); // Example color
                }

                else if (astroEvent.MoonDay.NewMoonDay == 1 || astroEvent.MoonDay.MiddleMoonDay == 1)
                {
                    return ColorManager.GetResourceColor("LightOrangeBackground", Colors.Black);
                }
            }

            // Attempt to retrieve the 'PrimaryBackground' color from resources if no eclipse
            if (Application.Current.Resources.TryGetValue("PrimaryBackground", out var colorValue) && colorValue is Color myColor)
            {
                return myColor; // Return the color from resources
            }

            // Return a fallback color if no condition is met
            return Colors.Transparent; // Fallback color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Converting from Color to boolean is not supported.");
        }
    }
}
