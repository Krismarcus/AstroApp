using Astrodaiva.Data.Enums;
using Astrodaiva.Data.Models;
using Astrodaiva.Services;
using Astrodaiva.UI.Controls;
using Astrodaiva.UI.Tools;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Astrodaiva.UI.Pages;

public partial class YearPage : ContentPage
{
    private readonly IOrientationService _orientation;
    public ObservableCollection<AstroEvent> ActiveAstroEvents { get; set; }
    public ObservableCollection<PlanetInZodiacDetails> PlanetInZodiacInfo { get; set; }
    public ObservableCollection<MonthSegment> MonthSegments { get; set; }
    public ObservableCollection<EclipseSegment> EclipseSegments { get; set; }
    public ObservableCollection<ZodiacSegment> SunInZodiacSegments { get; set; }
    public ObservableCollection<ZodiacSegment> MercuryInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeMercurySegments { get; set; }
    public ObservableCollection<ZodiacSegment> VenusInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeVenusSegments { get; set; }
    public ObservableCollection<ZodiacSegment> MarsInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeMarsSegments { get; set; }
    public ObservableCollection<ZodiacSegment> JupiterInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeJupiterSegments { get; set; }
    public ObservableCollection<ZodiacSegment> SaturnInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeSaturnSegments { get; set; }
    public ObservableCollection<ZodiacSegment> UranusInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeUranusSegments { get; set; }
    public ObservableCollection<ZodiacSegment> NeptuneInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeNeptuneSegments { get; set; }
    public ObservableCollection<ZodiacSegment> PlutoInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradePlutoSegments { get; set; }
    public ObservableCollection<ZodiacSegment> SelenaInZodiacSegments { get; set; }
    public ObservableCollection<ZodiacSegment> LilithInZodiacSegments { get; set; }
    public ObservableCollection<ZodiacSegment> RahuInZodiacSegments { get; set; }
    public ObservableCollection<ZodiacSegment> KetuInZodiacSegments { get; set; }

    public YearPage()
    {
        InitializeComponent();
        _orientation = ServiceHelper.GetRequiredService<IOrientationService>();
        Initialize();
        GenerateCalendar();        
        this.BindingContext = this;
        EclipseView.Segments = EclipseSegments;
        SetupSegmentClickHandlers();
        SetupFrameGestureHandlers();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _orientation?.LockLandscape();
    }

    protected override void OnDisappearing()
    {
        _orientation?.Unlock();
        base.OnDisappearing();
    }

    private void Initialize()
    {
        this.ActiveAstroEvents = App.AppData.AppDB.AstroEventsDB;
        var currentYear = DateTime.Now.Year;

        // Filter entries from January to December of the current year
        this.ActiveAstroEvents = new ObservableCollection<AstroEvent>(
            this.ActiveAstroEvents.Where(e => e.Date.Year == currentYear && e.Date.Month >= 1 && e.Date.Month <= 12)
        );
        this.PlanetInZodiacInfo = App.AppData.AppDB.PlanetInZodiacsDB;
    }

