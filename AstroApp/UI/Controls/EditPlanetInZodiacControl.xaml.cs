using AstroApp.Data.Models;

namespace AstroApp.UI.Controls;

public partial class EditPlanetInZodiacControl : ContentView
{
    private PlanetInZodiac planetInZodiac;

    public PlanetInZodiac PlanetInZodiac
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

    public EditPlanetInZodiacControl()
	{
		InitializeComponent();
        BindingContext = this;
    }

    internal void AddPlanetInZodiacDetails(PlanetInZodiac planetZodiac)
    {
        this.PlanetInZodiac = planetZodiac;
    }
}