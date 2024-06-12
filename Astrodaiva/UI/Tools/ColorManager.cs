using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools
{
    internal class ColorManager
    {
        public static Color GetResourceColor(string resourceName, Color defaultColor)
        {
            if (Application.Current.Resources.TryGetValue(resourceName, out var colorValue) && colorValue is Color color)
            {
                return color;
            }
            return defaultColor;
        }
    }
}