    private void SetupSegmentClickHandlers()
    {
        CustomZodiacLineViewSun.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Sun);
        CustomZodiacLineViewMercury.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Mercury);
        CustomZodiacLineViewVenus.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Venus);
        CustomZodiacLineViewMars.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Mars);
        CustomZodiacLineViewJupiter.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Jupiter);
        CustomZodiacLineViewSaturn.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Saturn);
        CustomZodiacLineViewUranus.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Uranus);
        CustomZodiacLineViewNeptune.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Neptune);
        CustomZodiacLineViewPluto.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Pluto);

        // Selena, Lilith, Rahu, Ketu
        CustomZodiacLineViewSelena.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Selena);
        CustomZodiacLineViewLilith.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Lilith);
        CustomZodiacLineViewRahu.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Rahu);
        CustomZodiacLineViewKetu.SegmentClicked += (sender, segment) => OnPlanetInZodiacSegmentClicked(sender, segment, Planet.Ketu);

        // Retrogrades
        CustomRetrogradeLineViewMercury.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Mercury);
        CustomRetrogradeLineViewVenus.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Venus);
        CustomRetrogradeLineViewMars.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Mars);
        CustomRetrogradeLineViewJupiter.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Jupiter);
        CustomRetrogradeLineViewSaturn.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Saturn);
        CustomRetrogradeLineViewUranus.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Uranus);
        CustomRetrogradeLineViewNeptune.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Neptune);
        CustomRetrogradeLineViewPluto.SegmentClicked += (sender, segment) => OnPlanetInRetrogradeSegmentClicked(sender, segment, Planet.Pluto);

        EclipseView.EclipseLineTapped += OnEclipseLineTapped;
        EclipseView.MarkerTapped += OnEclipseClicked;       

    }

    private void SetupFrameGestureHandlers()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) => {
            // Hide the frame with animation
            await BottomInfoFrame.TranslateTo(0, BottomInfoFrame.Height, 250, Easing.CubicIn);
            BottomInfoFrame.IsVisible = false;            
        };
        BottomInfoFrame.GestureRecognizers.Add(tapGesture);

        var panGesture = new PanGestureRecognizer();
        double yTranslation = 0;

        panGesture.PanUpdated += async (s, e) => {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    yTranslation = Math.Max(0, yTranslation + e.TotalY);
                    BottomInfoFrame.TranslationY = yTranslation;
                    break;
                case GestureStatus.Completed:
                    if (yTranslation > BottomInfoFrame.Height / 2)
                    {
                        await BottomInfoFrame.TranslateTo(0, BottomInfoFrame.Height, 250, Easing.CubicIn);
                        BottomInfoFrame.IsVisible = false;
                        SegmentSelectionManager.Instance.ClearSelection();
                    }
                    else
                    {
                        await BottomInfoFrame.TranslateTo(0, 0, 250, Easing.CubicOut);
                    }
                    yTranslation = 0;
                    break;
            }
        };
        BottomInfoFrame.GestureRecognizers.Add(panGesture);
    }

    private async void OnPlanetInZodiacSegmentClicked(object sender, ZodiacSegment segment, Planet planet)
    {
        if (segment == null) return;        

        var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == planet && p.ZodiacSign == segment.ZodiacSign);

        LabelDateRangeRow.IsVisible = true;
        PlanetInZodiacLabel.Text = TranslationManager.TranslatePlanetInZodiac(planet, segment.ZodiacSign);
        LabelShowingStartDate.Text = " (nuo " + segment.ZodiacStartDate.ToString("MMMM d, HH:mm", App.AppData.CultureInfo);
        LabelShoingEndDate.Text = " iki " + segment.ZodiacEndDate.ToString("MMMM d, HH:mm", App.AppData.CultureInfo) + ")";
        LabelShowingPlanetInZodiacInfo.Text = infoSourceItem?.PlanetInZodiacInfo ?? "No information available.";

        // Show the overlay frame with animation
        if (!BottomInfoFrame.IsVisible)
        {
            BottomInfoFrame.IsVisible = true;
            BottomInfoFrame.TranslationY = BottomInfoFrame.Height;
            await BottomInfoFrame.TranslateTo(0, 0, 250, Easing.CubicOut);
        }
    }

    private async void OnPlanetInRetrogradeSegmentClicked(object sender, RetrogradeSegment segment, Planet planet)
    {       
        if (segment == null) return;

        var infoSourceItem = App.AppData.AppDB.PlanetInRetrogradeDetailsDB.FirstOrDefault(p =>
            p.PlanetInRetrograde == planet);

        LabelDateRangeRow.IsVisible = true;
        PlanetInZodiacLabel.Text = TranslationManager.TranslatePlanetInRetrograde(planet);
        LabelShowingStartDate.Text = " (nuo " + segment.RetrogradeStartDate.ToString("MMMM d, HH:mm", App.AppData.CultureInfo);
        LabelShoingEndDate.Text = " iki " + segment.RetrogradeEndDate.ToString("MMMM d, HH:mm", App.AppData.CultureInfo) + ")";
        LabelShowingPlanetInZodiacInfo.Text = infoSourceItem?.PlanetInRetrogradeInfo ?? "No information available.";

        // Show the overlay frame with animation
        if (!BottomInfoFrame.IsVisible)
        {
            BottomInfoFrame.IsVisible = true;
            BottomInfoFrame.TranslationY = BottomInfoFrame.Height;
            await BottomInfoFrame.TranslateTo(0, 0, 250, Easing.CubicOut);
        }
    }

    private async void OnEclipseLineTapped(List<EclipseSegment> list)
    {
        SegmentSelectionManager.Instance.SelectSegment(null); // optional un-highlight markers if desired

        LabelDateRangeRow.IsVisible = false;
        PlanetInZodiacLabel.Text = $"Užtemimai {DateTime.Now.Year} metams";
        LabelShowingStartDate.Text = "";
        LabelShoingEndDate.Text = "";

        LabelShowingPlanetInZodiacInfo.Text =
            string.Join("\n", list.Select(e =>
                (e.IsSolar ? "☀" : "🌑") + " " + e.StartDate.ToString("MMMM d", App.AppData.CultureInfo)
            ));

        if (!BottomInfoFrame.IsVisible)
        {
            BottomInfoFrame.IsVisible = true;
            BottomInfoFrame.TranslationY = BottomInfoFrame.Height;
            await BottomInfoFrame.TranslateTo(0, 0, 250, Easing.CubicOut);
        }
    }

    private async void OnEclipseClicked(EclipseSegment seg)
    {
        LabelDateRangeRow.IsVisible = true;
        PlanetInZodiacLabel.Text = seg.IsSolar ? "☀ Saulės užtemimas" : "🌑 Mėnulio užtemimas";

        LabelShowingStartDate.Text = " (" + seg.EndDate.ToString("MMMM d, dddd", App.AppData.CultureInfo) + ")";
        LabelShoingEndDate.Text = "";

        LabelShowingPlanetInZodiacInfo.Text =
            $"Užtemimo periodas. Poveikis jaučiamas ~7 d. prieš ir po.";

        if (!BottomInfoFrame.IsVisible)
        {
            BottomInfoFrame.IsVisible = true;
            BottomInfoFrame.TranslationY = BottomInfoFrame.Height;
            await BottomInfoFrame.TranslateTo(0, 0, 250, Easing.CubicOut);
        }
    }


    public void GenerateCalendar()
    {
        this.MonthSegments = new ObservableCollection<MonthSegment>();
        this.EclipseSegments = new ObservableCollection<EclipseSegment>();
        this.SunInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.MercuryInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.VenusInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.MarsInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.JupiterInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.SaturnInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.UranusInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.NeptuneInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.PlutoInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.RetrogradeMercurySegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeVenusSegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeMarsSegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeJupiterSegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeSaturnSegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeUranusSegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeNeptuneSegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradePlutoSegments = new ObservableCollection<RetrogradeSegment>();
        this.SelenaInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.LilithInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.RahuInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.KetuInZodiacSegments = new ObservableCollection<ZodiacSegment>();        

        // Initialize the start points for each planet
        DateTime lastMonthStart = this.ActiveAstroEvents.FirstOrDefault()?.Date ?? DateTime.Today;
        ZodiacSign lastSunInSign = this.ActiveAstroEvents.FirstOrDefault()?.SunInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMercuryInSign = this.ActiveAstroEvents.FirstOrDefault()?.MercuryInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastVenusInSign = this.ActiveAstroEvents.FirstOrDefault()?.VenusInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMarsInSign = this.ActiveAstroEvents.FirstOrDefault()?.MarsInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastJupiterInSign = this.ActiveAstroEvents.FirstOrDefault()?.JupiterInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastSaturnInSign = this.ActiveAstroEvents.FirstOrDefault()?.SaturnInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastUranusInSign = this.ActiveAstroEvents.FirstOrDefault()?.UranusInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastNeptuneInSign = this.ActiveAstroEvents.FirstOrDefault()?.NeptuneInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastPlutoInSign = this.ActiveAstroEvents.FirstOrDefault()?.PlutoInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastSelenaInSign = this.ActiveAstroEvents.FirstOrDefault()?.SelenaInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastLilithInSign = this.ActiveAstroEvents.FirstOrDefault()?.LilithInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastRahuInSign = this.ActiveAstroEvents.FirstOrDefault()?.RahuInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastKetuInSign = this.ActiveAstroEvents.FirstOrDefault()?.KetuInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;

        bool lastIsMercuryRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.MercuryInZodiac.IsRetrograde ?? false;
        bool lastIsVenusRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.VenusInZodiac.IsRetrograde ?? false;
        bool lastIsMarsRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.MarsInZodiac.IsRetrograde ?? false;
        bool lastIsJupiterRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.JupiterInZodiac.IsRetrograde ?? false;
        bool lastIsSaturnRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.SaturnInZodiac.IsRetrograde ?? false;
        bool lastIsUranusRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.UranusInZodiac.IsRetrograde ?? false;
        bool lastIsNeptuneRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.NeptuneInZodiac.IsRetrograde ?? false;
        bool lastIsPlutoRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.PlutoInZodiac.IsRetrograde ?? false;

        DateTime? eclipseStart = null;
        bool eclipseActive = false;
        bool eclipseSolar = false;
        bool eclipseLunar = false;
        DateTime? startSunDate = null;
        DateTime? startMercuryDate = null;
        DateTime? startVenusDate = null;
        DateTime? startMarsDate = null;
        DateTime? startJupiterDate = null;
        DateTime? startSaturnDate = null;
        DateTime? startUranusDate = null;
        DateTime? startNeptuneDate = null;
        DateTime? startPlutoDate = null;
        DateTime? startSelenaDate = null;
        DateTime? startLilithDate = null;
        DateTime? startRahuDate = null;
        DateTime? startKetuDate = null;
        DateTime? startRetrogradeMercuryDate = null;
        DateTime? startRetrogradeVenusDate = null;
        DateTime? startRetrogradeMarsDate = null;
        DateTime? startRetrogradeJupiterDate = null;
        DateTime? startRetrogradeSaturnDate = null;
        DateTime? startRetrogradeUranusDate = null;
        DateTime? startRetrogradeNeptuneDate = null;
        DateTime? startRetrogradePlutoDate = null;

        foreach (var astroEvent in this.ActiveAstroEvents.OrderBy(e => e.Date)) // Ensure events are sorted by date
        {
            ZodiacSign sunInZodiac = astroEvent.SunInZodiac.NewZodiacSign;
            ZodiacSign mercuryInZodiac = astroEvent.MercuryInZodiac.NewZodiacSign;
            ZodiacSign venusInZodiac = astroEvent.VenusInZodiac.NewZodiacSign;
            ZodiacSign marsInZodiac = astroEvent.MarsInZodiac.NewZodiacSign;
            ZodiacSign jupiterInZodiac = astroEvent.JupiterInZodiac.NewZodiacSign;
            ZodiacSign saturnInZodiac = astroEvent.SaturnInZodiac.NewZodiacSign;
            ZodiacSign uranusInZodiac = astroEvent.UranusInZodiac.NewZodiacSign;
            ZodiacSign neptuneInZodiac = astroEvent.NeptuneInZodiac.NewZodiacSign;
            ZodiacSign plutoInZodiac = astroEvent.PlutoInZodiac.NewZodiacSign;
            ZodiacSign selenaInZodiac = astroEvent.SelenaInZodiac.NewZodiacSign;
            ZodiacSign lilitInZodiac = astroEvent.LilithInZodiac.NewZodiacSign;
            ZodiacSign rahuInZodiac = astroEvent.RahuInZodiac.NewZodiacSign;
            ZodiacSign ketuInZodiac = astroEvent.KetuInZodiac.NewZodiacSign;

            bool isMercuryRetrograde = astroEvent.MercuryInZodiac.IsRetrograde;
            bool isVenusRetrograde = astroEvent.VenusInZodiac.IsRetrograde;
            bool isMarsRetrograde = astroEvent.MarsInZodiac.IsRetrograde;
            bool isJupiterRetrograde = astroEvent.JupiterInZodiac.IsRetrograde;
            bool isSaturnRetrograde = astroEvent.SaturnInZodiac.IsRetrograde;
            bool isUranusRetrograde = astroEvent.UranusInZodiac.IsRetrograde;
            bool isNeptuneRetrograde = astroEvent.NeptuneInZodiac.IsRetrograde;
            bool isPlutoRetrograde = astroEvent.PlutoInZodiac.IsRetrograde;

            // Create month segments
            if (astroEvent.Date.Month != lastMonthStart.Month)
            {
                this.MonthSegments.Add(new MonthSegment
                {
                    MonthStartDate = lastMonthStart,
                    MonthEndDate = new DateTime(astroEvent.Date.Year, astroEvent.Date.Month, 1).AddDays(-1)
                });
                lastMonthStart = new DateTime(astroEvent.Date.Year, astroEvent.Date.Month, 1);
            }

            bool todaySolar = astroEvent.SunEclipse;
            bool todayLunar = astroEvent.MoonEclipse;

            // Eclipse START
            if ((todaySolar || todayLunar) && !eclipseActive)
            {
                eclipseActive = true;
                eclipseStart = astroEvent.Date;
                eclipseSolar = todaySolar;
                eclipseLunar = todayLunar;
            }

            // Eclipse END
            if (!(todaySolar || todayLunar) && eclipseActive)
            {
                EclipseSegments.Add(new EclipseSegment
                {
                    StartDate = eclipseStart.Value,
                    EndDate = astroEvent.Date.AddDays(-1),
                    IsSolar = eclipseSolar,
                    IsLunar = eclipseLunar
                });

                eclipseActive = false;
            }

            // Create segments for Sun in Zodiac
            if (sunInZodiac != lastSunInSign || startSunDate == null)
            {
                if (startSunDate != null)
                {
                    SunInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastSunInSign,
                        ZodiacStartDate = startSunDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startSunDate.Value).Days
                    });
                }
                startSunDate = astroEvent.Date;
                lastSunInSign = sunInZodiac;
            }

            // Create segments for Mercury in Zodiac
            if (mercuryInZodiac != lastMercuryInSign || startMercuryDate == null)
            {
                if (startMercuryDate != null)
                {
                    MercuryInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastMercuryInSign,
                        ZodiacStartDate = startMercuryDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startMercuryDate.Value).Days
                    });
                }
                startMercuryDate = astroEvent.Date;
                lastMercuryInSign = mercuryInZodiac;
            }

            // Create segments for Venus in Zodiac
            if (venusInZodiac != lastVenusInSign || startVenusDate == null)
            {
                if (startVenusDate != null)
                {
                    VenusInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastVenusInSign,
                        ZodiacStartDate = startVenusDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startVenusDate.Value).Days
                    });
                }
                startVenusDate = astroEvent.Date;
                lastVenusInSign = venusInZodiac;
            }

            // Create segments for Mars in Zodiac
            if (marsInZodiac != lastMarsInSign || startMarsDate == null)
            {
                if (startMarsDate != null)
                {
                    MarsInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastMarsInSign,
                        ZodiacStartDate = startMarsDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startMarsDate.Value).Days
                    });
                }
                startMarsDate = astroEvent.Date;
                lastMarsInSign = marsInZodiac;
            }

            // Create segments for Jupiter in Zodiac
            if (jupiterInZodiac != lastJupiterInSign || startJupiterDate == null)
            {
                if (startJupiterDate != null)
                {
                    JupiterInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastJupiterInSign,
                        ZodiacStartDate = startJupiterDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startJupiterDate.Value).Days
                    });
                }
                startJupiterDate = astroEvent.Date;
                lastJupiterInSign = jupiterInZodiac;
            }

            // Create segments for Saturn in Zodiac
            if (saturnInZodiac != lastSaturnInSign || startSaturnDate == null)
            {
                if (startSaturnDate != null)
                {
                    SaturnInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastSaturnInSign,
                        ZodiacStartDate = startSaturnDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startSaturnDate.Value).Days
                    });
                }
                startSaturnDate = astroEvent.Date;
                lastSaturnInSign = saturnInZodiac;
            }

            // Create segments for Uranus in Zodiac
            if (uranusInZodiac != lastUranusInSign || startUranusDate == null)
            {
                if (startUranusDate != null)
                {
                    UranusInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastUranusInSign,
                        ZodiacStartDate = startUranusDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startUranusDate.Value).Days
                    });
                }
                startUranusDate = astroEvent.Date;
                lastUranusInSign = uranusInZodiac;
            }

            // Create segments for Neptune in Zodiac
            if (neptuneInZodiac != lastNeptuneInSign || startNeptuneDate == null)
            {
                if (startNeptuneDate != null)
                {
                    NeptuneInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastNeptuneInSign,
                        ZodiacStartDate = startNeptuneDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startNeptuneDate.Value).Days
                    });
                }
                startNeptuneDate = astroEvent.Date;
                lastNeptuneInSign = neptuneInZodiac;
            }

            // Create segments for Pluto in Zodiac
            if (plutoInZodiac != lastPlutoInSign || startPlutoDate == null)
            {
                if (startPlutoDate != null)
                {
                    PlutoInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastPlutoInSign,
                        ZodiacStartDate = startPlutoDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startPlutoDate.Value).Days
                    });
                }
                startPlutoDate = astroEvent.Date;
                lastPlutoInSign = plutoInZodiac;
            }

            // Create segments for Selena in Zodiac
            if (selenaInZodiac != lastSelenaInSign || startSelenaDate == null)
            {
                if (startSelenaDate != null)
                {
                    SelenaInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastSelenaInSign,
                        ZodiacStartDate = startSelenaDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startSelenaDate.Value).Days
                    });
                }
                startSelenaDate = astroEvent.Date;
                lastSelenaInSign = selenaInZodiac;
            }

            // Create segments for Lilith in Zodiac
            if (lilitInZodiac != lastLilithInSign || startLilithDate == null)
            {
                if (startLilithDate != null)
                {
                    LilithInZodiacSegments.Add(new ZodiacSegment
                    {                   
                        ZodiacSign = lastLilithInSign,
                        ZodiacStartDate = startLilithDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startLilithDate.Value).Days
                    });
                }
                startLilithDate = astroEvent.Date;
                lastLilithInSign = lilitInZodiac;
            }

            // Create segments for Rahu in Zodiac
            if (rahuInZodiac != lastRahuInSign || startRahuDate == null)
            {
                if (startRahuDate != null)
                {
                    RahuInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastRahuInSign,
                        ZodiacStartDate = startRahuDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRahuDate.Value).Days
                    });
                }
                startRahuDate = astroEvent.Date;
                lastRahuInSign = rahuInZodiac;
            }

            // Create segments for Ketu in Zodiac
            if (ketuInZodiac != lastKetuInSign || startKetuDate == null)
            {
                if (startKetuDate != null)
                {
                    KetuInZodiacSegments.Add(new ZodiacSegment
                    {
                        ZodiacSign = lastKetuInSign,
                        ZodiacStartDate = startKetuDate.Value,
                        ZodiacEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startKetuDate.Value).Days
                    });
                }
                startKetuDate = astroEvent.Date;
                lastKetuInSign = ketuInZodiac;
            }

            // Handle Retrograde for Mercury
            if (isMercuryRetrograde != lastIsMercuryRetrograde || startRetrogradeMercuryDate == null)
            {
                if (startRetrogradeMercuryDate != null)
                {
                    RetrogradeMercurySegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Mercury,
                        IsRetrograde = lastIsMercuryRetrograde,
                        RetrogradeStartDate = startRetrogradeMercuryDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradeMercuryDate.Value).Days
                    });
                }
                startRetrogradeMercuryDate = astroEvent.Date;
                lastIsMercuryRetrograde = isMercuryRetrograde;
            }

            // Handle Retrograde for Venus
            if (isVenusRetrograde != lastIsVenusRetrograde || startRetrogradeVenusDate == null)
            {
                if (startRetrogradeVenusDate != null)
                {
                    RetrogradeVenusSegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Venus,
                        IsRetrograde = lastIsVenusRetrograde,
                        RetrogradeStartDate = startRetrogradeVenusDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradeVenusDate.Value).Days
                    });
                }
                startRetrogradeVenusDate = astroEvent.Date;
                lastIsVenusRetrograde = isVenusRetrograde;
            }

            // Handle Retrograde for Mars
            if (isMarsRetrograde != lastIsMarsRetrograde || startRetrogradeMarsDate == null)
            {
                if (startRetrogradeMarsDate != null)
                {
                    RetrogradeMarsSegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Mars,
                        IsRetrograde = lastIsMarsRetrograde,
                        RetrogradeStartDate = startRetrogradeMarsDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradeMarsDate.Value).Days
                    });
                }
                startRetrogradeMarsDate = astroEvent.Date;
                lastIsMarsRetrograde = isMarsRetrograde;
            }

            // Handle Retrograde for Jupiter
            if (isJupiterRetrograde != lastIsJupiterRetrograde || startRetrogradeJupiterDate == null)
            {
                if (startRetrogradeJupiterDate != null)
                {
                    RetrogradeJupiterSegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Jupiter,
                        IsRetrograde = lastIsJupiterRetrograde,
                        RetrogradeStartDate = startRetrogradeJupiterDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradeJupiterDate.Value).Days
                    });
                }
                startRetrogradeJupiterDate = astroEvent.Date;
                lastIsJupiterRetrograde = isJupiterRetrograde;
            }

            // Handle Retrograde for Saturn
            if (isSaturnRetrograde != lastIsSaturnRetrograde || startRetrogradeSaturnDate == null)
            {
                if (startRetrogradeSaturnDate != null)
                {
                    RetrogradeSaturnSegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Saturn,
                        IsRetrograde = lastIsSaturnRetrograde,
                        RetrogradeStartDate = startRetrogradeSaturnDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradeSaturnDate.Value).Days
                    });
                }
                startRetrogradeSaturnDate = astroEvent.Date;
                lastIsSaturnRetrograde = isSaturnRetrograde;
            }

            // Handle Retrograde for Uranus
            if (isUranusRetrograde != lastIsUranusRetrograde || startRetrogradeUranusDate == null)
            {
                if (startRetrogradeUranusDate != null)
                {
                    RetrogradeUranusSegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Uranus,
                        IsRetrograde = lastIsUranusRetrograde,
                        RetrogradeStartDate = startRetrogradeUranusDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradeUranusDate.Value).Days
                    });
                }
                startRetrogradeUranusDate = astroEvent.Date;
                lastIsUranusRetrograde = isUranusRetrograde;
            }

            // Handle Retrograde for Neptune
            if (isNeptuneRetrograde != lastIsNeptuneRetrograde || startRetrogradeNeptuneDate == null)
            {
                if (startRetrogradeNeptuneDate != null)
                {
                    RetrogradeNeptuneSegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Neptune,
                        IsRetrograde = lastIsNeptuneRetrograde,
                        RetrogradeStartDate = startRetrogradeNeptuneDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradeNeptuneDate.Value).Days
                    });
                }
                startRetrogradeNeptuneDate = astroEvent.Date;
                lastIsNeptuneRetrograde = isNeptuneRetrograde;
            }

            // Handle Retrograde for Pluto
            if (isPlutoRetrograde != lastIsPlutoRetrograde || startRetrogradePlutoDate == null)
            {
                if (startRetrogradePlutoDate != null)
                {
                    RetrogradePlutoSegments.Add(new RetrogradeSegment
                    {
                        Planet = Planet.Pluto,
                        IsRetrograde = lastIsPlutoRetrograde,
                        RetrogradeStartDate = startRetrogradePlutoDate.Value,
                        RetrogradeEndDate = astroEvent.Date.AddDays(-1),
                        Duration = (astroEvent.Date.AddDays(-1) - startRetrogradePlutoDate.Value).Days
                    });
                }
                startRetrogradePlutoDate = astroEvent.Date;
                lastIsPlutoRetrograde = isPlutoRetrograde;
            }
        }

        // Add the last segments for each tracking
        var lastDate = this.ActiveAstroEvents.LastOrDefault()?.Date ?? DateTime.Today;
        if (eclipseActive && eclipseStart.HasValue)
        {
            EclipseSegments.Add(new EclipseSegment
            {
                StartDate = eclipseStart.Value,
                EndDate = lastDate,
                IsSolar = eclipseSolar,
                IsLunar = eclipseLunar
            });
        }
        if (startSunDate != null)
            SunInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastSunInSign, ZodiacStartDate = startSunDate.Value, ZodiacEndDate = lastDate });
        if (startMercuryDate != null)
            MercuryInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastMercuryInSign, ZodiacStartDate = startMercuryDate.Value, ZodiacEndDate = lastDate });
        if (startVenusDate != null)
            VenusInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastVenusInSign, ZodiacStartDate = startVenusDate.Value, ZodiacEndDate = lastDate });
        if (startMarsDate != null)
            MarsInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastMarsInSign, ZodiacStartDate = startMarsDate.Value, ZodiacEndDate = lastDate });
        if (startJupiterDate != null)
            JupiterInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastJupiterInSign, ZodiacStartDate = startJupiterDate.Value, ZodiacEndDate = lastDate });
        if (startSaturnDate != null)
            SaturnInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastSaturnInSign, ZodiacStartDate = startSaturnDate.Value, ZodiacEndDate = lastDate });
        if (startUranusDate != null)
            UranusInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastUranusInSign, ZodiacStartDate = startUranusDate.Value, ZodiacEndDate = lastDate });
        if (startNeptuneDate != null)
            NeptuneInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastNeptuneInSign, ZodiacStartDate = startNeptuneDate.Value, ZodiacEndDate = lastDate });
        if (startPlutoDate != null)
            PlutoInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastPlutoInSign, ZodiacStartDate = startPlutoDate.Value, ZodiacEndDate = lastDate });
        if (startSelenaDate != null)
            SelenaInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastSelenaInSign, ZodiacStartDate = startSelenaDate.Value, ZodiacEndDate = lastDate });
        if (startLilithDate != null)
            LilithInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastLilithInSign, ZodiacStartDate = startLilithDate.Value, ZodiacEndDate = lastDate });
        if (startRahuDate != null)
            RahuInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastRahuInSign, ZodiacStartDate = startRahuDate.Value, ZodiacEndDate = lastDate });
        if (startKetuDate != null)
            KetuInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastKetuInSign, ZodiacStartDate = startKetuDate.Value, ZodiacEndDate = lastDate });

        // Add the last segments for retrogrades
        if (startRetrogradeMercuryDate != null)
            RetrogradeMercurySegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Mercury,
                IsRetrograde = lastIsMercuryRetrograde,
                RetrogradeStartDate = startRetrogradeMercuryDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradeMercuryDate.Value).Days
            });
        if (startRetrogradeVenusDate != null)
            RetrogradeVenusSegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Venus,
                IsRetrograde = lastIsVenusRetrograde,
                RetrogradeStartDate = startRetrogradeVenusDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradeVenusDate.Value).Days
            });
        if (startRetrogradeMarsDate != null)
            RetrogradeMarsSegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Mars,
                IsRetrograde = lastIsMarsRetrograde,
                RetrogradeStartDate = startRetrogradeMarsDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradeMarsDate.Value).Days
            });
        if (startRetrogradeJupiterDate != null)
            RetrogradeJupiterSegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Jupiter,
                IsRetrograde = lastIsJupiterRetrograde,
                RetrogradeStartDate = startRetrogradeJupiterDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradeJupiterDate.Value).Days
            });
        if (startRetrogradeSaturnDate != null)
            RetrogradeSaturnSegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Saturn,
                IsRetrograde = lastIsSaturnRetrograde,
                RetrogradeStartDate = startRetrogradeSaturnDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradeSaturnDate.Value).Days
            });
        if (startRetrogradeUranusDate != null)
            RetrogradeUranusSegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Uranus,
                IsRetrograde = lastIsUranusRetrograde,
                RetrogradeStartDate = startRetrogradeUranusDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradeUranusDate.Value).Days
            });
        if (startRetrogradeNeptuneDate != null)
            RetrogradeNeptuneSegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Neptune,
                IsRetrograde = lastIsNeptuneRetrograde,
                RetrogradeStartDate = startRetrogradeNeptuneDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradeNeptuneDate.Value).Days
            });
        if (startRetrogradePlutoDate != null)
            RetrogradePlutoSegments.Add(new RetrogradeSegment
            {
                Planet = Planet.Pluto,
                IsRetrograde = lastIsPlutoRetrograde,
                RetrogradeStartDate = startRetrogradePlutoDate.Value,
                RetrogradeEndDate = lastDate,
                Duration = (lastDate - startRetrogradePlutoDate.Value).Days
            });

// Handle the last month segment
        if (lastMonthStart != null)
            this.MonthSegments.Add(new MonthSegment
            {
                MonthStartDate = lastMonthStart,
                MonthEndDate = lastDate
            });
    }
}