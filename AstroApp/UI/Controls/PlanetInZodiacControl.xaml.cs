using AstroApp.Data.Enums;

namespace AstroApp.UI.Controls;

public partial class PlanetInZodiacControl : ContentView
{
	public PlanetInZodiacControl()
	{
		InitializeComponent();
        this.PopulatePicker();
	}

    public void PopulatePicker()
    {
        foreach (ZodiacSign zodiac in Enum.GetValues(typeof(ZodiacSign)))
        {
            PlanetInZodiacPicker.Items.Add(zodiac.ToString());           
        }        
    }
}