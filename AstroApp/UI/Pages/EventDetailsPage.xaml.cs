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

    private InfoScreen infoConteiner;
    public InfoScreen InfoConteiner
    {
        get => infoConteiner;
        set
        {
            if (infoConteiner != value)
            {
                infoConteiner = value;
                OnPropertyChanged(nameof(InfoConteiner));
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
            var startOffset = 5; // Adjust based on the left margin of the gradient line.
            var endOffset = -10; // Adjust based on the right margin of the gradient line.
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

    private async Task ToggleMoonGridAnimation(Grid moonGrid, string infoText, string header)
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
            await moonGrid.ScaleTo(1.2, 400, Easing.CubicOut);
            isMoonGridEnlarged = true;
            currentlyEnlargedMoonGrid = moonGrid;

            // Update the details about the moon day here

            // Show the details after updating
            await ShowMoonDayInfo(infoText, header); // Assuming you have a method to show details about the moon day
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
        if (InfoConteiner == null)
        {
            InfoConteiner = new InfoScreen();
        }

        if (sender == previousMoonDayImageGrid)
        {
            string moonDayLT = TranslateMoonDay(DayAstroEvent.MoonDay.PreviousMoonDay);
            string moonDayInfo = DayAstroEvent.MoonDay.PreviousMoonDayInfo;
            await ToggleMoonGridAnimation(previousMoonDayImageGrid, moonDayInfo, moonDayLT);               
            
        }
        else if (sender == newMoonDayImageGrid)
        {
            string moonDayLT = TranslateMoonDay(DayAstroEvent.MoonDay.NewMoonDay);
            string moonDayInfo = DayAstroEvent.MoonDay.NewMoonDayInfo;
            await ToggleMoonGridAnimation(newMoonDayImageGrid, moonDayInfo, moonDayLT);                
            
        }
        else if (sender == middleMoonDayMarkerGrid)
        {
            string moonDayLT = TranslateMoonDay(DayAstroEvent.MoonDay.MiddleMoonDay);
            string moonDayInfo = DayAstroEvent.MoonDay.MiddleMoonDayInfo;
            await ToggleMoonGridAnimation(middleMoonDayMarkerGrid, moonDayInfo, moonDayLT);

        }
    }

    private string TranslateMoonDay(int moonDay)
    {
        switch (moonDay)
        {
            case 1:
                return "1.Žibintas";
            case 2:
                return "2.Banginis";
            case 3:
                return "3.Leopardas";
            case 4:
                return "4.Medis";
            case 5:
                return "5.Vienaragis";
            case 6:
                return "6.Vaivorykštė";
            case 7:
                return "7.Gaidys";
            case 8:
                return "8.Feniksas";
            case 9:
                return "9.Šikšnosparnis";
            case 10:
                return "10.Fontanas";
            case 11:
                return "11.Karūna";
            case 12:
                return "žuvyse";
            case 13:
                return "13.Ratas";
            case 14:
                return "14.Trimitas";
            case 15:
                return "15.Gyvatė";
            case 16:
                return "16.Balandis";
            case 17:
                return "17.Vynuogė";
            case 18:
                return "18.Bezdžionė";
            case 19:
                return "19.Voras";
            case 20:
                return "20.Erelis";
            case 21:
                return "21.Arklys";
            case 22:
                return "22.Dramblys";
            case 23:
                return "23.Krokodilas";
            case 24:
                return "24.Meška";
            case 25:
                return "25.Vėžlys";
            case 26:
                return "26.Varlė";
            case 27:
                return "27.Laivas";
            case 28:
                return "28.Lotosas";
            case 29:
                return "29.Aštunkojis";
            case 30:
                return "30.Gulbė";
            default:
                return "nežinoma diena"; // Default case for unknown or uninitialized values
        }
    }

    private async Task ShowMoonDayInfo(string infoText, string header)
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
        UpdateChatBubbleContent(infoText, header);

        // Fade in new content
        var fadeAnimation = moonDayInfoScreen.FadeTo(1, 200, Easing.CubicOut);
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

    private void UpdateChatBubbleContent(string newContent, string newHeader)
    {
        // Directly update the chat bubble's text here
        InfoConteiner.Header = newHeader;
        InfoConteiner.InfoText = newContent;
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
        moonDayInfoScreen.Opacity = 1; // Reset opacity back to fully opaque
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
        if (InfoConteiner == null)
        {
            InfoConteiner = new InfoScreen();
        }

        if (sender == sunInZodiacGrid)
        {            
            if (sunInZodiacGrid != null)
            {
                string zodiacLT = TranslateZodiac(DayAstroEvent.SunInZodiac.NewZodiacSign);
                string sunInZodiacHeader = "Saulė " + zodiacLT;
                string sunInZodiacInfo = DayAstroEvent.SunInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(sunInZodiacGrid, sunInZodiacInfo, sunInZodiacHeader);

            }
        }

        if (sender == moonInZodiacGrid)
        {
            if (moonInZodiacGrid != null)
            {
                string zodiacLT = TranslateZodiac(DayAstroEvent.MoonInZodiac.NewZodiacSign);
                string moonInZodiacHeader = "Mėnulis " + zodiacLT;
                string moonInZodiacInfo = DayAstroEvent.MoonInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(moonInZodiacGrid, moonInZodiacInfo, moonInZodiacHeader);

            }
        }

        if (sender == venusInZodiacGrid)
        {
            if (venusInZodiacGrid != null)
            {
                string zodiacLT = TranslateZodiac(DayAstroEvent.VenusInZodiac.NewZodiacSign);
                string venusInZodiacHeader = "Venera " + zodiacLT;
                string venusInZodiacInfo = DayAstroEvent.VenusInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(venusInZodiacGrid, venusInZodiacInfo, venusInZodiacHeader);

            }
        }

        if (sender == marsInZodiacGrid)
        {
            if (marsInZodiacGrid != null)
            {
                string zodiacLT = TranslateZodiac(DayAstroEvent.MarsInZodiac.NewZodiacSign);
                string marsInZodiacHeader = "Marsas " + zodiacLT;
                string marsInZodiacInfo = DayAstroEvent.MarsInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(marsInZodiacGrid, marsInZodiacInfo, marsInZodiacHeader);
            }
        }

        if (sender == mercuryInZodiacGrid)
        {
            if (mercuryInZodiacGrid != null)
            {
                string zodiacLT = TranslateZodiac(DayAstroEvent.MercuryInZodiac.NewZodiacSign);
                string mercuryInZodiacHeader = "Marsas " + zodiacLT;
                string mercuryInZodiacInfo = DayAstroEvent.MercuryInZodiac.PlanetInZodiacInfo;
                await ToggleMoonGridAnimation(mercuryInZodiacGrid, mercuryInZodiacInfo, mercuryInZodiacHeader);

            }
        }
    }

    private string TranslateZodiac(ZodiacSign planetInZodiac)
    {
        switch (planetInZodiac)
        {
            case ZodiacSign.Aries:
                return "avine";
            case ZodiacSign.Taurus:
                return "jautyje";
            case ZodiacSign.Gemini:
                return "dvyniuose";
            case ZodiacSign.Cancer:
                return "vėžyje";
            case ZodiacSign.Leo:
                return "liūte";
            case ZodiacSign.Virgo:
                return "mergelėje";
            case ZodiacSign.Libra:
                return "svarstyklėse";
            case ZodiacSign.Scorpio:
                return "skorpione";
            case ZodiacSign.Sagittarius:
                return "šaulyje";
            case ZodiacSign.Capricorn:
                return "ožiaragyje";
            case ZodiacSign.Aquarius:
                return "vandenyje";
            case ZodiacSign.Pisces:
                return "žuvyse";
            default:
                return "nežinomas ženklas"; // Default case for unknown or uninitialized values
        }
    }
}


