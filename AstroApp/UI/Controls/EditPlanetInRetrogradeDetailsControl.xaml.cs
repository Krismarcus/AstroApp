using AstroApp.Data.Models;

namespace AstroApp.UI.Controls;

public partial class EditPlanetInRetrogradeDetailsControl : ContentView
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

    public EditPlanetInRetrogradeDetailsControl()
	{
		InitializeComponent();
        BindingContext = this;
	}

    internal void AddPlanetInRetrograde(PlanetInRetrogradeDetails planetInRetrograde)
    {
        this.PlanetInRetrograde = planetInRetrograde;
    }
}