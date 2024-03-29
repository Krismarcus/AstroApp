using AstroApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools.Converters
{
    class NewMoonDayToDarkTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Expecting the value to be an AstroEvent object
            if (value is AstroEvent astroEvent)
            {
                if (astroEvent.MoonDay.NewMoonDay == 1 || astroEvent.MoonDay.MiddleMoonDay == 1)
                {
                    if (Application.Current.Resources.TryGetValue("PrimaryBackground", out var colorValue) && colorValue is Color myColor)
                    {
                        return myColor; // Return the color from resources
                    }
                }
            }

            // Attempt to retrieve the 'PrimaryBackground' color from resources if no eclipse
            
            return Color.FromRgb(240, 201, 134);            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Converting from Color to boolean is not supported.");
        }
    }
}
