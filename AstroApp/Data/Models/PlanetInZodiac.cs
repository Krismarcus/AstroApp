using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public class PlanetInZodiac
    {
        public Planet Planet { get; set; }
        public ZodiacSign ZodiacSign { get; set; }
        public string PlanetInZodiacInfo { get; set; }
        public bool IsRetrograde { get; set; }
    }
}
