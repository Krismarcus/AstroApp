using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AstroApp.UI.Pages;

public partial class EventDetailsPage : ContentPage, INotifyPropertyChanged
{

    private DateTime currentDate;

    public DateTime CurrentDate
    {
        get => currentDate;
        set
        {
            if (currentDate != value)
            {
                currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
                // Trigger the update and animation each time the date changes
                Task.Run(() => InitializeDataAsync(currentDate));
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
            }
        }
    }

    public ObservableCollection<AstroEvent> AstroEvents { get; set; } = new ObservableCollection<AstroEvent>();

    private MoonDaySlide moonDayConteiner;
    public MoonDaySlide MoonDayConteiner
    {
        get => moonDayConteiner;
        set
        {
            if (moonDayConteiner != value)
            {
                moonDayConteiner = value;
                OnPropertyChanged(nameof(MoonDayConteiner));
            }
        }
    }

    public EventDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;        
    }    

    public async Task InitializeDataAsync(DateTime date)
    {                
        CurrentDate = date;
        DayAstroEvent = AstroEvents.FirstOrDefault(e => e.Date == date);

        // Ensure UI operations happen on the main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            timeLabel.Opacity = 0;            
            UpdateMoonDayInfo();
            Task.Delay(50).ContinueWith(t => AnimateMarkerToPosition(DayAstroEvent.MoonDay));
        });
    }    

    private async void AnimateMarkerToPosition(MoonDay moonDay)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (layout.Width <= 0) return; // Ensures the layout is ready

            var totalMinutesInDay = 24 * 60;
            var startOffset = previousMoonDayMarker.Width + timeLine.Margin.Left;
            var availableWidth = layout.Width - startOffset - newMoonDayMarker.Width;

            // Animate Middle Moon Day Marker if it's a Triple Moon Day
            if (moonDay.IsTripleMoonDay)
            {
                var middleMoonTransitionMinutes = moonDay.MiddleMoonDayTransitionTime.Hour * 60 + moonDay.MiddleMoonDayTransitionTime.Minute;
                var middlePositionRatio = (double)middleMoonTransitionMinutes / totalMinutesInDay;
                var middleGridTargetPositionX = startOffset + (availableWidth * middlePositionRatio);

                secondTimeLabel.Opacity = 0; // Initially set the secondTimeLabel's opacity to 0

                uint middleAnimationDuration = 200;
                await middleMoonDayMarkerGrid.TranslateTo(middleGridTargetPositionX - middleMoonDayMarker.Width / 2, 0, middleAnimationDuration, Easing.Linear);

                await Task.Delay(300); // Delay for 0.5 sec
                await secondTimeLabel.FadeTo(1, 200); // Fade in the second label after the delay
            }

            // Continue with New Moon Day Marker animation
            var newMoonTransitionMinutes = moonDay.TransitionTime.Hour * 60 + moonDay.TransitionTime.Minute;
            var newPositionRatio = (double)newMoonTransitionMinutes / totalMinutesInDay;
            var gridTargetPositionX = startOffset + (availableWidth * newPositionRatio);

            timeLabel.Opacity = 0; // Initially set the timeLabel's opacity to 0

            uint animationDuration = 200;
            await newMoonDayMarkerGrid.TranslateTo(gridTargetPositionX - newMoonDayMarker.Width / 2, 0, animationDuration, Easing.Linear);

            await Task.Delay(300); // Delay for 0.5 sec
            await timeLabel.FadeTo(1, 200); // Fade in the label after the delay
        });
    }

    private async void OnPageTapped(object sender, TappedEventArgs e)
    {
        var frame = sender as Frame; // Cast sender to Border.
        if (frame == null) return; // Safety check.

        // Scale the border to 1.1x size over 100 milliseconds.
        await frame.ScaleTo(1.1, 100);
        // Then scale it back to original size over 100 milliseconds.
        await frame.ScaleTo(1.0, 100);
        await Navigation.PopModalAsync(); // Close the modal page
    }

    public async Task InitializeAstroEventList()
    {
        this.AstroEvents = App.AppData.AppDB.AstroEventsDB;        
    }    

    private async void PrevDateButton_Clicked(object sender, TappedEventArgs e)
    {
        await leftArrow.TranslateTo(-10, 0, 100); // Move 10 units to the left over 100ms
        await leftArrow.TranslateTo(0, 0, 100); // Move back to original position
        timeLabel.Opacity = 0;

        if (MoonDayInfoGrid.IsVisible == true)
        {
            this.MoonDayInfoGrid.IsVisible = false;
        }
        // Adjust the date to the previous day        
        CurrentDate = CurrentDate.AddDays(-1);
        await InitializeDataAsync(CurrentDate);
    }

    private async void NextDateButton_Clicked(object sender, TappedEventArgs e)
    {
        await rightArrow.TranslateTo(10, 0, 100); // Move 10 units to the right over 100ms
        await rightArrow.TranslateTo(0, 0, 100); // Move back to original position
        timeLabel.Opacity = 0;

        if (MoonDayInfoGrid.IsVisible == true)
        {
            this.MoonDayInfoGrid.IsVisible = false;
        }
        // Adjust the date to the next day        
        CurrentDate = CurrentDate.AddDays(1);
        await InitializeDataAsync(CurrentDate);
    }

    public void UpdateDayEventInfoList()
    {
        if (App.AppData.AppDB.MoonDaysDB == null || App.AppData.AppDB.PlanetInZodiacsDB == null || DayAstroEvent == null)
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

    private async void MoonDayRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender == middleMoonDayMarkerGrid)
        {
            if (MoonDayConteiner == null)
            {
                MoonDayConteiner = new MoonDaySlide();
            }

            this.MoonDayConteiner.MoonDay = DayAstroEvent.MoonDay.MiddleMoonDay;
            this.MoonDayConteiner.MoonDayInfo = DayAstroEvent.MoonDay.MiddleMoonDayInfo;
            MoonDayInfoGrid.IsVisible = true;
            MoonDayInfoGrid.Opacity = 0; // Ensure it's fully transparent initially
            await MoonDayInfoGrid.FadeTo(1, 250); // 250ms for fade in

        }
        else if (sender == newMoonDayMarkerGrid)
        {
            if (MoonDayConteiner == null)
            {
                MoonDayConteiner = new MoonDaySlide();
            }

            this.MoonDayConteiner.MoonDay = DayAstroEvent.MoonDay.NewMoonDay;
            this.MoonDayConteiner.MoonDayInfo = DayAstroEvent.MoonDay.NewMoonDayInfo;
            MoonDayInfoGrid.IsVisible = true;
            MoonDayInfoGrid.Opacity = 0; // Ensure it's fully transparent initially
            await MoonDayInfoGrid.FadeTo(1, 250); // 250ms for fade in
        }
    }

    private async void PreviuosMoonDayRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (MoonDayConteiner == null)
        {
            MoonDayConteiner = new MoonDaySlide();
        }

        this.MoonDayConteiner.MoonDay = DayAstroEvent.MoonDay.PreviousMoonDay;
        this.MoonDayConteiner.MoonDayInfo = DayAstroEvent.MoonDay.PreviousMoonDayInfo;
        MoonDayInfoGrid.IsVisible = true;
        MoonDayInfoGrid.Opacity = 0; // Ensure it's fully transparent initially
        await MoonDayInfoGrid.FadeTo(1, 250); // 250ms for fade in
    }

    private async void MoonDayInfoContainerHideRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await MoonDayInfoGrid.FadeTo(0, 250); // 250ms for fade out
        MoonDayInfoGrid.IsVisible = false;
    }
    
}