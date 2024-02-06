using AstroApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public class AstroEvent
    {
        public DateTime Date { get; set; }        
        public ObservableCollection<PlanetInZodiac> PlanetInZodiacs { get; set; } = new ObservableCollection<PlanetInZodiac>();
        public MoonDay MoonDay { get; set; }        
        public ObservableCollection<PlanetEvent> PlanetEvents { get; set; } = new ObservableCollection<PlanetEvent>();
        public bool SunEclipse { get; set; }
        public bool MoonEclipse { get; set; }
        public string EventText { get; set; }
    }
}
