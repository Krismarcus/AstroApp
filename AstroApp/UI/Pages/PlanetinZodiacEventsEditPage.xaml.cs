using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Pages;

public partial class PlanetInZodiacEventsEditPage : ContentPage
{
    public ObservableCollection<PlanetInZodiac> PlanetInZodiacs { get; set; }    

    public PlanetInZodiacEventsEditPage()
	{
		InitializeComponent();
        Initialize();
        this.BindingContext = this;
    }

    public async void Initialize()
    {
        //this.MoonDays = await appActions.LoadMoonDaysAsync();
        PopulatePlanetsInZodiacsDetailsList();
    }

    private void PopulatePlanetsInZodiacsDetailsList()
    {
        this.PlanetInZodiacs = App.AppData.AppDB.PlanetInZodiacsDB;
        if (this.PlanetInZodiacs == null)
        {
            this.PlanetInZodiacs = new ObservableCollection<PlanetInZodiac>();

            // Iterate over each value in the Planet enum
            foreach (Planet planet in Enum.GetValues(typeof(Planet)))
            {
                // Iterate over each value in the ZodiacSign enum
                foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
                {
                    // Create a new PlanetInZodiac and add it to the collection
                    this.PlanetInZodiacs.Add(new PlanetInZodiac
                    {
                        Planet = planet,
                        ZodiacSign = zodiacSign,
                        PlanetInZodiacInfo = "",
                    });
                }
            }
        }

        for (int i = 0; i < PlanetInZodiacs.Count; i++)
        {
            EditPlanetInZodiacControl editPlanetInZodiacControl = new EditPlanetInZodiacControl();

            PlanetInZodiac planetInZodiac = PlanetInZodiacs[i];
            if (planetInZodiac != null)
            {
                editPlanetInZodiacControl.AddPlanetInZodiacDetails(planetInZodiac);
            }

            this.EventList.Add(editPlanetInZodiacControl);
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var appActions = new Services.AppActions();
        appActions.SavePlanetinZodiacsAsync(PlanetInZodiacs);
        await Application.Current.MainPage.DisplayAlert("Success", "Planets in Zodiacs saved succesfully", "OK");
    }
}