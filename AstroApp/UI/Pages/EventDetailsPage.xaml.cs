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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(200); // Increase the delay slightly
        AnimateMarkerToPosition(dayAstroEvent.MoonDay);
    }

    private void AnimateMarkerToPosition(MoonDay moonDay)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (layout.Width <= 0) return; // Ensure the layout has been laid out

            // First Marker and Label Animation
            var totalMinutesInDay = 24 * 60 - 1;
            var transitionMinutes = moonDay.TransitionTime.Hour * 60 + moonDay.TransitionTime.Minute;
            var positionRatio = (double)transitionMinutes / totalMinutesInDay;
            var adjustedWidth = layout.Width - this.marker.WidthRequest;
            var targetPositionX = adjustedWidth * positionRatio;

            timeLabel.Text = moonDay.TransitionTime.ToString("HH:mm");
            timeLabel.TranslationX = targetPositionX - (timeLabel.Width / 2);
            timeLabel.TranslationY = -20; // Adjust based on your UI needs
            timeLabel.IsVisible = false; // Start with the label hidden

            uint animationDuration = 200; // Duration for animation

            await marker.TranslateTo(targetPositionX, 0, animationDuration, Easing.Linear);
            timeLabel.IsVisible = true;

            // Second Marker and Label Animation for Triple Moon Day
            if (moonDay.IsTripleMoonDay)
            {
                var middleTransitionMinutes = moonDay.MiddleMoonDayTransitionTime.Hour * 60 + moonDay.MiddleMoonDayTransitionTime.Minute;
                var middlePositionRatio = (double)middleTransitionMinutes / totalMinutesInDay;
                var middleTargetPositionX = adjustedWidth * middlePositionRatio;

                secondTimeLabel.Text = moonDay.MiddleMoonDayTransitionTime.ToString("HH:mm");
                secondTimeLabel.TranslationX = middleTargetPositionX - (secondTimeLabel.Width / 2);
                secondTimeLabel.TranslationY = -20; // Adjust based on your UI needs
                secondTimeLabel.IsVisible = false; // Initially hidden

                secondTimeLabel.IsVisible = true;
                await secondMarker.TranslateTo(middleTargetPositionX, 0, animationDuration, Easing.Linear);
                
            }
        });
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
            DayAstroEvent.MoonDay.NewMoonDayInfo = infoSourceItem.NewMoonDayInfo;
        }

        // Find and apply PreviousMoonDayInfo
        var previousMoonDayItem = App.AppData.AppDB.MoonDaysDB.FirstOrDefault(m =>
            m.NewMoonDay == DayAstroEvent.MoonDay.PreviousMoonDay);
        if (previousMoonDayItem != null)
        {
            // Assuming the PreviousMoonDay's NewMoonDayInfo should be applied
            // to the current MoonDay's PreviousMoonDayInfo
            DayAstroEvent.MoonDay.PreviousMoonDayInfo = previousMoonDayItem.NewMoonDayInfo;
        }

        if (DayAstroEvent.MoonDay.IsTripleMoonDay == true)
        {
            var middleMoonDayItem = App.AppData.AppDB.MoonDaysDB.FirstOrDefault(m =>
            m.NewMoonDay == DayAstroEvent.MoonDay.MiddleMoonDay);
            
            if (middleMoonDayItem != null)
            {
                DayAstroEvent.MoonDay.MiddleMoonDayInfo = middleMoonDayItem.NewMoonDayInfo;
            }
        }
    }
    private void TapPreviousMoonDay_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Details about " + (MoonDaySymbol)DayAstroEvent.MoonDay.PreviousMoonDay + " Day", DayAstroEvent.MoonDay.PreviousMoonDayInfo, "OK");
    }
    private void TapMiddleMoonDay_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Details about " + (MoonDaySymbol)DayAstroEvent.MoonDay.MiddleMoonDay + " Day", DayAstroEvent.MoonDay.MiddleMoonDayInfo, "OK");
    }

    private void TapNewMoonDay_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Details about " + (MoonDaySymbol)DayAstroEvent.MoonDay.NewMoonDay + " Day", DayAstroEvent.MoonDay.NewMoonDayInfo, "OK");
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

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (layout.Width <= 0) return; // Ensure the layout has been laid out

        // Preparing the calculation
        var totalMinutesInDay = 24 * 60 - 1;
        var transitionMinutes = dayAstroEvent.MoonDay.TransitionTime.Hour * 60 + dayAstroEvent.MoonDay.TransitionTime.Minute;
        var positionRatio = (double)transitionMinutes / totalMinutesInDay;
        var targetPositionX = positionRatio * layout.Width; // Calculate target X based on layout width

        // Update label text with HH:mm format
        timeLabel.Text = dayAstroEvent.MoonDay.TransitionTime.ToString("HH:mm");

        // Animate marker to the target position
        uint animationDuration = 3000; // Adjust based on your preference
        marker.TranslateTo(targetPositionX, 0, animationDuration, Easing.Linear);

        // Assuming the label should be directly above the marker, calculate an offset if needed
        var labelYOffset = 20; // Adjust as needed
                                // Make sure to adjust the X translation if you need the label to precisely follow the marker's center
        timeLabel.TranslateTo(targetPositionX, timeLabel.TranslationY, animationDuration, Easing.Linear);
    }

    private void AnimateMarkerToPosition(double targetPositionX)
    {
        // Use TranslateTo for animation, adjust duration and easing as needed
        var animationDuration = 1000; // Duration in milliseconds, adjust based on your preference
        this.marker.TranslateTo(targetPositionX, 0, (uint)animationDuration, Easing.Linear);
    }
}