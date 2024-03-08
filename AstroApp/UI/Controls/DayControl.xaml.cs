using AstroApp.Data.Models;
using AstroApp.UI.Pages;
using System.ComponentModel;

namespace AstroApp.UI.Controls;

public partial class DayControl : ContentView, INotifyPropertyChanged
{
    private int dayNumber;

    public int DayNumber
    {
        get { return dayNumber; }
        set
        {
            if (dayNumber != value)
            {
                dayNumber = value;
                OnPropertyChanged(nameof(DayNumber));

            }
        }
    }

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

    public DayControl()
    {
        InitializeComponent();        
        BindingContext = this;
        
    }

    public void AddAstroEvent(AstroEvent astroEventForDate)
    {
        this.DayAstroEvent = astroEventForDate;        
    }

    public void AddDayCardDayNumber(int dayNumber)
    {
        this.DayNumber = dayNumber;
    }

    private async void OnDayTapped(object sender, EventArgs e)
    {
        if (DayAstroEvent != null)
        {
            var eventDetailsPage = new EventDetailsPage();
            await eventDetailsPage.InitializeDataAsync(DayAstroEvent);
            await Navigation.PushModalAsync(eventDetailsPage);

        }
    }
}