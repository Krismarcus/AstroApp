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
        DateTime astroDate = DayAstroEvent.Date;
        this.DayAstroEvent.PlanetInZodiacs.Single(p => p.Planet == Data.Enums.Planet.Sun).NewZodiacSign = SunZodiacSignYearlyCalendar(astroDate);
    }

    public ZodiacSign SunZodiacSignYearlyCalendar(DateTime date)
    {
        // Define the zodiac sign start dates
        var zodiacSignStartDates = new Dictionary<ZodiacSign, DateTime>
    {
        {ZodiacSign.Aquarius, new DateTime(date.Year, 1, 20)},
        {ZodiacSign.Pisces, new DateTime(date.Year, 2, 19)},
        {ZodiacSign.Aries, new DateTime(date.Year, 3, 21)},
        {ZodiacSign.Taurus, new DateTime(date.Year, 4, 20)},
        {ZodiacSign.Gemini, new DateTime(date.Year, 5, 21)},
        {ZodiacSign.Cancer, new DateTime(date.Year, 6, 21)},
        {ZodiacSign.Leo, new DateTime(date.Year, 7, 23)},
        {ZodiacSign.Virgo, new DateTime(date.Year, 8, 23)},
        {ZodiacSign.Libra, new DateTime(date.Year, 9, 23)},
        {ZodiacSign.Scorpio, new DateTime(date.Year, 10, 23)},
        {ZodiacSign.Sagittarius, new DateTime(date.Year, 11, 22)},
        // Handle Capricorn specially since it spans the new year
        {ZodiacSign.Capricorn, new DateTime(date.Year, 12, 22)}
    };

        // Adjust for Capricorn spanning the year end and start
        if (date.Month == 1 && date.Day <= 19)
        {
            return ZodiacSign.Capricorn; // For dates from January 1 to January 19
        }

        // Determine the zodiac sign based on the date
        foreach (var signStartDate in zodiacSignStartDates.OrderByDescending(kv => kv.Value))
        {
            if (date >= signStartDate.Value)
            {
                return signStartDate.Key;
            }
        }

        // Default to Capricorn if none found, handles dates after December 22
        return ZodiacSign.Capricorn;

    }

    public void PopulatePickers()
    {
        foreach (MoonDaySymbol moonDay in Enum.GetValues(typeof(MoonDaySymbol)))
        {
            this.NewMoonDayPicker.Items.Add(moonDay.ToString());
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
}