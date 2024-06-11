using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace AstroApp.UI.Controls;

public partial class EditDayControl : ContentView, INotifyPropertyChanged
{
    private bool isActive;

    public bool IsActive
    {
        get { return isActive; }
        set
        {
            if (isActive != value)
            {
                isActive = value;
                OnPropertyChanged(nameof(IsActive));

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

    private void MoonDayTitle_Tapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is EditDayControl editDayControl)
        {
            if (editDayControl.DayAstroEvent.MoonPhase >= 4)
            {
                editDayControl.DayAstroEvent.MoonPhase = 1;
            }
            else
            {
                editDayControl.DayAstroEvent.MoonPhase += 1;
            }
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

    private void ActivityIcon_Tapped(object sender, EventArgs e)
    {
        var tappedElement = sender as Image;
        if (tappedElement?.BindingContext is EditDayControl editDayControl && e is TappedEventArgs tappedEventArgs)
        {
            var activity = tappedEventArgs.Parameter as string;
            if (activity != null)
            {
                switch (activity)
                {
                    case "Barber":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Barber,
                            (quality) => editDayControl.DayAstroEvent.Barber = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Barber));
                        break;
                    case "Beauty":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Beauty,
                            (quality) => editDayControl.DayAstroEvent.Beauty = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Beauty));
                        break;
                    case "Buystuff":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Buystuff,
                            (quality) => editDayControl.DayAstroEvent.Buystuff = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Buystuff));
                        break;
                    case "Contracts":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Contracts,
                            (quality) => editDayControl.DayAstroEvent.Contracts = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Contracts));
                        break;
                    case "ImportantTasks":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.ImportantTasks,
                            (quality) => editDayControl.DayAstroEvent.ImportantTasks = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.ImportantTasks));
                        break;
                    case "Gardening":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Gardening,
                            (quality) => editDayControl.DayAstroEvent.Gardening = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Gardening));
                        break;
                    case "Love":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Love,
                            (quality) => editDayControl.DayAstroEvent.Love = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Love));
                        break;
                    case "Meetings":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Meetings,
                            (quality) => editDayControl.DayAstroEvent.Meetings = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Meetings));
                        break;
                    case "NewIdeas":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.NewIdeas,
                            (quality) => editDayControl.DayAstroEvent.NewIdeas = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.NewIdeas));
                        break;
                    case "Tech":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Tech,
                            (quality) => editDayControl.DayAstroEvent.Tech = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Tech));
                        break;
                    case "Travel":
                        CycleActivityQuality(
                            () => editDayControl.DayAstroEvent.Travel,
                            (quality) => editDayControl.DayAstroEvent.Travel = quality
                        );
                        OnPropertyChanged(nameof(editDayControl.DayAstroEvent.Travel));
                        break;
                    default:
                        Debug.WriteLine("Unknown activity: " + activity);
                        break;
                }
            }
        }
        else
        {
            Debug.WriteLine("DayAstroEvent is null or BindingContext is not set correctly");
        }
    }
}