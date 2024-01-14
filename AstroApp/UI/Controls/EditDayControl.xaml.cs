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
        foreach (ZodiacSign sign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.SunInZodiacPicker.Items.Add(sign.ToString());
            this.MoonInZodiacPicker.Items.Add(sign.ToString());
        }

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