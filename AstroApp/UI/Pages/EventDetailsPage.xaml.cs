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
    public ObservableCollection<MoonDaySlide> MoonDaysCarousel { get; set; }

    public EventDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {        
        base.OnAppearing();
        // It's crucial not to await in OnAppearing directly, hence using Task.Run
        Task.Run(() => InitializeDataAsync(currentDate));
    }

    public async Task InitializeDataAsync(DateTime date)
    {
        CurrentDate = date;
        DayAstroEvent = AstroEvents.FirstOrDefault(e => e.Date == date);

        // Ensure UI operations happen on the main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            timeLabel.Opacity = 0;
            UpdateDayEventInfoList();
            GenerateCarousel();
            // Delay the animation slightly to ensure UI elements are ready
            Task.Delay(200).ContinueWith(t => AnimateMarkerToPosition(DayAstroEvent.MoonDay));
        });
    }


    private void GenerateCarousel()
    {
        MoonDaysCarousel = new ObservableCollection<MoonDaySlide>();
        MoonDaysCarousel.Add(new MoonDaySlide { MoonDay = DayAstroEvent.MoonDay.PreviousMoonDay, MoonDayInfo = DayAstroEvent.MoonDay.PreviousMoonDayInfo, });
        MoonDaysCarousel.Add(new MoonDaySlide { MoonDay = DayAstroEvent.MoonDay.NewMoonDay, MoonDayInfo = DayAstroEvent.MoonDay.NewMoonDayInfo, TransitionTime = DayAstroEvent.MoonDay.TransitionTime });
        if (DayAstroEvent.MoonDay.IsTripleMoonDay == true)
        {
            MoonDaysCarousel.Add(new MoonDaySlide { MoonDay = DayAstroEvent.MoonDay.MiddleMoonDay, MoonDayInfo = DayAstroEvent.MoonDay.MiddleMoonDayInfo, TransitionTime = DayAstroEvent.MoonDay.MiddleMoonDayTransitionTime });
        }
    }

    private async void AnimateMarkerToPosition(MoonDay moonDay)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (layout.Width <= 0) return; // Ensures the layout is ready

            var totalMinutesInDay = 24 * 60;
            // Calculate the starting X position as the width of the previousMoonDayMarker image or any fixed offset if known
            var startOffset = previousMoonDayMarker.Width + timeLine.Margin.Left; // Assume this is the width or use a fixed offset if the image width is dynamic
            var availableWidth = layout.Width - startOffset - newMoonDayMarker.Width; // Adjust available width for animation

            var newMoonTransitionMinutes = moonDay.TransitionTime.Hour * 60 + moonDay.TransitionTime.Minute;
            var newPositionRatio = (double)newMoonTransitionMinutes / totalMinutesInDay;
            // Calculate the target X position, ensuring it aligns with the start of the line for a 0:00 TransitionTime
            var gridTargetPositionX = startOffset + (availableWidth * newPositionRatio);

            // Initially set the timeLabel's opacity to 0 to ensure it's not visible during the animation
            timeLabel.Opacity = 0;

            // Animate the grid to its target position
            uint animationDuration = 200;
            await newMoonDayMarkerGrid.TranslateTo(gridTargetPositionX - newMoonDayMarker.Width / 2, 0, animationDuration, Easing.Linear);

            // Make the timeLabel appear after 0.5 sec delay once the grid has reached its target position
            await Task.Delay(500); // Delay for 0.5 sec
            await timeLabel.FadeTo(1, 200); // Fade in the label after the delay
        });
    }

    private async void OnPageTapped(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync(); // Close the modal page
    }

    public async Task InitializeAstroEventList()
    {
        this.AstroEvents = App.AppData.AppDB.AstroEventsDB;        
    }    

    private async void PrevDateButton_Clicked(object sender, EventArgs e)
    {
        timeLabel.Opacity = 0;
        // Adjust the date to the previous day        
        CurrentDate = CurrentDate.AddDays(-1);
        await InitializeDataAsync(CurrentDate);
    }

    private async void NextDateButton_Clicked(object sender, EventArgs e)
    {        
        // Adjust the date to the next day        
        CurrentDate = CurrentDate.AddDays(1);
        await InitializeDataAsync(CurrentDate);
    }

    private void UpdateDayEventInfoList()
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

    private async Task ShowMoonDayCarousel(int position)
    {
        // Ensure the view is positioned off-screen initially for the slide-in effect
        MoonDayCarousel.TranslationX = -this.Width;
        MoonDayCarousel.IsVisible = true;

        MoonDayCarousel.Position = position;

        // Animate the view to slide in from the left
        await MoonDayCarousel.TranslateTo(0, 0, 250, Easing.SinInOut);
    }

    private async Task HideMoonDayCarousel()
    {
        // Animate the view to slide out to the left
        await MoonDayCarousel.TranslateTo(-this.Width, 0, 250, Easing.SinInOut);

        // After animation completes, hide the view
        MoonDayCarousel.IsVisible = false;
    }

    private void MoonDayRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender == previousMoonDayMarker)
        {
            _ = ShowMoonDayCarousel(0);
        }
        else if (sender == middleMoonDayMarker)
        {
            _ = ShowMoonDayCarousel(2);
        }
        else if (sender == newMoonDayMarker)
        {
            _ = ShowMoonDayCarousel(1);
        }
    }

    private void MoonDayInfoConteinerRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        HideMoonDayCarousel();
    }

}