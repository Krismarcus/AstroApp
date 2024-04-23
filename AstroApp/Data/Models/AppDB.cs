
using System.Collections.ObjectModel;

namespace AstroApp.Data.Models
{
    public class AppDB
    {
        public ObservableCollection<AstroEvent> AstroEventsDB { get; set; }
        public ObservableCollection<PlanetInZodiac> PlanetInZodiacsDB { get; set; }
        public ObservableCollection<PlanetInRetrogradeDetails> PlanetInRetrogradeDetailsDB { get; set; }
        public ObservableCollection<MoonDay> MoonDaysDB { get; set; }
    }
}
