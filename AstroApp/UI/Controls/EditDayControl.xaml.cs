using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.ComponentModel;

namespace AstroApp.UI.Controls;

public partial class EditDayControl : ContentView, INotifyPropertyChanged
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

    public EditDayControl()
    {
        InitializeComponent();
        PopulatePickers();
        BindingContext = this;
    }

    internal void AddAstroEvent(AstroEvent astroEventForDate)
    {
        this.DayAstroEvent = astroEventForDate;        
    }

    public void PopulatePickers()
    {
        foreach (MoonDaySymbol moonDay in Enum.GetValues(typeof(MoonDaySymbol)))
        {
            this.NewMoonDayPicker.Items.Add(moonDay.ToString());
        }

        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.SunInZodiacPicker.Items.Add(zodiacSign.ToString());
        }

        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.MoonInZodiacPicker.Items.Add(zodiacSign.ToString());
        }

        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.VenusInZodiacPicker.Items.Add(zodiacSign.ToString());
        }

        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.MarsInZodiacPicker.Items.Add(zodiacSign.ToString());
        }

        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.MercuryInZodiacPicker.Items.Add(zodiacSign.ToString());
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void AddEvent_Clicked(object sender, EventArgs e)
    {
        var newEvent = new PlanetEvent
        {
            Planet1 = Planet.Mars, // Default or user-selected value
            Planet2 = Planet.Venus, // Default or user-selected value
            AspectSymbol = AspectSymbol.Conjunction // Default or user-selected value
        };
        DayAstroEvent.PlanetEvents.Add(newEvent);
        OnPropertyChanged(nameof(DayAstroEvent)); // Notify UI about the change
    }

    private void RemoveEvent_Clicked(object sender, EventArgs e)
    {
        if (DayAstroEvent.PlanetEvents.Any())
        {
            // Get the last item
            var lastEvent = DayAstroEvent.PlanetEvents.LastOrDefault();

            // Remove the last item
            DayAstroEvent.PlanetEvents.Remove(lastEvent);

            // Notify the UI that the collection has changed
            OnPropertyChanged(nameof(DayAstroEvent));
        }
    }

    private void MercuryRetrograde_Tapped(object sender, TappedEventArgs e)
    {
        DayAstroEvent.MercuryInZodiac.IsRetrograde = !DayAstroEvent.MercuryInZodiac.IsRetrograde;
    }

    private void VenusRetrograde_Tapped(object sender, TappedEventArgs e)
    {
        DayAstroEvent.VenusInZodiac.IsRetrograde = !DayAstroEvent.VenusInZodiac.IsRetrograde;
    }

    private void MarsRetrograde_Tapped(object sender, TappedEventArgs e)
    {
        DayAstroEvent.MarsInZodiac.IsRetrograde = !DayAstroEvent.MarsInZodiac.IsRetrograde;
    }

    private void MoonDayTitle_Tapped(object sender, TappedEventArgs e)
    {        
        if (DayAstroEvent.MoonPhase >= 4)
        {
            DayAstroEvent.MoonPhase = 1;
        }
        else
        {            
            DayAstroEvent.MoonPhase += 1;
        }
    }

    private void CycleActivityQuality(Func<ActivityQuality> getQuality, Action<ActivityQuality> setQuality)
    {
        var currentQuality = getQuality();

        
        var nextQuality = (int)currentQuality + 1;

        
        if (nextQuality > (int)ActivityQuality.Bad)
        {
            nextQuality = (int)ActivityQuality.Neutral;
        }

        
        setQuality((ActivityQuality)nextQuality);
    }
    private void BarberIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Barber,
            (quality) => dayAstroEvent.Barber = quality
        );
    }

    private void BeautyIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Beauty, 
            (quality) => dayAstroEvent.Beauty = quality 
        );
    }    

    private void BuyStuffIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Buystuff, 
            (quality) => dayAstroEvent.Buystuff = quality 
        );
    }

    private void ContractsIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Contracts,
            (quality) => dayAstroEvent.Contracts = quality
        );
    }
    private void ImportantTasksIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.ImportantTasks,
            (quality) => dayAstroEvent.ImportantTasks = quality
        );
    }
    private void GardeningIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Gardening,
            (quality) => dayAstroEvent.Gardening = quality
        );
    }

    private void LoveIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Love,
            (quality) => dayAstroEvent.Love = quality
        );
    }

    private void MeetingsIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Meetings,
            (quality) => dayAstroEvent.Meetings = quality
        );
    }

    private void NewIdeasIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.NewIdeas,
            (quality) => dayAstroEvent.NewIdeas = quality
        );
    }

    private void TechIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Tech, 
            (quality) => dayAstroEvent.Tech = quality 
        );
    }

    private void TravelIcon_Tapped(object sender, EventArgs e)
    {
        CycleActivityQuality(
            () => dayAstroEvent.Travel,
            (quality) => dayAstroEvent.Travel = quality
        );
    }


}