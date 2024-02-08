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
        BindingContext = this;
    }

    private void UpdateDayEventInfoList()
    {
        if (App.AppData.AppDB.PlanetInZodiacsDB == null || DayAstroEvent == null)
            return;

        UpdatePlanetInZodiacInfo(DayAstroEvent.SunInZodiac);
        UpdatePlanetInZodiacInfo(DayAstroEvent.MoonInZodiac);
        UpdatePlanetInZodiacInfo(DayAstroEvent.VenusInZodiac);
        UpdatePlanetInZodiacInfo(DayAstroEvent.MarsInZodiac);
        UpdatePlanetInZodiacInfo(DayAstroEvent.MercuryInZodiac);
        UpdateMoonDayInfo();
    }

    private void UpdatePlanetInZodiacInfo(PlanetInZodiac planetInZodiac)
    {
        if (planetInZodiac == null) return;

        var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == planetInZodiac.Planet && p.NewZodiacSign == planetInZodiac.NewZodiacSign);

        if (infoSourceItem != null)
        {
            planetInZodiac.PlanetInZodiacInfo = infoSourceItem.PlanetInZodiacInfo;
        }
    }

    private void UpdateMoonDayInfo()
    {
        if (DayAstroEvent?.MoonDay == null || App.AppData.AppDB.MoonDaysDB == null)
            return;

        var infoSourceItem = App.AppData.AppDB.MoonDaysDB.FirstOrDefault(m =>
            m.NewMoonDay == DayAstroEvent.MoonDay.NewMoonDay);

        if (infoSourceItem != null)
        {
            DayAstroEvent.MoonDay.MoonDayInfo = infoSourceItem.MoonDayInfo;
        }
    }

    private void TapNewMoonDay_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Details about " + (MoonDaySymbol)DayAstroEvent.MoonDay.NewMoonDay + " Day", DayAstroEvent.MoonDay.MoonDayInfo, "OK");
    }

    private void TapSunInZodiac_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Sun in " + DayAstroEvent.SunInZodiac.NewZodiacSign + " Details", DayAstroEvent.SunInZodiac.PlanetInZodiacInfo, "OK");
    }

    private void TapMoonInZodiac_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Moon in " + DayAstroEvent.MoonInZodiac.NewZodiacSign + " Details", DayAstroEvent.MoonInZodiac.PlanetInZodiacInfo, "OK");
    }


    private void TapVenusInZodiac_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Venus in " + DayAstroEvent.VenusInZodiac.NewZodiacSign + " Details", DayAstroEvent.VenusInZodiac.PlanetInZodiacInfo, "OK");
    }

    private void TapMarsInZodiac_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Mars in " + DayAstroEvent.MarsInZodiac.NewZodiacSign + " Details", DayAstroEvent.MarsInZodiac.PlanetInZodiacInfo, "OK");
    }

    private void TapMercuryInZodiac_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Mercury in " + DayAstroEvent.MercuryInZodiac.NewZodiacSign + " Details", DayAstroEvent.MercuryInZodiac.PlanetInZodiacInfo, "OK");
    }
}