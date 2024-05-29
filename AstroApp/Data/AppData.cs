using AstroApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data
{
    public class AppData
    {
        public AppDB AppDB { get; set; }
        public CultureInfo CultureInfo { get; set; } = new CultureInfo("lt-LT");
    }
}
