using Astrodaiva.Data.Enums;
using Astrodaiva.Data.Models;
using Astrodaiva.UI.Tools;
using Microsoft.Maui.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;

namespace Astrodaiva.UI.Pages;

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

    public AstroDayDetails AstroDayDetails { get; set; }

    public ObservableCollection<AstroEvent> AstroEvents { get; set; } = new ObservableCollection<AstroEvent>();
    public ObservableCollection<PlanetInRetrogradeDetails> PlanetInRetrogradeDetails { get; set; } = new ObservableCollection<PlanetInRetrogradeDetails>();

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

    private SKBitmap _blurredBitmap;

    public EventDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
        if (InfoConteiner == null)
        {
            InfoConteiner = new InfoScreen();
        }
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
            Task.Delay(50).ContinueWith(t => AnimateMarkerToPosition(DayAstroEvent.MoonDay));
        });
    }

    private async void OnMonthTapped(object sender, TappedEventArgs e)
    {        
        await monthLabel.ScaleTo(1.1, 100);
        // Then scale it back to original size over 100 milliseconds.
        await monthLabel.ScaleTo(1.0, 100);
        await Navigation.PopModalAsync(); // Close the modal page
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
        var frame = sender as Grid; // Cast sender to Border.
        if (frame == null) return; // Safety check.

        await downArrowImage.TranslateTo(0, 10, 100); // Move 10 units to the left over 100ms
        await downArrowImage.TranslateTo(0, 0, 100); // Move back to original position
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
        if (App.AppData.AppDB.MoonDayDetailsDB == null || App.AppData.AppDB.PlanetInZodiacsDB == null || DayAstroEvent == null)
            return;
        AstroDayDetails = new AstroDayDetails();        
        UpdatePlanetInZodiacInfo();
        UpdateMoonDayInfo();

        if (App.AppData.AppDB.PlanetInRetrogradeDetailsDB != null)
        {
            PlanetInRetrogradeDetails = App.AppData.AppDB.PlanetInRetrogradeDetailsDB;
        }
    }

    private void UpdatePlanetInZodiacInfo()
    {
        if (DayAstroEvent == null) return;

        // Update Sun in Zodiac Details
        var sunInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Sun && p.ZodiacSign == DayAstroEvent.SunInZodiac.NewZodiacSign);
        if (sunInfoSourceItem != null)
        {
            AstroDayDetails.SunInZodiacDetails = sunInfoSourceItem.PlanetInZodiacInfo;
        }

        // Update Moon in Zodiac Details
        var moonInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Moon && p.ZodiacSign == DayAstroEvent.MoonInZodiac.NewZodiacSign);
        if (moonInfoSourceItem != null)
        {
            AstroDayDetails.MoonInZodiacDetails = moonInfoSourceItem.PlanetInZodiacInfo;
        }

        // Update Mercury in Zodiac Details
        var mercuryInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Mercury && p.ZodiacSign == DayAstroEvent.MercuryInZodiac.NewZodiacSign);
        if (mercuryInfoSourceItem != null)
        {
            AstroDayDetails.MercuryInZodiacDetails = mercuryInfoSourceItem.PlanetInZodiacInfo;
        }

        // Update Venus in Zodiac Details
        var venusInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Venus && p.ZodiacSign == DayAstroEvent.VenusInZodiac.NewZodiacSign);
        if (venusInfoSourceItem != null)
        {
            AstroDayDetails.VenusInZodiacDetails = venusInfoSourceItem.PlanetInZodiacInfo;
        }

        // Update Mars in Zodiac Details
        var marsInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Mars && p.ZodiacSign == DayAstroEvent.MarsInZodiac.NewZodiacSign);
        if (marsInfoSourceItem != null)
        {
            AstroDayDetails.MarsInZodiacDetails = marsInfoSourceItem.PlanetInZodiacInfo;
        }

        // Update any other planets similarly...
    }

    private void UpdateMoonDayInfo()
    {
        if (DayAstroEvent?.MoonDay == null || App.AppData.AppDB.MoonDayDetailsDB == null)
            return;

        var infoSourceItem = App.AppData.AppDB.MoonDayDetailsDB.FirstOrDefault(m =>
            m.MoonDay == DayAstroEvent.MoonDay.NewMoonDay);
        if (infoSourceItem != null)
        {
            AstroDayDetails.NewMoonDayDetails = infoSourceItem.MoonDayInfo;
        }

        // Find and apply PreviousMoonDayInfo
        var previousMoonDayItem = App.AppData.AppDB.MoonDayDetailsDB.FirstOrDefault(m =>
            m.MoonDay == DayAstroEvent.MoonDay.PreviousMoonDay);
        if (previousMoonDayItem != null)
        {
            // Assuming the PreviousMoonDay's NewMoonDayInfo should be applied
            // to the current MoonDay's PreviousMoonDayInfo
            AstroDayDetails.PreviousMoonDayDetails = previousMoonDayItem.MoonDayInfo;
        }

        if (DayAstroEvent.MoonDay.IsTripleMoonDay == true)
        {
            var middleMoonDayItem = App.AppData.AppDB.MoonDayDetailsDB.FirstOrDefault(m =>
            m.MoonDay == DayAstroEvent.MoonDay.MiddleMoonDay);

            if (middleMoonDayItem != null)
            {
                AstroDayDetails.MiddleMoonDayDetails = middleMoonDayItem.MoonDayInfo;
            }
        }
    }

    private async Task ToggleMoonGridAnimation(string infoText, string header)
    {
        await ShowMoonDayInfo(infoText, header); // Assuming you have a method to show details about the moon day
        
    }



    private async void MoonImage_Tapped(object sender, EventArgs e)
    {
        if (sender == previousMoonDayImageGrid)
        {
            string moonDayLT = TranslationManager.TranslateMoonDay(DayAstroEvent.MoonDay.PreviousMoonDay);
            string moonDayInfo = AstroDayDetails.PreviousMoonDayDetails;
            await ToggleMoonGridAnimation(moonDayInfo, moonDayLT);               
            
        }
        else if (sender == newMoonDayImageGrid)
        {
            string moonDayLT = TranslationManager.TranslateMoonDay(DayAstroEvent.MoonDay.NewMoonDay);
            string moonDayInfo = AstroDayDetails.NewMoonDayDetails;
            await ToggleMoonGridAnimation(moonDayInfo, moonDayLT);                
            
        }
        else if (sender == middleMoonDayMarkerGrid)
        {
            string moonDayLT = TranslationManager.TranslateMoonDay(DayAstroEvent.MoonDay.MiddleMoonDay);
            string moonDayInfo = AstroDayDetails.MiddleMoonDayDetails;
            await ToggleMoonGridAnimation(moonDayInfo, moonDayLT);

        }
    }

    private async Task ShowMoonDayInfo(string infoText, string header)
    {
        popupText.Text = infoText;
        popupHeader.Text = header;
        await CaptureAndBlurAsync();
        //Show the overlay and popup
        popupOverlay.Opacity = 0;
        popupOverlay.IsVisible = true;

        await Task.WhenAll(
            popupOverlay.FadeTo(1, 200, Easing.CubicOut)
        );
    }

    private async void HideInfoScreen(object sender, TappedEventArgs e)
    {
        await Task.WhenAll(
            popupOverlay.FadeTo(0, 200, Easing.CubicIn)
        );

        popupOverlay.IsVisible = false;
        _blurredBitmap = null;

    }

    private async void PlanetInZodiacGrid_Tapped(object sender, EventArgs e)
    {
        if (sender == sunInZodiacGrid)
        {            
            if (sunInZodiacGrid != null)
            {
                string sunInZodiacHeader = TranslationManager.TranslatePlanetInZodiac(Planet.Sun, DayAstroEvent.SunInZodiac.NewZodiacSign);
                string sunInZodiacInfo = AstroDayDetails.SunInZodiacDetails;                
                await ToggleMoonGridAnimation(sunInZodiacInfo, sunInZodiacHeader);

            }
        }

        if (sender == moonInZodiacGrid)
        {
            if (moonInZodiacGrid != null)
            {
                string moonInZodiacHeader = TranslationManager.TranslatePlanetInZodiac(Planet.Moon, DayAstroEvent.MoonInZodiac.NewZodiacSign);
                string moonInZodiacInfo = AstroDayDetails.MoonInZodiacDetails;
                await ToggleMoonGridAnimation(moonInZodiacInfo, moonInZodiacHeader);

            }
        }

        if (sender == venusInZodiacGrid)
        {
            if (venusInZodiacGrid != null)
            {
                string venusInZodiacHeader = TranslationManager.TranslatePlanetInZodiac(Planet.Venus, DayAstroEvent.VenusInZodiac.NewZodiacSign);
                string venusInZodiacInfo = AstroDayDetails.VenusInZodiacDetails;

                if (DayAstroEvent.VenusInZodiac.IsRetrograde == true)
                {
                    string venusInRetrogradeInfo = PlanetInRetrogradeDetails.FirstOrDefault(r => r.PlanetInRetrograde == Planet.Venus).PlanetInRetrogradeInfo;
                    venusInZodiacInfo = venusInZodiacInfo + "\n" + venusInRetrogradeInfo;
                }
                await ToggleMoonGridAnimation(venusInZodiacInfo, venusInZodiacHeader);

            }
        }

        if (sender == marsInZodiacGrid)
        {
            if (marsInZodiacGrid != null)
            {
                string marsInZodiacHeader = TranslationManager.TranslatePlanetInZodiac(Planet.Mars, DayAstroEvent.MarsInZodiac.NewZodiacSign);
                string marsInZodiacInfo = AstroDayDetails.MarsInZodiacDetails;

                if (DayAstroEvent.MarsInZodiac.IsRetrograde == true)
                {
                    string marsInRetrogradeInfo = PlanetInRetrogradeDetails.FirstOrDefault(r => r.PlanetInRetrograde == Planet.Mars).PlanetInRetrogradeInfo;
                    marsInZodiacInfo = marsInZodiacInfo + "\n" + marsInRetrogradeInfo;
                }
                await ToggleMoonGridAnimation(marsInZodiacInfo, marsInZodiacHeader);
            }
        }

        if (sender == mercuryInZodiacGrid)
        {
            if (mercuryInZodiacGrid != null)
            {
                string mercuryInZodiacHeader = TranslationManager.TranslatePlanetInZodiac(Planet.Mercury, DayAstroEvent.MercuryInZodiac.NewZodiacSign);                
                string mercuryInZodiacInfo = AstroDayDetails.MercuryInZodiacDetails;

                if (DayAstroEvent.MercuryInZodiac.IsRetrograde == true)
                {
                    string mercuryInRetrogradeInfo = PlanetInRetrogradeDetails.FirstOrDefault(r => r.PlanetInRetrograde == Planet.Mercury).PlanetInRetrogradeInfo;
                    mercuryInZodiacInfo = mercuryInZodiacInfo + "\n" + mercuryInRetrogradeInfo;
                }
                await ToggleMoonGridAnimation(mercuryInZodiacInfo, mercuryInZodiacHeader);

            }
        }
    }

    private async Task CaptureAndBlurAsync()
    {
        // If already blurred and no update needed, reuse
        if (_blurredBitmap != null)
        {
            blurCanvas.IsVisible = true;
            blurCanvas.InvalidateSurface();
            return;
        }

        var screenshot = await Screenshot.CaptureAsync();
        if (screenshot == null) return;

        using var stream = await screenshot.OpenReadAsync();

        // Process in background thread
        _blurredBitmap = await Task.Run(() =>
        {
            using var skStream = new SKManagedStream(stream);
            var original = SKBitmap.Decode(skStream);
            if (original == null) return null;

            // Create a new bitmap with the same dimensions as original
            var blurred = new SKBitmap(original.Width, original.Height);

            // Apply blur directly to the full-size image
            var filter = SKImageFilter.CreateBlur(4f, 4f);
            var paint = new SKPaint { ImageFilter = filter };

            using (var canvas = new SKCanvas(blurred))
            {
                canvas.Clear();
                canvas.DrawBitmap(original, 0, 0, paint);
            }

            return blurred;
        });

        if (_blurredBitmap != null)
        {
            blurCanvas.IsVisible = true;
            blurCanvas.InvalidateSurface();
        }
    }

    private void OnBlurCanvasPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        if (_blurredBitmap == null) return;

        var canvas = e.Surface.Canvas;
        canvas.Clear();

        var dest = new SKRect(0, 0, e.Info.Width, e.Info.Height);
        canvas.DrawBitmap(_blurredBitmap, dest);
    }

}


