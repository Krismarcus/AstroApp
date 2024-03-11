using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Pages;
using System.ComponentModel;
using System.Reflection;

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

    private ActivityQuality activityProfile;

    public ActivityQuality ActivityProfile
    {
        get => activityProfile;
        set
        {
            if (activityProfile != value)
            {
                activityProfile = value;
                OnPropertyChanged(nameof(ActivityProfile));
            }
        }
    }

    private bool isProfileActivated;

    public bool IsProfileActivated
    {
        get => isProfileActivated;
        set
        {
            if (isProfileActivated != value)
            {
                isProfileActivated = value;
                OnPropertyChanged(nameof(IsProfileActivated));
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

    internal void SetBorderColor(string activityProfile)
    {
        if (DayAstroEvent == null || string.IsNullOrWhiteSpace(activityProfile))
            return;
        
        // The type should be of DayAstroEvent, not activityProfile.
        Type targetType = DayAstroEvent.GetType();

        // Use targetType to find the property. The second parameter to GetProperty should be the property name, not the type.
        PropertyInfo propertyInfo = targetType.GetProperty(activityProfile, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo != null && propertyInfo.PropertyType == typeof(ActivityQuality))
        {
            // Get the value of the property from the DayAstroEvent object, not from activityProfile.
            object value = propertyInfo.GetValue(DayAstroEvent);

            // If the value is not null, cast it to ActivityQuality and set ActivityProfile.
            if (value != null)
            {
                isProfileActivated = true;
                ActivityProfile = (ActivityQuality)value;
            }
        }
    }
}