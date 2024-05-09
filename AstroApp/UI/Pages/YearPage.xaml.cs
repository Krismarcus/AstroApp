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

        ZodiacSign lastSunInSign = this.ActiveAstroEvents.FirstOrDefault()?.SunInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMercuryInSign = this.ActiveAstroEvents.FirstOrDefault()?.MercuryInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastVenusInSign = this.ActiveAstroEvents.FirstOrDefault()?.VenusInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;
        ZodiacSign lastMarsInSign = this.ActiveAstroEvents.FirstOrDefault()?.MarsInZodiac.NewZodiacSign ?? ZodiacSign.Pisces;

        int currentSegmentDuration = 0;  // To track the duration of the current zodiac sign

        foreach (var astroEvent in this.ActiveAstroEvents.OrderBy(e => e.Date))  // Ensure events are sorted by date
        {
            ZodiacSign sunInZodiac = astroEvent.SunInZodiac.NewZodiacSign;
            ZodiacSign mercuryInZodiac = astroEvent.MercuryInZodiac.NewZodiacSign;
            ZodiacSign venusInZodiac = astroEvent.VenusInZodiac.NewZodiacSign;
            ZodiacSign marsInZodiac = astroEvent.MarsInZodiac.NewZodiacSign;

            if (sunInZodiac != lastSunInSign)
            {
                // If the sign changes, finalize the current segment and add to the calendar
                if (currentSegmentDuration > 0)
                {
                    var segment = new ZodiacSegment { ZodiacSign = lastSunInSign, Duration = currentSegmentDuration };
                    SunInZodiacSegments.Add(segment);
                }
                // Reset the duration for the new segment
                currentSegmentDuration = 0;
                lastSunInSign = sunInZodiac;
            }

            if (mercuryInZodiac != lastMercuryInSign)
            {
                // If the sign changes, finalize the current segment and add to the calendar
                if (currentSegmentDuration > 0)
                {
                    var segment = new ZodiacSegment { ZodiacSign = lastMercuryInSign, Duration = currentSegmentDuration };
                    MercuryInZodiacSegments.Add(segment);
                }
                // Reset the duration for the new segment
                currentSegmentDuration = 0;
                lastMercuryInSign = mercuryInZodiac;
            }

            if (venusInZodiac != lastVenusInSign)
            {
                // If the sign changes, finalize the current segment and add to the calendar
                if (currentSegmentDuration > 0)
                {
                    var segment = new ZodiacSegment { ZodiacSign = lastVenusInSign, Duration = currentSegmentDuration };
                    VenusInZodiacSegments.Add(segment);
                }
                // Reset the duration for the new segment
                currentSegmentDuration = 0;
                lastVenusInSign = venusInZodiac;
            }

            if (marsInZodiac != lastMarsInSign)
            {
                // If the sign changes, finalize the current segment and add to the calendar
                if (currentSegmentDuration > 0)
                {
                    var segment = new ZodiacSegment { ZodiacSign = lastMarsInSign, Duration = currentSegmentDuration };
                    MarsInZodiacSegments.Add(segment);
                }
                // Reset the duration for the new segment
                currentSegmentDuration = 0;
                lastMarsInSign = marsInZodiac;
            }
            currentSegmentDuration++;  // Increment the duration of the current zodiac segment
        }

        // Add the last segment of the year
        var finalSuninSignSegment = new ZodiacSegment { ZodiacSign = lastSunInSign, Duration = currentSegmentDuration };
        SunInZodiacSegments.Add(finalSuninSignSegment);
        var finalMercuryinSignSegment = new ZodiacSegment { ZodiacSign = lastMercuryInSign, Duration = currentSegmentDuration };
        MercuryInZodiacSegments.Add(finalMercuryinSignSegment);
        var finalVenusinSignSegment = new ZodiacSegment { ZodiacSign = lastVenusInSign, Duration = currentSegmentDuration };
        VenusInZodiacSegments.Add(finalVenusinSignSegment);
        var finalMarsinSignSegment = new ZodiacSegment { ZodiacSign = lastMarsInSign, Duration = currentSegmentDuration };
        MarsInZodiacSegments.Add(finalMarsinSignSegment);
    }
}