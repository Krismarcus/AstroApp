using AstroApp.Data.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public partial class AstroEvent : ObservableObject
    {
        [ObservableProperty]
        private DateTime date;
        [ObservableProperty]
        private PlanetInZodiac sunInZodiac;
        [ObservableProperty]
        private PlanetInZodiac moonInZodiac;
        [ObservableProperty]
        private PlanetInZodiac venusInZodiac; 
        [ObservableProperty]
        private PlanetInZodiac marsInZodiac;
        [ObservableProperty]
        private PlanetInZodiac mercuryInZodiac;
        [ObservableProperty]
        private MoonDay moonDay;
        [ObservableProperty]
        private int moonPhase;
        public ObservableCollection<PlanetEvent> PlanetEvents { get; set; } = new ObservableCollection<PlanetEvent>();
        [ObservableProperty]
        private bool sunEclipse;
        [ObservableProperty]
        private bool moonEclipse;
        public ObservableCollection<ActivitySymbol> GoodDay {  get; set; } = new ObservableCollection<ActivitySymbol>();
        public ObservableCollection<ActivitySymbol> BadDay { get; set; } = new ObservableCollection<ActivitySymbol>();
        [ObservableProperty]
        private string eventText;
    }
}
