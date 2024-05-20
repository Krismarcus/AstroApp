using AstroApp.Data.Enums;

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
    }
}