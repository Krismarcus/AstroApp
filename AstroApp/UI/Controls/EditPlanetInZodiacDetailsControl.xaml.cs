using AstroApp.Data.Models;

namespace AstroApp.UI.Controls;

public partial class EditPlanetInZodiacDetailsControl : ContentView
{
    private PlanetInZodiacDetails planetInZodiac;

    public PlanetInZodiacDetails PlanetInZodiac
    {
        get { return planetInZodiac; }
        set
        {
            if (planetInZodiac != value)
            {
                planetInZodiac = value;
                OnPropertyChanged(nameof(PlanetInZodiac));

            }
        }
    }

    public EditPlanetInZodiacDetailsControl()
	{
		InitializeComponent();
        BindingContext = this;
    }

    internal void AddPlanetInZodiacDetails(PlanetInZodiacDetails planetZodiac)
    {
        this.PlanetInZodiac = planetZodiac;
    }
}