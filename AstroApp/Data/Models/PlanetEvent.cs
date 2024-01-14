using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public class PlanetEvent
    {
        public Planet Planet1 { get; set; }
        public Planet Planet2 { get; set; }
        public AspectSymbol AspectSymbol { get; set; }
    }
}
