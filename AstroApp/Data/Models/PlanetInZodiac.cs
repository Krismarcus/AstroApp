using AstroApp.Data.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroApp.Data.Models
{
    public partial class PlanetInZodiac : ObservableObject
    {
        [ObservableProperty]
        private Planet planet;
        [ObservableProperty]
        private ZodiacSign zodiacSign;
        [ObservableProperty]
        private string planetInZodiacInfo;
        [ObservableProperty]
        private bool isRetrograde;
    }
}
