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
        [ObservableProperty]
        private ActivityQuality beauty;
        [ObservableProperty]
        private ActivityQuality buystuff;
        [ObservableProperty]
        private ActivityQuality contracts;
        [ObservableProperty]
        private ActivityQuality importantTasks;
        [ObservableProperty]
        private ActivityQuality gardening;
        [ObservableProperty]
        private ActivityQuality love;
        [ObservableProperty]
        private ActivityQuality meetings;
        [ObservableProperty]
        private ActivityQuality newIdeas;
        [ObservableProperty]
        private ActivityQuality tech;
        [ObservableProperty]
        private string eventText;
    }
}
