using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;
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

    public ObservableCollection<MoonDaySlide> MoonDaysCarousel { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public EventDetailsPage()
    {
        InitializeComponent();
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

    var totalMinutesInDay = 24 * 60; // Total minutes in a day

    // Adjusted width to account for the static image on one side
    var offsetForStaticImage = 40; // Width of the static image taking up space
    var availableWidth = layout.Width - offsetForStaticImage - newMoonDayMarker.Width; // Adjust for the image width on both ends

    // Calculate position for the New Moon marker
    var newMoonTransitionMinutes = moonDay.TransitionTime.Hour * 60 + moonDay.TransitionTime.Minute;
    var newPositionRatio = (double)newMoonTransitionMinutes / totalMinutesInDay;
    var newMoonTargetPositionX = availableWidth * newPositionRatio + offsetForStaticImage; // Offset added to the target position

    // Update and show the label for New Moon
    timeLabel.Text = moonDay.TransitionTime.ToString("HH:mm");
    var labelXPosition = newMoonTargetPositionX - (timeLabel.Width / 2);
    labelXPosition = Math.Max(labelXPosition, offsetForStaticImage); // Adjust for offset
    labelXPosition = Math.Min(labelXPosition, layout.Width - timeLabel.Width - offsetForStaticImage); // Prevent overflow beyond the available width

    timeLabel.TranslationX = labelXPosition +2;
    timeLabel.TranslationY = 40;
    timeLabel.IsVisible = true;

    uint animationDuration = 200; // Animation duration in milliseconds
    await newMoonDayMarker.TranslateTo(newMoonTargetPositionX, 0, animationDuration, Easing.Linear);

    // Repeat similar calculations for the Middle Moon marker if applicable
    if (moonDay.IsTripleMoonDay)
    {
        var middleMoonTransitionMinutes = moonDay.MiddleMoonDayTransitionTime.Hour * 60 + moonDay.MiddleMoonDayTransitionTime.Minute;
        var middlePositionRatio = (double)middleMoonTransitionMinutes / totalMinutesInDay;
        var middleTargetPositionX = availableWidth * middlePositionRatio + offsetForStaticImage; // Adjusted for the static image

        // Update and show the label for Middle Moon
        secondTimeLabel.Text = moonDay.MiddleMoonDayTransitionTime.ToString("HH:mm");
        var middleLabelXPosition = middleTargetPositionX - (secondTimeLabel.Width / 2);
        middleLabelXPosition = Math.Max(middleLabelXPosition, offsetForStaticImage); // Ensure label stays within bounds
        middleLabelXPosition = Math.Min(middleLabelXPosition, layout.Width - secondTimeLabel.Width - offsetForStaticImage); // Prevent overflow

        secondTimeLabel.TranslationX = middleLabelXPosition;
        secondTimeLabel.TranslationY = 40;
        secondTimeLabel.IsVisible = true;

        await middleMoonDayMarker.TranslateTo(middleTargetPositionX, 0, animationDuration, Easing.Linear);
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
        GenerateCarousel();
        BindingContext = this;
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