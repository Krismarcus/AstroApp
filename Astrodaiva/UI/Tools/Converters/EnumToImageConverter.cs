using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools.Converters
{
    public class EnumToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                string enumName = enumValue.ToString();
                string imagePath = $"{enumName.ToLower()}.png"; // Assuming lowercase enum names and PNG images								
                return imagePath;

                //// Modify this logic to return the appropriate image file name based on the enum value
                //switch (enumValue)

                //{
                //	case ZodiacSign.Aries:
                //		return "aries.png"; // Replace with the actual image file name

                //	case ZodiacSign.Taurus:
                //		return "taurus.png"; // Replace with the actual image file name
                //							 // 											
                //	default:

                //		return "default.png"; // Default image if no match is found
                //}
            }

            return null; // Return null if the value is not valid
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This is One Way Conversion");
        }
    }
}
