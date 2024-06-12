using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Pages;

public partial class PlanetInZodiacEventsEditPage : ContentPage
{
    public ObservableCollection<PlanetInZodiacDetails> PlanetInZodiacsDetails { get; set; }

    public ObservableCollection<PlanetInRetrogradeDetails> PlanetInRetrogradeDetails { get; set; }

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
        PopulatePlanetsInRetrogradeDetailsList();
    }

    private void PopulatePlanetsInZodiacsDetailsList()
    {
        this.PlanetInZodiacsDetails = App.AppData.AppDB.PlanetInZodiacsDB;
        if (this.PlanetInZodiacsDetails == null)
        {
            this.PlanetInZodiacsDetails = new ObservableCollection<PlanetInZodiacDetails>();

            // Iterate over each value in the Planet enum
            foreach (Planet planet in Enum.GetValues(typeof(Planet)))
            {
                // Iterate over each value in the ZodiacSign enum
                foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
                {
                    // Create a new PlanetInZodiac and add it to the collection
                    this.PlanetInZodiacsDetails.Add(new PlanetInZodiacDetails
                    {
                        Planet = planet,
                        ZodiacSign = zodiacSign,                        
                    });
                }
            }
        }

        for (int i = 0; i < PlanetInZodiacsDetails.Count; i++)
        {
            EditPlanetInZodiacDetailsControl editPlanetInZodiacDetailsControl = new EditPlanetInZodiacDetailsControl();

            PlanetInZodiacDetails planetInZodiac = PlanetInZodiacsDetails[i];
            if (planetInZodiac != null)
            {
                editPlanetInZodiacDetailsControl.AddPlanetInZodiacDetails(planetInZodiac);
            }

            this.EventList.Add(editPlanetInZodiacDetailsControl);
        }
    }

    private void PopulatePlanetsInRetrogradeDetailsList()
    {
        this.PlanetInRetrogradeDetails = App.AppData.AppDB.PlanetInRetrogradeDetailsDB;
        if (this.PlanetInRetrogradeDetails == null)
        {
            this.PlanetInRetrogradeDetails = new ObservableCollection<PlanetInRetrogradeDetails>();

            // Iterate over each value in the Planet enum
            foreach (Planet planet in Enum.GetValues(typeof(Planet)))
            {
                this.PlanetInRetrogradeDetails.Add(new PlanetInRetrogradeDetails
                {
                    PlanetInRetrograde = planet,
                    PlanetInRetrogradeInfo = ""
                });

            }
        }

        for (int i = 0; i < PlanetInRetrogradeDetails.Count; i++)
        {

            EditPlanetInRetrogradeDetailsControl editPlanetInRetrogradeControl = new EditPlanetInRetrogradeDetailsControl();
            PlanetInRetrogradeDetails planetInRetrograde = PlanetInRetrogradeDetails[i];
            if (planetInRetrograde != null)
            {
                editPlanetInRetrogradeControl.AddPlanetInRetrograde(planetInRetrograde);
            }
            this.RetrogradeList.Add(editPlanetInRetrogradeControl);

        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var appActions = new Services.AppActions();
        await appActions.SavePlanetinZodiacsAsync(PlanetInZodiacsDetails);
        await appActions.SavePlanetInRetrogradeAsync(PlanetInRetrogradeDetails);
        await Application.Current.MainPage.DisplayAlert("Success", "Planets in Zodiacs saved succesfully", "OK");
    }
}