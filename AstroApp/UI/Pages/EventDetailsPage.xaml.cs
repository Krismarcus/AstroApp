using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;

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

    private MoonDayDetails moonDayConteiner;
    public MoonDayDetails MoonDayConteiner
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

    private PlanetInZodiacDetails planetInZodiacConteiner;
    public PlanetInZodiacDetails PlanetInZodiacConteiner
    {
        get => planetInZodiacConteiner;
        set
        {
            if (planetInZodiacConteiner != value)
            {
                planetInZodiacConteiner = value;
                OnPropertyChanged(nameof(PlanetInZodiacConteiner));
            }
        }
    }

    private Grid currentlyEnlargedMoonGrid;
    private bool isMoonGridEnlarged = false;

    private Grid currentlyEnlargedPlanetInZodiacGrid;
    private bool isPlanetInZodiacGridEnlarged = false;

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
            ResetToDefaultState();
            UpdateDayEventInfoList();
            Task.Delay(50).ContinueWith(t => AnimateMarkerToPosition(DayAstroEvent.MoonDay));
        });
    }    

    private async void AnimateMarkerToPosition(MoonDay moonDay)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (newMoonDayMarkerGrid.Width <= 0) return; // Ensures the layout is ready

            var totalMinutesInDay = 24 * 60;
            var startImageWidth = 80;
            var startOffset = 5 + startImageWidth; // Adjust based on the left margin of the gradient line.
            var endOffset = -startImageWidth - 10; // Adjust based on the right margin of the gradient line.
            var lineWidthAdjustment = 10; // Adjustment for the marker's width to align with the line start/end.

            var availableWidth = timeLine.Width - (startOffset + endOffset) - lineWidthAdjustment;

            // Animate Middle Moon Day Marker if it's a Triple Moon Day
            if (moonDay.IsTripleMoonDay)
            {
                var middleMoonPosition = CalculateMarkerPosition(moonDay.MiddleMoonDayTransitionTime, totalMinutesInDay, startOffset, availableWidth);
                await AnimateMarker(middleMoonDayMarkerGrid, middleMoonPosition, secondTimeLabel);
            }

            // Continue with New Moon Day Marker animation
            var newMoonPosition = CalculateMarkerPosition(moonDay.TransitionTime, totalMinutesInDay, startOffset, availableWidth);
            await AnimateMarker(newMoonDayMarkerGrid, newMoonPosition, timeLabel);
        });
    }

    private double CalculateMarkerPosition(DateTime transitionDateTime, int totalMinutesInDay, double startOffset, double availableWidth)
    {
        var transitionMinutes = transitionDateTime.TimeOfDay.TotalMinutes; // Use TotalMinutes of TimeOfDay
        var positionRatio = transitionMinutes / totalMinutesInDay;
        return startOffset + (availableWidth * positionRatio);
    }

    private async Task AnimateMarker(Grid markerGrid, double targetPositionX, Label label)
    {
        uint animationDuration = 200;
        await markerGrid.TranslateTo(targetPositionX - (markerGrid.Width / 2), 0, animationDuration, Easing.Linear);

        await Task.Delay(300); // Delay for visibility transition
        await label.FadeTo(1, 200); // Fade in the label after the delay
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
        
        // Adjust the date to the previous day        
        CurrentDate = CurrentDate.AddDays(-1);
        await InitializeDataAsync(CurrentDate);
    }

    private async void NextDateButton_Clicked(object sender, TappedEventArgs e)
    {
        await rightArrow.TranslateTo(10, 0, 100); // Move 10 units to the right over 100ms
        await rightArrow.TranslateTo(0, 0, 100); // Move back to original position
        timeLabel.Opacity = 0;
        
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

    private async Task ToggleMoonGridAnimation(Grid moonGrid, string moonDayInfo)
    {
        // Check if there's a previously enlarged grid and it's not the same as the current, shrink it
        if (currentlyEnlargedMoonGrid != null && currentlyEnlargedMoonGrid != moonGrid && isMoonGridEnlarged)
        {
            await currentlyEnlargedMoonGrid.ScaleTo(1, 200, Easing.CubicIn);
            await HideMoonDayInfo(); // Assuming you have a method to hide details about the moon day
        }

        // Now handle the current grid
        if (currentlyEnlargedMoonGrid != moonGrid || !isMoonGridEnlarged)
        {
            await moonGrid.ScaleTo(1.5, 400, Easing.CubicOut);
            isMoonGridEnlarged = true;
            currentlyEnlargedMoonGrid = moonGrid;

            // Update the details about the moon day here

            // Show the details after updating
            await ShowMoonDayInfo(moonDayInfo); // Assuming you have a method to show details about the moon day
        }
        else
        {
            await moonGrid.ScaleTo(1, 400, Easing.CubicIn);
            isMoonGridEnlarged = false;
            // Optionally, hide the details if the same grid is tapped again
            await HideMoonDayInfo();
        }
    }



    private async void MoonImage_Tapped(object sender, EventArgs e)
    {
        if (MoonDayConteiner == null)
        {
            MoonDayConteiner = new MoonDayDetails();
        }

        if (sender == previousMoonDayImageGrid)
        {            
                string moonDayInfo = DayAstroEvent.MoonDay.PreviousMoonDayInfo;
                await ToggleMoonGridAnimation(previousMoonDayImageGrid, moonDayInfo);               
            
        }
        else if (sender == newMoonDayImageGrid)
        {                        
                string moonDayInfo = DayAstroEvent.MoonDay.NewMoonDayInfo;
                await ToggleMoonGridAnimation(newMoonDayImageGrid, moonDayInfo);                
            
        }
    }

    private async Task ShowMoonDayInfo(string moonDayInfo)
    {
        // Fade out existing content quickly if it's already visible
        if (moonDayInfoScreen.IsVisible)
        {
            await moonDayInfoScreen.FadeTo(0, 200, Easing.CubicIn);
        }

        // Assuming chatBubble.Text is updated elsewhere in the MoonImage_Tapped handler
        moonDayInfoScreen.Opacity = 0;
        moonDayInfoScreen.Scale = 0.5;
        moonDayInfoScreen.IsVisible = true;
        UpdateChatBubbleContent(moonDayInfo);

        // Fade in new content
        var fadeAnimation = moonDayInfoScreen.FadeTo(0.8, 200, Easing.CubicOut);
        var scaleAnimation = moonDayInfoScreen.ScaleTo(1, 200, Easing.CubicOut);
        await Task.WhenAll(fadeAnimation, scaleAnimation);
    }

    private async Task HideMoonDayInfo()
    {
        // Initiate fade out
        await moonDayInfoScreen.FadeTo(0, 200, Easing.CubicIn);

        // Scale down and hide
        var scaleAnimation = moonDayInfoScreen.ScaleTo(0.5, 200, Easing.CubicIn);
        await Task.WhenAll(scaleAnimation);
        moonDayInfoScreen.IsVisible = false;
    }

    private void UpdateChatBubbleContent(string newContent)
    {
        // Directly update the chat bubble's text here
        MoonDayConteiner.MoonDayInfo = newContent;
    }

    private async Task ResetToDefaultState()
    {
        // Check if any moon image is currently enlarged and reset its scale if necessary
        if (isMoonGridEnlarged && currentlyEnlargedMoonGrid != null)
        {
            await currentlyEnlargedMoonGrid.ScaleTo(1, 400, Easing.CubicIn);
            isMoonGridEnlarged = false;
            currentlyEnlargedMoonGrid = null; // Reset the reference to the currently enlarged moon image
        }

        // Hide the chat bubble with animation and then reset its properties
        await HideMoonDayInfo();

        // After the chat bubble is hidden, reset its text and other properties to default        
        moonDayInfoScreen.Opacity = 0.8; // Reset opacity back to fully opaque
        moonDayInfoScreen.Scale = 1; // Reset scale to its original size
        moonDayInfoScreen.IsVisible = false; // Ensure it's hidden

        // Reset any other states or properties related to your UI or data here
        // For example, if you have a container or model that needs to be reset
        // MoonDayConteiner.MoonDayInfo = null; // Reset or clear as appropriate for your application
    }

    private async void CloseMoonDayInfoScreen_Clicked(object sender, EventArgs e)
    {
        // Call the method to reset the UI elements to their original state
        // This includes hiding the chat bubble and resetting any enlarged images
        await ResetToDefaultState();
    }

    private async void PlanetInZodiacGrid_Tapped(object sender, EventArgs e)
    {
        if (MoonDayConteiner == null)
        {
            MoonDayConteiner = new MoonDayDetails();
        }

        if (sender == sunInZodiacGrid)
        {            
            if (sunInZodiacGrid != null)
            {
                string sunInZodiacInfo = DayAstroEvent.SunInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(sunInZodiacGrid, sunInZodiacInfo);

            }
        }

        if (sender == moonInZodiacGrid)
        {
            if (moonInZodiacGrid != null)
            {
                string moonInZodiacInfo = DayAstroEvent.MoonInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(moonInZodiacGrid, moonInZodiacInfo);

            }
        }

        if (sender == venusInZodiacGrid)
        {
            if (venusInZodiacGrid != null)
            {
                string venusInZodiacInfo = DayAstroEvent.VenusInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(venusInZodiacGrid, venusInZodiacInfo);

            }
        }

        if (sender == marsInZodiacGrid)
        {
            if (marsInZodiacGrid != null)
            {
                string marsInZodiacInfo = DayAstroEvent.MarsInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(marsInZodiacGrid, marsInZodiacInfo);
            }
        }

        if (sender == mercuryInZodiacGrid)
        {
            if (mercuryInZodiacGrid != null)
            {
                string mercuryInZodiacInfo = DayAstroEvent.MercuryInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(mercuryInZodiacGrid, mercuryInZodiacInfo);

            }
        }
    }
}


