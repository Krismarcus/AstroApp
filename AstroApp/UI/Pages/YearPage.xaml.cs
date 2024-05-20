using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Pages;

public partial class YearPage : ContentPage
{
    public ObservableCollection<AstroEvent> ActiveAstroEvents { get; set; }
    public ObservableCollection<PlanetInZodiac> PlanetInZodiacInfo { get; set; }
    public ObservableCollection<MonthSegment> MonthSegments { get; set; }
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


    public YearPage()
    {
        InitializeComponent();
        Initialize();
        GenerateCalendar();
        this.BindingContext = this;
        SetupSegmentClickHandlers();
    }

    private void Initialize()
    {
        this.ActiveAstroEvents = App.AppData.AppDB.AstroEventsDB;
        this.PlanetInZodiacInfo = App.AppData.AppDB.PlanetInZodiacsDB;
    }

    private void SetupSegmentClickHandlers()
    {
        CustomZodiacLineViewSun.SegmentClicked += OnPlanetInSunZodiacSegmentClicked;
        CustomZodiacLineViewMercury.SegmentClicked += OnPlanetInMercuryZodiacSegmentClicked;
        CustomZodiacLineViewVenus.SegmentClicked += OnPlanetInVenusZodiacSegmentClicked;
        CustomZodiacLineViewMars.SegmentClicked += OnPlanetInMarsZodiacSegmentClicked;
    }

    private void OnPlanetInSunZodiacSegmentClicked(object sender, ZodiacSign sign)
    {
        if (sign == null) return;

        var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Sun && p.NewZodiacSign == sign);

        LabelShowingZodiacSign.Text = infoSourceItem.PlanetInZodiacInfo;
    }

    private void OnPlanetInMercuryZodiacSegmentClicked(object sender, ZodiacSign sign)
    {
        if (sign == null) return;

        var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Mercury && p.NewZodiacSign == sign);

        LabelShowingZodiacSign.Text = infoSourceItem.PlanetInZodiacInfo;
    }

    private void OnPlanetInVenusZodiacSegmentClicked(object sender, ZodiacSign sign)
    {
        if (sign == null) return;

        var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Venus && p.NewZodiacSign == sign);

        LabelShowingZodiacSign.Text = infoSourceItem.PlanetInZodiacInfo;
    }

    private void OnPlanetInMarsZodiacSegmentClicked(object sender, ZodiacSign sign)
    {
        if (sign == null) return;

        var infoSourceItem = App.AppData.AppDB.PlanetInZodiacsDB.FirstOrDefault(p =>
            p.Planet == Planet.Mars && p.NewZodiacSign == sign);

        LabelShowingZodiacSign.Text = infoSourceItem.PlanetInZodiacInfo;
    }

    public void GenerateCalendar()
    {
        this.MonthSegments = new ObservableCollection<MonthSegment>();
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

        bool lastIsMercuryRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.MercuryInZodiac.IsRetrograde ?? false;
        bool lastIsVenusRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.VenusInZodiac.IsRetrograde ?? false;
        bool lastIsMarsRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.MarsInZodiac.IsRetrograde ?? false;
        bool lastIsJupiterRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.JupiterInZodiac.IsRetrograde ?? false;
        bool lastIsSaturnRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.SaturnInZodiac.IsRetrograde ?? false;
        bool lastIsUranusRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.UranusInZodiac.IsRetrograde ?? false;
        bool lastIsNeptuneRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.NeptuneInZodiac.IsRetrograde ?? false;
        bool lastIsPlutoRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.PlutoInZodiac.IsRetrograde ?? false;

        DateTime? startSunDate = null;
        DateTime? startMercuryDate = null;
        DateTime? startVenusDate = null;
        DateTime? startMarsDate = null;
        DateTime? startJupiterDate = null;
        DateTime? startSaturnDate = null;
        DateTime? startUranusDate = null;
        DateTime? startNeptuneDate = null;
        DateTime? startPlutoDate = null;
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

            // Handle Retrograde for Mercury
            if (isMercuryRetrograde != lastIsMercuryRetrograde || startRetrogradeMercuryDate == null)
            {
                if (startRetrogradeMercuryDate != null)
                {
                    RetrogradeMercurySegments.Add(new RetrogradeSegment
                    {
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

        // Add the last segments for retrogrades
        if (startRetrogradeMercuryDate != null)
            RetrogradeMercurySegments.Add(new RetrogradeSegment { IsRetrograde = lastIsMercuryRetrograde, RetrogradeStartDate = startRetrogradeMercuryDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeVenusDate != null)
            RetrogradeVenusSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsVenusRetrograde, RetrogradeStartDate = startRetrogradeVenusDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeMarsDate != null)
            RetrogradeMarsSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsMarsRetrograde, RetrogradeStartDate = startRetrogradeMarsDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeJupiterDate != null)
            RetrogradeJupiterSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsJupiterRetrograde, RetrogradeStartDate = startRetrogradeJupiterDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeSaturnDate != null)
            RetrogradeSaturnSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsSaturnRetrograde, RetrogradeStartDate = startRetrogradeSaturnDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeUranusDate != null)
            RetrogradeUranusSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsUranusRetrograde, RetrogradeStartDate = startRetrogradeUranusDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeNeptuneDate != null)
            RetrogradeNeptuneSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsNeptuneRetrograde, RetrogradeStartDate = startRetrogradeNeptuneDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradePlutoDate != null)
            RetrogradePlutoSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsPlutoRetrograde, RetrogradeStartDate = startRetrogradePlutoDate.Value, RetrogradeEndDate = lastDate });

        // Handle the last month segment
        if (lastMonthStart != null)
            this.MonthSegments.Add(new MonthSegment
            {
                MonthStartDate = lastMonthStart,
                MonthEndDate = lastDate
            });
    }
}
