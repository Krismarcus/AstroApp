using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    
    private Image currentlyEnlargedMoonImage;
    private bool isMoonImageEnlarged = false;

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
            UpdateMoonDayInfo();
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

    private async void MiddleMoonDayRecognizer_Tapped(object sender, TappedEventArgs e)
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

    private async void NewMoonDayRecognizer_Tapped(object sender, TappedEventArgs e)
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

    private async void MoonDayInfoContainerHideRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await MoonDayInfoGrid.FadeTo(0, 250); // 250ms for fade out
        MoonDayInfoGrid.IsVisible = false;
    }

    private async Task ToggleMoonImageAnimation(Image moonImage, string moonDayInfo)
    {
        // If there's a previously enlarged image and it's not the same as the current, shrink it
        if (currentlyEnlargedMoonImage != null && currentlyEnlargedMoonImage != moonImage && isMoonImageEnlarged)
        {
            await currentlyEnlargedMoonImage.ScaleTo(1, 200, Easing.CubicIn);
            await HideMoonDayInfo(); // Make sure to hide (fade out) the chat bubble before updating content
        }

        // Update the chat bubble content in the appropriate place based on your logic here

        // Now handle the current image
        if (currentlyEnlargedMoonImage != moonImage || !isMoonImageEnlarged)
        {
            await moonImage.ScaleTo(1.5, 400, Easing.CubicOut);
            isMoonImageEnlarged = true;
            currentlyEnlargedMoonImage = moonImage;

            // Since the chat bubble is already invisible or fading out, now update the content
            // This should happen right before you start fading it back in
            UpdateChatBubbleContent(moonDayInfo); // Placeholder for where you'd update the content

            // Show the chat bubble with the updated info
            await ShowMoonDayInfo();
        }
        else
        {
            await moonImage.ScaleTo(1, 400, Easing.CubicIn);
            isMoonImageEnlarged = false;
            // Optionally, hide the chat bubble if the same image is clicked again
            await HideMoonDayInfo();
        }
    }



    private async void MoonImage_Tapped(object sender, EventArgs e)
    {
        if (MoonDayConteiner == null)
        {
            MoonDayConteiner = new MoonDaySlide();
        }

        if (sender == previousMoonDayImage)
        {
            var previousMoonImage = this.previousMoonDayImage.Children.OfType<Image>().FirstOrDefault();
            if (previousMoonImage != null)
            {   
                string moonDayInfo = DayAstroEvent.MoonDay.PreviousMoonDayInfo;
                await ToggleMoonImageAnimation(previousMoonImage, moonDayInfo);
                
            }
        }
        else if (sender == newMoonDayImage)
        {
            var newMoonDayImage = this.newMoonDayImage.Children.OfType<Image>().FirstOrDefault();
            if (newMoonDayImage != null)
            {
                string moonDayInfo = DayAstroEvent.MoonDay.NewMoonDayInfo;
                await ToggleMoonImageAnimation(newMoonDayImage, moonDayInfo);                
            }
        }
    }

    private async Task ShowMoonDayInfo()
    {
        // Fade out existing content quickly if it's already visible
        if (chatBubble.IsVisible)
        {
            await chatBubble.FadeTo(0, 200, Easing.CubicIn);
        }

        // Assuming chatBubble.Text is updated elsewhere in the MoonImage_Tapped handler
        chatBubble.Opacity = 0;
        chatBubble.Scale = 0.5;
        chatBubble.IsVisible = true;

        // Fade in new content
        var fadeAnimation = chatBubble.FadeTo(1, 200, Easing.CubicOut);
        var scaleAnimation = chatBubble.ScaleTo(1, 200, Easing.CubicOut);
        await Task.WhenAll(fadeAnimation, scaleAnimation);
    }

    private async Task HideMoonDayInfo()
    {
        // Initiate fade out
        await chatBubble.FadeTo(0, 200, Easing.CubicIn);

        // Scale down and hide
        var scaleAnimation = chatBubble.ScaleTo(0.5, 200, Easing.CubicIn);
        await Task.WhenAll(scaleAnimation);
        chatBubble.IsVisible = false;
    }

    private void UpdateChatBubbleContent(string newContent)
    {
        // Directly update the chat bubble's text here
        MoonDayConteiner.MoonDayInfo = newContent;
    }

    private async Task ResetToDefaultState()
    {
        // Check if any moon image is currently enlarged and reset its scale if necessary
        if (isMoonImageEnlarged && currentlyEnlargedMoonImage != null)
        {
            await currentlyEnlargedMoonImage.ScaleTo(1, 400, Easing.CubicIn);
            isMoonImageEnlarged = false;
            currentlyEnlargedMoonImage = null; // Reset the reference to the currently enlarged moon image
        }

        // Hide the chat bubble with animation and then reset its properties
        await HideMoonDayInfo();

        // After the chat bubble is hidden, reset its text and other properties to default        
        chatBubble.Opacity = 1; // Reset opacity back to fully opaque
        chatBubble.Scale = 1; // Reset scale to its original size
        chatBubble.IsVisible = false; // Ensure it's hidden

        // Reset any other states or properties related to your UI or data here
        // For example, if you have a container or model that needs to be reset
        // MoonDayConteiner.MoonDayInfo = null; // Reset or clear as appropriate for your application
    }

    private async void CloseChatBubble_Clicked(object sender, EventArgs e)
    {
        // Call the method to reset the UI elements to their original state
        // This includes hiding the chat bubble and resetting any enlarged images
        await ResetToDefaultState();
    }
}


