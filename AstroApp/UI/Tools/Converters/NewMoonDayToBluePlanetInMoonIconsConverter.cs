using AstroApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    class NewMoonDayToBluePlanetInMoonIconsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is AstroEvent astroEvent)
            {
                if (astroEvent.MoonDay.NewMoonDay == 1 || astroEvent.MoonDay.MiddleMoonDay == 1)
                {
                    string enumName = astroEvent.MoonInZodiac.NewZodiacSign.ToString();
                    string imagePath = $"{enumName.ToLower()}_blue.png"; // Assuming lowercase enum names and PNG images
                    return imagePath;

                }

                else
                {
                    string enumName = astroEvent.MoonInZodiac.NewZodiacSign.ToString();
                    string imagePath = $"{enumName.ToLower()}.png"; // Assuming lowercase enum names and PNG images
                    return imagePath;
                }

            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
