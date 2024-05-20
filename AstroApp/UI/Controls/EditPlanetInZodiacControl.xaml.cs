using AstroApp.Data.Enums;
using AstroApp.Data.Models;

namespace AstroApp.UI.Controls
{
    public partial class EditPlanetInZodiacControl : ContentView
    {
        public EditPlanetInZodiacControl()
        {
            InitializeComponent();
            PopulatePicker();
        }

        private void PopulatePicker()
        {
            foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
            {
                ZodiacSignPicker.Items.Add(zodiacSign.ToString());
            }
        }

        private void OnLabelTapped(object sender, EventArgs e)
        {
            if (BindingContext is PlanetInZodiac planetInZodiac)
            {
                planetInZodiac.IsRetrograde = !planetInZodiac.IsRetrograde;
                OnPropertyChanged(nameof(planetInZodiac.IsRetrograde));
            }
        }
    }
}