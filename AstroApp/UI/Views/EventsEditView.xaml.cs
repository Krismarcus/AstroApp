using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Views;

public partial class EventsEditView : ContentView
{    
    public ObservableCollection<PlanetInZodiac> PlanetInZodiacs { get; set; }
    public ObservableCollection<MoonDay> MoonDays { get; set; } 

    public EventsEditView()
	{
		InitializeComponent();        
        Initialize();        
        this.BindingContext = this;
    }

    public async void Initialize()
    {
        var appActions = new Services.AppActions();
        this.PlanetInZodiacs = await appActions.LoadPlanetInZodiacsDetailsAsync();
        //this.MoonDays = await appActions.LoadMoonDaysAsync();
        this.PopulateList();
    }

    private void PopulateList()
    {        
        PopulatePlanetsInZodiacsDetailsList();
    }

    private void PopulatePlanetsInZodiacsDetailsList()
    {        
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