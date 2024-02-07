using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.ComponentModel;

namespace AstroApp.UI.Pages;

public partial class EventDetailsPage : ContentPage
{
    private AstroEvent dayAstroEvent; 

    public AstroEvent DayAstroEvent
    {
        get { return dayAstroEvent; }
        set
        {
            if (dayAstroEvent != value)
            {
                dayAstroEvent = value;
                OnPropertyChanged(nameof(DayAstroEvent));

            }
        }
    }

    private PlanetInZodiac sunInZodiac;

    public PlanetInZodiac SunInZodiac
    {
        get { return sunInZodiac; }
        set
        {
            if (sunInZodiac != value)
            {
                sunInZodiac = value;
                OnPropertyChanged(nameof(SunInZodiac));

            }
        }
    }

    private PlanetInZodiac moonInZodiac;

    public PlanetInZodiac MoonInZodiac
    {
        get { return moonInZodiac; }
        set
        {
            if (moonInZodiac != value)
            {
                moonInZodiac = value;
                OnPropertyChanged(nameof(MoonInZodiac));

            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public EventDetailsPage()
    {        
        InitializeComponent();        
    }   

    private async void OnPageTapped(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync(); // Close the modal page
    }

    public async Task InitializeDataAsync(AstroEvent astroEvent)
    {        
        DayAstroEvent = astroEvent;
        UpdateDayEventInfoList();
        this.SunInZodiac = DayAstroEvent.PlanetInZodiacs.Single(i => i.Planet == Data.Enums.Planet.Sun);
        this.MoonInZodiac = DayAstroEvent.PlanetInZodiacs.Single(i => i.Planet == Data.Enums.Planet.Moon);
        BindingContext = this;
    }

    private void UpdateDayEventInfoList()
    {
        if (DayAstroEvent?.PlanetInZodiacs == null || App.AppData.AppDB.PlanetInZodiacsDB == null)
            return;

        foreach (var planetInZodiac in DayAstroEvent.PlanetInZodiacs)
        {
            var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(
                p => p.Planet == planetInZodiac.Planet && p.NewZodiacSign == planetInZodiac.NewZodiacSign);

            if (infoSourceItem != null)
            {
                planetInZodiac.PlanetInZodiacInfo = infoSourceItem.PlanetInZodiacInfo;
            }
        }

        if (DayAstroEvent?.MoonDay == null || App.AppData.AppDB.MoonDaysDB == null)
            return;

        else
        {
            var infoSourceItem = App.AppData.AppDB.MoonDaysDB.FirstOrDefault(
                 m => m.NewMoonDay == DayAstroEvent.MoonDay.NewMoonDay);

            if (infoSourceItem != null)
            {
                DayAstroEvent.MoonDay.MoonDayInfo = infoSourceItem.MoonDayInfo;
            }
        }
    }

    private void TapSunInZodiac_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Sun in " + SunInZodiac.NewZodiacSign + " Details", SunInZodiac.PlanetInZodiacInfo, "OK");
    }

    private void TapMoonInZodiac_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Moon in "+ MoonInZodiac.NewZodiacSign + " Details", MoonInZodiac.PlanetInZodiacInfo, "OK");
    }

    private void TapNewMoonDay_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Details about " + (MoonDaySymbol)DayAstroEvent.MoonDay.NewMoonDay + " Day", DayAstroEvent.MoonDay.MoonDayInfo, "OK");
    }   
}