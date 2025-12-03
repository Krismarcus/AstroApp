using Astrodaiva.Data.Enums;
using Astrodaiva.Data.Models;
using Astrodaiva.Services;
using Astrodaiva.UI.Tools;
using Microsoft.Maui.Graphics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Astrodaiva.UI.Pages
{
    public partial class EventDetailsPage : ContentPage, INotifyPropertyChanged
    {
        private readonly IOrientationService _orientation;

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

        // 🔹 For safe marker animation
        private bool _markerSizeHandlerAttached = false;
        private MoonDay _lastMoonDayForAnimation;

        public EventDetailsPage()
        {
            InitializeComponent();
            BindingContext = this;
            _orientation = ServiceHelper.GetRequiredService<IOrientationService>();

            if (InfoConteiner == null)
            {
                InfoConteiner = new InfoScreen();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _orientation?.LockPortrait();
        }

        protected override void OnDisappearing()
        {
            _orientation?.Unlock();
            base.OnDisappearing();
        }

        /// <summary>
        /// Main entry to load data + animate for given date.
        /// Call this when you open the modal and on prev/next buttons.
        /// </summary>
        public async Task InitializeDataAsync(DateTime date)
        {
            // keep CurrentDate updated, but no side effects in setter
            CurrentDate = date;

            if (AstroEvents == null || AstroEvents.Count == 0)
            {
                await InitializeAstroEventList();
            }

            DayAstroEvent = AstroEvents.FirstOrDefault(e => e.Date.Date == date.Date);

            if (DayAstroEvent == null)
                return;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                timeLabel.Opacity = 0;
                secondTimeLabel.Opacity = 0;

                UpdateDayEventInfoList();

                // trigger animation every time data is initialized
                AnimateMarkerToPosition(DayAstroEvent.MoonDay);
            });
        }

        public async Task InitializeAstroEventList()
        {
            AstroEvents = App.AppData.AppDB.AstroEventsDB;
        }

        private async void OnMonthTapped(object sender, TappedEventArgs e)
        {
            await downArrowImage.TranslateTo(0, 10, 100);
            await downArrowImage.TranslateTo(0, 0, 100);
            await Navigation.PopModalAsync(); // Close the modal page        
        }

        /// <summary>
        /// Animates the moon marker(s). If layout is not ready yet, it waits for SizeChanged once,
        /// then re-runs when timeLine has a real Width.
        /// </summary>
        private void AnimateMarkerToPosition(MoonDay moonDay)
        {
            if (moonDay == null)
                return;

            _lastMoonDayForAnimation = moonDay;

            // Layout not ready yet – defer until SizeChanged
            if (timeLine.Width <= 0 || newMoonDayMarkerGrid.Width <= 0)
            {
                if (!_markerSizeHandlerAttached)
                {
                    _markerSizeHandlerAttached = true;
                    timeLine.SizeChanged += TimeLine_SizeChangedForMarker;
                }
                return;
            }

            // Layout is ready – do the actual animation
            _ = MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (DayAstroEvent == null)
                    return;

                var totalMinutesInDay = 24 * 60;
                var startOffset = 5;   // Adjust based on left margin of the gradient line.
                var endOffset = -10;   // Adjust based on right margin.
                var lineWidthAdjustment = 10;

                var availableWidth = timeLine.Width - (startOffset + endOffset) - lineWidthAdjustment;

                // Triple moon day middle marker
                if (moonDay.IsTripleMoonDay)
                {
                    var middleMoonPosition = CalculateMarkerPosition(
                        moonDay.MiddleMoonDayTransitionTime,
                        totalMinutesInDay,
                        startOffset,
                        availableWidth);

                    await AnimateMarker(middleMoonDayMarkerGrid, middleMoonPosition, secondTimeLabel);
                }

                // New moon marker
                var newMoonPosition = CalculateMarkerPosition(
                    moonDay.TransitionTime,
                    totalMinutesInDay,
                    startOffset,
                    availableWidth);

                await AnimateMarker(newMoonDayMarkerGrid, newMoonPosition, timeLabel);
            });
        }

        /// <summary>
        /// Called once the timeLine finally has size; re-runs the last requested animation.
        /// </summary>
        private void TimeLine_SizeChangedForMarker(object sender, EventArgs e)
        {
            if (timeLine.Width <= 0 || newMoonDayMarkerGrid.Width <= 0 || _lastMoonDayForAnimation == null)
                return;

            timeLine.SizeChanged -= TimeLine_SizeChangedForMarker;
            _markerSizeHandlerAttached = false;

            // Re-run with correct sizes
            AnimateMarkerToPosition(_lastMoonDayForAnimation);
        }

        private double CalculateMarkerPosition(DateTime transitionDateTime, int totalMinutesInDay, double startOffset, double availableWidth)
        {
            var transitionMinutes = transitionDateTime.TimeOfDay.TotalMinutes;
            var positionRatio = transitionMinutes / totalMinutesInDay;
            return startOffset + (availableWidth * positionRatio);
        }

        private async Task AnimateMarker(Grid markerGrid, double targetPositionX, Label label)
        {
            uint animationDuration = 200;

            await markerGrid.TranslateTo(targetPositionX - (markerGrid.Width / 2), 0, animationDuration, Easing.Linear);

            await Task.Delay(300);
            await label.FadeTo(1, 200);
        }

        private async void PrevDateButton_Clicked(object sender, TappedEventArgs e)
        {
            await leftArrow.TranslateTo(-10, 0, 100);
            await leftArrow.TranslateTo(0, 0, 100);
            timeLabel.Opacity = 0;
            secondTimeLabel.Opacity = 0;

            CurrentDate = CurrentDate.AddDays(-1);
            await InitializeDataAsync(CurrentDate);
        }

        private async void NextDateButton_Clicked(object sender, TappedEventArgs e)
        {
            await rightArrow.TranslateTo(10, 0, 100);
            await rightArrow.TranslateTo(0, 0, 100);
            timeLabel.Opacity = 0;
            secondTimeLabel.Opacity = 0;

            CurrentDate = CurrentDate.AddDays(1);
            await InitializeDataAsync(CurrentDate);
        }

        public void UpdateDayEventInfoList()
        {
            if (App.AppData.AppDB.MoonDayDetailsDB == null ||
                App.AppData.AppDB.PlanetInZodiacsDB == null ||
                DayAstroEvent == null)
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

            // Sun
            var sunInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
                p.Planet == Planet.Sun && p.ZodiacSign == DayAstroEvent.SunInZodiac.NewZodiacSign);
            if (sunInfoSourceItem != null)
                AstroDayDetails.SunInZodiacDetails = sunInfoSourceItem.PlanetInZodiacInfo;

            // Moon
            var moonInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
                p.Planet == Planet.Moon && p.ZodiacSign == DayAstroEvent.MoonInZodiac.NewZodiacSign);
            if (moonInfoSourceItem != null)
                AstroDayDetails.MoonInZodiacDetails = moonInfoSourceItem.PlanetInZodiacInfo;

            // Mercury
            var mercuryInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
                p.Planet == Planet.Mercury && p.ZodiacSign == DayAstroEvent.MercuryInZodiac.NewZodiacSign);
            if (mercuryInfoSourceItem != null)
                AstroDayDetails.MercuryInZodiacDetails = mercuryInfoSourceItem.PlanetInZodiacInfo;

            // Venus
            var venusInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
                p.Planet == Planet.Venus && p.ZodiacSign == DayAstroEvent.VenusInZodiac.NewZodiacSign);
            if (venusInfoSourceItem != null)
                AstroDayDetails.VenusInZodiacDetails = venusInfoSourceItem.PlanetInZodiacInfo;

            // Mars
            var marsInfoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
                p.Planet == Planet.Mars && p.ZodiacSign == DayAstroEvent.MarsInZodiac.NewZodiacSign);
            if (marsInfoSourceItem != null)
                AstroDayDetails.MarsInZodiacDetails = marsInfoSourceItem.PlanetInZodiacInfo;

            // ...other planets if needed
        }

        private void UpdateMoonDayInfo()
        {
            if (DayAstroEvent?.MoonDay == null || App.AppData.AppDB.MoonDayDetailsDB == null)
                return;

            // New Moon Day info
            var infoSourceItem = App.AppData.AppDB.MoonDayDetailsDB.FirstOrDefault(m =>
                m.MoonDay == DayAstroEvent.MoonDay.NewMoonDay);
            if (infoSourceItem != null)
            {
                AstroDayDetails.NewMoonDayDetails = infoSourceItem.MoonDayInfo;
            }

            // Previous Moon Day info
            var previousMoonDayItem = App.AppData.AppDB.MoonDayDetailsDB.FirstOrDefault(m =>
                m.MoonDay == DayAstroEvent.MoonDay.PreviousMoonDay);
            if (previousMoonDayItem != null)
            {
                AstroDayDetails.PreviousMoonDayDetails = previousMoonDayItem.MoonDayInfo;
            }

            // Middle Moon Day info (triple moon day)
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
            await ShowMoonDayInfo(infoText, header);
        }

        private async void MoonImage_Tapped(object sender, EventArgs e)
        {
            if (DayAstroEvent == null || AstroDayDetails == null)
                return;

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

            popupOverlay.Opacity = 0;
            popupOverlay.IsVisible = true;

            await popupOverlay.FadeTo(1, 200, Easing.CubicOut);
        }

        private async void HideInfoScreen(object sender, TappedEventArgs e)
        {
            await popupOverlay.FadeTo(0, 200, Easing.CubicIn);
            popupOverlay.IsVisible = false;
            _blurredBitmap = null;
        }

        private async void PlanetInZodiacGrid_Tapped(object sender, EventArgs e)
        {
            if (DayAstroEvent == null || AstroDayDetails == null)
                return;

            if (sender == sunInZodiacGrid)
            {
                string header = TranslationManager.TranslatePlanetInZodiac(Planet.Sun, DayAstroEvent.SunInZodiac.NewZodiacSign);
                string info = AstroDayDetails.SunInZodiacDetails;
                await ToggleMoonGridAnimation(info, header);
            }
            else if (sender == moonInZodiacGrid)
            {
                string header = TranslationManager.TranslatePlanetInZodiac(Planet.Moon, DayAstroEvent.MoonInZodiac.NewZodiacSign);
                string info = AstroDayDetails.MoonInZodiacDetails;
                await ToggleMoonGridAnimation(info, header);
            }
            else if (sender == venusInZodiacGrid)
            {
                string header = TranslationManager.TranslatePlanetInZodiac(Planet.Venus, DayAstroEvent.VenusInZodiac.NewZodiacSign);
                string info = AstroDayDetails.VenusInZodiacDetails;

                if (DayAstroEvent.VenusInZodiac.IsRetrograde == true)
                {
                    string retroInfo = PlanetInRetrogradeDetails
                        .FirstOrDefault(r => r.PlanetInRetrograde == Planet.Venus)?.PlanetInRetrogradeInfo;
                    if (!string.IsNullOrWhiteSpace(retroInfo))
                        info = info + "\n" + retroInfo;
                }

                await ToggleMoonGridAnimation(info, header);
            }
            else if (sender == marsInZodiacGrid)
            {
                string header = TranslationManager.TranslatePlanetInZodiac(Planet.Mars, DayAstroEvent.MarsInZodiac.NewZodiacSign);
                string info = AstroDayDetails.MarsInZodiacDetails;

                if (DayAstroEvent.MarsInZodiac.IsRetrograde == true)
                {
                    string retroInfo = PlanetInRetrogradeDetails
                        .FirstOrDefault(r => r.PlanetInRetrograde == Planet.Mars)?.PlanetInRetrogradeInfo;
                    if (!string.IsNullOrWhiteSpace(retroInfo))
                        info = info + "\n" + retroInfo;
                }

                await ToggleMoonGridAnimation(info, header);
            }
            else if (sender == mercuryInZodiacGrid)
            {
                string header = TranslationManager.TranslatePlanetInZodiac(Planet.Mercury, DayAstroEvent.MercuryInZodiac.NewZodiacSign);
                string info = AstroDayDetails.MercuryInZodiacDetails;

                if (DayAstroEvent.MercuryInZodiac.IsRetrograde == true)
                {
                    string retroInfo = PlanetInRetrogradeDetails
                        .FirstOrDefault(r => r.PlanetInRetrograde == Planet.Mercury)?.PlanetInRetrogradeInfo;
                    if (!string.IsNullOrWhiteSpace(retroInfo))
                        info = info + "\n" + retroInfo;
                }

                await ToggleMoonGridAnimation(info, header);
            }
        }

        private async Task CaptureAndBlurAsync()
        {
            if (_blurredBitmap != null)
            {
                blurCanvas.IsVisible = true;
                blurCanvas.InvalidateSurface();
                return;
            }

            await Task.Delay(16); // small delay to ensure layout is drawn

            IScreenshotResult result = await PageRoot.CaptureAsync();

            if (result == null)
                return;

            await using var stream = await result.OpenReadAsync();

            _blurredBitmap = await Task.Run(() =>
            {
                using var skStream = new SKManagedStream(stream);
                var original = SKBitmap.Decode(skStream);
                if (original == null) return null;

                var blurred = new SKBitmap(original.Width, original.Height);

                using var canvas = new SKCanvas(blurred);
                using var paint = new SKPaint { ImageFilter = SKImageFilter.CreateBlur(4f, 4f) };

                canvas.Clear();
                canvas.DrawBitmap(original, 0, 0, paint);

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
}
