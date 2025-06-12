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
    internal class NewMoonDayToLightBackgroundColorConverter : IValueConverter
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
                    return ColorManager.GetResourceColor("GreyBackground", Colors.Black);
                }

                else if (astroEvent.MoonDay.NewMoonDay == 1 || astroEvent.MoonDay.MiddleMoonDay == 1)
                {
                    return ColorManager.GetResourceColor("ShadedBackground", Colors.Black);
                }
            }

            return ColorManager.GetResourceColor("SecondaryBackground", Colors.Black);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Converting from Color to boolean is not supported.");
        }
    }
}
