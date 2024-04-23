using AstroApp.Data.Models;

namespace AstroApp.UI.Controls;

public partial class EditPlanetInRetrogradeControl : ContentView
{
    private PlanetInRetrogradeDetails planetInRetrograde;

    public PlanetInRetrogradeDetails PlanetInRetrograde
    {
        get { return planetInRetrograde; }
        set
        {
            if (planetInRetrograde != value)
            {
                planetInRetrograde = value;
                OnPropertyChanged(nameof(PlanetInRetrograde));

            }
        }
    }

    public EditPlanetInRetrogradeControl()
	{
		InitializeComponent();
        BindingContext = this;
	}

    internal void AddPlanetInRetrograde(PlanetInRetrogradeDetails planetInRetrograde)
    {
        this.PlanetInRetrograde = planetInRetrograde;
    }
}