using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Pages;
using System.Collections.ObjectModel;
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

    private int calendarRow;
    public int CalendarRow
    {
        get { return calendarRow; }
        set
        {
            if (calendarRow != value)
            {
                calendarRow = value;
                OnPropertyChanged(nameof(CalendarRow));

            }
        }
    }

    private int calendarColumn;
    public int CalendarColumn
    {
        get { return calendarColumn; }
        set
        {
            if (calendarColumn != value)
            {
                calendarRow = value;
                OnPropertyChanged(nameof(CalendarColumn));

            }
        }
    }
    private AstroEvent dayAstroEvent;

    public AstroEvent DayAstroEvent
    {
        get => dayAstroEvent;
        set
        {
            if (dayAstroEvent != value)
            {
                dayAstroEvent = value;
                OnPropertyChanged(nameof(DayAstroEvent));
                UpdateDayCardColor(); // Trigger shadow color update
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
                UpdateDayCardColor(); // Trigger shadow color update
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
                UpdateDayCardColor(); // Trigger shadow color update
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

    public void OnOffProfile(bool isActivated)
    {
        this.IsProfileActivated = isActivated;
    }

    public void ChangeActivityProfile(string activityProfile)
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

            if (value != null)
            {                
                ActivityProfile = (ActivityQuality)value;
            }            
        }

        else
        {
            ActivityProfile = ActivityQuality.None;            
        }
    }

    public void LocateDayCardGrid(DateTime date)
    {
        // Get the first day of the month for the given date
        DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

        // Adjust .DayOfWeek to make Monday the first day of the week
        // .NET considers Sunday as 0, so we map Monday (1) to 0, ..., Sunday (0) to 6
        int startColumn = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

        // Calculate the row and column for the specific date
        // Subtract 1 from date.Day to make it zero-based, add startColumn, then divide and modulo by 7
        int dayIndex = date.Day - 1; // Zero-based index for the day of the month

        // Correct calculation for the column, ensuring it wraps correctly at the end of the week
        calendarColumn = (dayIndex + startColumn) % 7;

        // Adjusted calculation for the row to start from 1 instead of 0
        // We add startColumn to ensure we're taking into account where the first day of the month starts
        // The + 1 at the end ensures we start counting from row 1 instead of row 0
        calendarRow = (dayIndex + startColumn) / 7 + 1;

        // Notify that properties have changed
        OnPropertyChanged(nameof(CalendarRow));
        OnPropertyChanged(nameof(CalendarColumn));
    }

    private async void OnDayTapped(object sender, EventArgs e)
    {
        var border = sender as Border; // Cast sender to Border.
        if (border == null) return; // Safety check.

        // Scale the border to 1.1x size over 100 milliseconds.
        await border.ScaleTo(1.1, 100);
        // Then scale it back to original size over 100 milliseconds.
        await border.ScaleTo(1.0, 100);

        if (DayAstroEvent != null)
        {
            var eventDetailsPage = new EventDetailsPage();
            eventDetailsPage.InitializeAstroEventList();
            eventDetailsPage.InitializeDataAsync(DayAstroEvent.Date);            
            Navigation.PushModalAsync(eventDetailsPage);
            eventDetailsPage.UpdateDayEventInfoList();
        }
    }

    private void UpdateDayCardColor()
    {

        Color shadowColor = GetResourceColor("PrimaryLightText", Colors.Transparent);
        Color fontColor = GetResourceColor("PrimaryLightText", Colors.Transparent);

        if (DayAstroEvent != null)
        {
            if (!IsProfileActivated)
            {
                if (DayAstroEvent.MoonDay.NewMoonDay == 1)
                {
                    shadowColor = GetResourceColor("PrimaryBackground", Colors.Transparent);
                    fontColor = GetResourceColor("PrimaryBackground", Colors.Transparent);
                }

                else if (DayAstroEvent.MoonEclipse || DayAstroEvent.SunEclipse)
                {
                    shadowColor = GetResourceColor("PrimaryDarkText", Colors.Transparent);                    
                }
            }
            else
            {
                // Set fontColor based on activity profile
                fontColor = ActivityProfile switch
                {
                    ActivityQuality.Good => Colors.Green,
                    ActivityQuality.Bad => Colors.Red,
                    _ => GetResourceColor("PrimaryLightText", Colors.Transparent),
                };

                // Set shadowColor based on activity profile, default to transparent for cases not good or bad
                shadowColor = ActivityProfile switch
                {
                    ActivityQuality.Good => Colors.Green,
                    ActivityQuality.Bad => Colors.Red,
                    _ => Colors.Transparent, // Default to transparent if not good or bad
                };
            }
        }

        // Apply the shadow color
        ApplyShadowColor(shadowColor, fontColor);
    }

    private Color GetResourceColor(string resourceName, Color defaultColor)
    {
        if (Application.Current.Resources.TryGetValue(resourceName, out var colorValue) && colorValue is Color color)
        {
            return color;
        }
        return defaultColor;
    }

    private void ApplyShadowColor(Color shadowColor, Color fontColor)
    {
        dayLabel.TextColor = fontColor;
        // Assuming `dayCard` is the x:Name of your Border control
        dayCard.Shadow = new Shadow
        {
            Brush = new SolidColorBrush(shadowColor),
            Radius = 5,
            Opacity = 1,
            Offset = new Point(0, 5)
        };
    }
}