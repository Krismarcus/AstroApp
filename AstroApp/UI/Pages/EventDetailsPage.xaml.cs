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
        BindingContext = DayAstroEvent;
    }

    private void UpdateDayEventInfoList()
    {
        if (DayAstroEvent?.PlanetInZodiacs == null || App.AppData.AppDB.PlanetInZodiacsDB == null)
            return;

        foreach (var planetInZodiac in DayAstroEvent.PlanetInZodiacs)
        {
            var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(
                p => p.Planet == planetInZodiac.Planet && p.ZodiacSign == planetInZodiac.ZodiacSign);

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
}