using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Pages;

public partial class YearPage : ContentPage
{
    public ObservableCollection<AstroEvent> ActiveAstroEvents { get; set; }
    public ObservableCollection<ZodiacSegment> SunInZodiacSegments { get; set; }
    public ObservableCollection<ZodiacSegment> MercuryInZodiacSegments { get; set; }
    public ObservableCollection<ZodiacSegment> VenusInZodiacSegments { get; set; }
    public ObservableCollection<ZodiacSegment> MarsInZodiacSegments { get; set; }

    public YearPage()
    {
        InitializeComponent();
        Initialize();
        GenerateCalendar();
        this.BindingContext = this;
    }

    private void Initialize()
    {
        this.ActiveAstroEvents = App.AppData.AppDB.AstroEventsDB;
    }

    public void GenerateCalendar()
    {
        this.SunInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.MercuryInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.VenusInZodiacSegments = new ObservableCollection<ZodiacSegment>();
        this.MarsInZodiacSegments = new ObservableCollection<ZodiacSegment>();

        // Initialize lastSign variables to handle the very first segment
        ZodiacSign lastSunInSign = this.ActiveAstroEvents.FirstOrDefault()?.SunInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMercuryInSign = this.ActiveAstroEvents.FirstOrDefault()?.MercuryInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastVenusInSign = this.ActiveAstroEvents.FirstOrDefault()?.VenusInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMarsInSign = this.ActiveAstroEvents.FirstOrDefault()?.MarsInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;

        DateTime? startSunDate = null;
        DateTime? startMercuryDate = null;
        DateTime? startVenusDate = null;
        DateTime? startMarsDate = null;

        foreach (var astroEvent in this.ActiveAstroEvents.OrderBy(e => e.Date))  // Ensure events are sorted by date
        {
            ZodiacSign sunInZodiac = astroEvent.SunInZodiac.NewZodiacSign;
            ZodiacSign mercuryInZodiac = astroEvent.MercuryInZodiac.NewZodiacSign;
            ZodiacSign venusInZodiac = astroEvent.VenusInZodiac.NewZodiacSign;
            ZodiacSign marsInZodiac = astroEvent.MarsInZodiac.NewZodiacSign;

            if (sunInZodiac != lastSunInSign || startSunDate == null)
            {
                if (startSunDate != null)
                {
                    var endDate = astroEvent.Date.AddDays(-1);
                    SunInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastSunInSign, ZodiacStartDate = startSunDate.Value, ZodiacEndDate = endDate });
                }
                startSunDate = astroEvent.Date;
                lastSunInSign = sunInZodiac;
            }

            if (mercuryInZodiac != lastMercuryInSign || startMercuryDate == null)
            {
                if (startMercuryDate != null)
                {
                    var endDate = astroEvent.Date.AddDays(-1);
                    MercuryInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastMercuryInSign, ZodiacStartDate = startMercuryDate.Value, ZodiacEndDate = endDate });
                }
                startMercuryDate = astroEvent.Date;
                lastMercuryInSign = mercuryInZodiac;
            }

            if (venusInZodiac != lastVenusInSign || startVenusDate == null)
            {
                if (startVenusDate != null)
                {
                    var endDate = astroEvent.Date.AddDays(-1);
                    VenusInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastVenusInSign, ZodiacStartDate = startVenusDate.Value, ZodiacEndDate = endDate });
                }
                startVenusDate = astroEvent.Date;
                lastVenusInSign = venusInZodiac;
            }

            if (marsInZodiac != lastMarsInSign || startMarsDate == null)
            {
                if (startMarsDate != null)
                {
                    var endDate = astroEvent.Date.AddDays(-1);
                    MarsInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastMarsInSign, ZodiacStartDate = startMarsDate.Value, ZodiacEndDate = endDate });
                }
                startMarsDate = astroEvent.Date;
                lastMarsInSign = marsInZodiac;
            }
        }

        // Add the last segments for each planet
        var lastDate = this.ActiveAstroEvents.LastOrDefault()?.Date ?? DateTime.Today;
        SunInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastSunInSign, ZodiacStartDate = startSunDate.Value, ZodiacEndDate = lastDate });
        MercuryInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastMercuryInSign, ZodiacStartDate = startMercuryDate.Value, ZodiacEndDate = lastDate });
        VenusInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastVenusInSign, ZodiacStartDate = startVenusDate.Value, ZodiacEndDate = lastDate });
        MarsInZodiacSegments.Add(new ZodiacSegment { ZodiacSign = lastMarsInSign, ZodiacStartDate = startMarsDate.Value, ZodiacEndDate = lastDate });
    }

}