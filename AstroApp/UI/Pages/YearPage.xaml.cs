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
    public ObservableCollection<RetrogradeSegment> RetrogradeMercurySegments {  get; set; }
    public ObservableCollection<ZodiacSegment> VenusInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeVenusSegments { get; set; }
    public ObservableCollection<ZodiacSegment> MarsInZodiacSegments { get; set; }
    public ObservableCollection<RetrogradeSegment> RetrogradeMarsSegments { get; set; }

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
        this.RetrogradeMercurySegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeVenusSegments = new ObservableCollection<RetrogradeSegment>();
        this.RetrogradeMarsSegments = new ObservableCollection<RetrogradeSegment>();

        // Initialize the start points for each planet
        DateTime lastMonthStart = this.ActiveAstroEvents.FirstOrDefault()?.Date ?? DateTime.Today;
        ZodiacSign lastSunInSign = this.ActiveAstroEvents.FirstOrDefault()?.SunInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMercuryInSign = this.ActiveAstroEvents.FirstOrDefault()?.MercuryInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastVenusInSign = this.ActiveAstroEvents.FirstOrDefault()?.VenusInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMarsInSign = this.ActiveAstroEvents.FirstOrDefault()?.MarsInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        bool lastIsMercuryRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.MercuryInZodiac.IsRetrograde ?? false;
        bool lastIsVenusRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.VenusInZodiac.IsRetrograde ?? false;
        bool lastIsMarsRetrograde = this.ActiveAstroEvents.FirstOrDefault()?.MarsInZodiac.IsRetrograde ?? false;

        DateTime? startSunDate = null;
        DateTime? startMercuryDate = null;
        DateTime? startVenusDate = null;
        DateTime? startMarsDate = null;
        DateTime? startRetrogradeMercuryDate = null;
        DateTime? startRetrogradeVenusDate = null;
        DateTime? startRetrogradeMarsDate = null;

        foreach (var astroEvent in this.ActiveAstroEvents.OrderBy(e => e.Date)) // Ensure events are sorted by date
        {
            ZodiacSign sunInZodiac = astroEvent.SunInZodiac.NewZodiacSign;
            ZodiacSign mercuryInZodiac = astroEvent.MercuryInZodiac.NewZodiacSign;
            ZodiacSign venusInZodiac = astroEvent.VenusInZodiac.NewZodiacSign;
            ZodiacSign marsInZodiac = astroEvent.MarsInZodiac.NewZodiacSign;
            bool isMercuryRetrograde = astroEvent.MercuryInZodiac.IsRetrograde;
            bool isVenusRetrograde = astroEvent.VenusInZodiac.IsRetrograde;
            bool isMarsRetrograde = astroEvent.MarsInZodiac.IsRetrograde;

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

            // Similar logic for other planets
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
        if (startRetrogradeMercuryDate != null)
            RetrogradeMercurySegments.Add(new RetrogradeSegment { IsRetrograde = lastIsMercuryRetrograde, RetrogradeStartDate = startRetrogradeMercuryDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeVenusDate != null)
            RetrogradeVenusSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsVenusRetrograde, RetrogradeStartDate = startRetrogradeVenusDate.Value, RetrogradeEndDate = lastDate });
        if (startRetrogradeMarsDate != null)
            RetrogradeMarsSegments.Add(new RetrogradeSegment { IsRetrograde = lastIsMarsRetrograde, RetrogradeStartDate = startRetrogradeMarsDate.Value, RetrogradeEndDate = lastDate });

        // Handle the last month segment
        if (lastMonthStart != null)
            this.MonthSegments.Add(new MonthSegment
            {                
                MonthStartDate = lastMonthStart,
                MonthEndDate = lastDate
            });
    }

}