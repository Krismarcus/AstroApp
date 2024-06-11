using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AstroApp.UI.Pages;

public partial class AdminPage : ContentPage, INotifyPropertyChanged
{
    private string monthName;
    private int month, year;
    private int skipDayIndex;
    public ObservableCollection<EditDayControl> TempDayList { get; set; }

    public string MonthName
    {
        get => monthName;
        set
        {
            if (monthName != value)
            {
                monthName = value;
                OnPropertyChanged(nameof(MonthName));
            }
        }
    }

    public int Month
    {
        get => month;
        set
        {
            if (month != value)
            {
                month = value;
                OnPropertyChanged(nameof(Month));
            }
        }
    }

    public int Year
    {
        get => year;
        set
        {
            if (year != value)
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
        }
    }

    public int SkipDayIndex
    {
        get => skipDayIndex;
        set
        {
            if (skipDayIndex != value)
            {
                skipDayIndex = value;
                OnPropertyChanged(nameof(SkipDayIndex));
            }
        }
    }

    public int SelectedMoonDay { get; set; }
    public bool Is29MoonDayCycle { get; set; }
    public ZodiacSign? SelectedVenusZodiac { get; set; }
    public ZodiacSign? SelectedMarsZodiac { get; set; }
    public ZodiacSign? SelectedMercuryZodiac { get; set; }
    public ZodiacSign? SelectedJupiterZodiac { get; set; }
    public ZodiacSign? SelectedSaturnZodiac { get; set; }
    public ZodiacSign? SelectedUranusZodiac { get; set; }
    public ZodiacSign? SelectedNeptuneZodiac { get; set; }
    public ZodiacSign? SelectedPlutoZodiac { get; set; }
    public ZodiacSign? SelectedSelenaZodiac { get; set; }
    public ZodiacSign? SelectedLilitZodiac { get; set; }
    public ZodiacSign? SelectedRahuZodiac { get; set; }
    public ZodiacSign? SelectedKetuZodiac { get; set; }

    public ObservableCollection<AstroEvent> ActiveAstroEvents { get; set; }
    public ObservableCollection<ZodiacSign> ZodiacSigns { get; set; }

    public AdminPage()
    {
        InitializeComponent();
        PopulatePickers();
        Initialize();
        this.BindingContext = this;
    }

    public async void Initialize()
    {
        this.ActiveAstroEvents = App.AppData.AppDB.AstroEventsDB;
        if (this.ActiveAstroEvents == null)
        {
            this.ActiveAstroEvents = new ObservableCollection<AstroEvent> { };
        }
        InitializeDayControls();
        UpdateList(DateTime.Now.Year, DateTime.Now.Month);
    }

    private void InitializeDayControls()
    {
        this.TempDayList = new ObservableCollection<EditDayControl>();
        for (int i = 1; i <= 31; i++)
        {
            var editDayControl = new EditDayControl();
            this.TempDayList.Add(editDayControl);
        }
    }

    private void UpdateList(int year, int month)
    {
        this.year = year;
        this.month = month;
        DateTime startOfMonth = new DateTime(year, month, 1);
        this.MonthName = startOfMonth.ToString("MMMM");
        int days = DateTime.DaysInMonth(year, month);

        UpdateAstroEventsForMonth(days);
    }

    private void UpdateAstroEventsForMonth(int days)
    {
        for (int i = 0; i < 31; i++)
        {
            if (i < days)
            {
                DateTime currentDate = new DateTime(year, month, i + 1);
                AstroEvent astroEventForDate = ActiveAstroEvents.FirstOrDefault(e => e.Date.Date == currentDate.Date);

                if (astroEventForDate == null)
                {
                    astroEventForDate = new AstroEvent()
                    {
                        Date = currentDate,
                        SunInZodiac = CreateBlankPlanetInZodiac(Planet.Sun, currentDate),
                        MoonInZodiac = CreateBlankPlanetInZodiac(Planet.Moon),
                        VenusInZodiac = CreateBlankPlanetInZodiac(Planet.Venus),
                        MarsInZodiac = CreateBlankPlanetInZodiac(Planet.Mars),
                        MercuryInZodiac = CreateBlankPlanetInZodiac(Planet.Mercury),
                        JupiterInZodiac = CreateBlankPlanetInZodiac(Planet.Jupiter),
                        SaturnInZodiac = CreateBlankPlanetInZodiac(Planet.Saturn),
                        UranusInZodiac = CreateBlankPlanetInZodiac(Planet.Uranus),
                        NeptuneInZodiac = CreateBlankPlanetInZodiac(Planet.Neptune),
                        PlutoInZodiac = CreateBlankPlanetInZodiac(Planet.Pluto),
                        SelenaInZodiac = CreateBlankPlanetInZodiac(Planet.Selena),
                        LilithInZodiac = CreateBlankPlanetInZodiac(Planet.Lilith),
                        RahuInZodiac = CreateBlankPlanetInZodiac(Planet.Rahu),
                        KetuInZodiac = CreateBlankPlanetInZodiac(Planet.Ketu),
                        MoonDay = new MoonDay() { NewMoonDay = 0, TransitionTime = DateTime.MinValue },
                        MoonPhase = (int)MoonPhaseSymbol.None,
                        SunEclipse = false,
                        MoonEclipse = false,
                        Barber = ActivityQuality.Neutral,
                        Beauty = ActivityQuality.Neutral,
                        Buystuff = ActivityQuality.Neutral,
                        Contracts = ActivityQuality.Neutral,
                        ImportantTasks = ActivityQuality.Neutral,
                        Gardening = ActivityQuality.Neutral,
                        Love = ActivityQuality.Neutral,
                        Meetings = ActivityQuality.Neutral,
                        NewIdeas = ActivityQuality.Neutral,
                        Tech = ActivityQuality.Neutral,
                        Travel = ActivityQuality.Neutral,
                        EventText = ""
                    };

                    ActiveAstroEvents.Add(astroEventForDate);
                }
                TempDayList[i].IsActive = true;
                TempDayList[i].AddAstroEvent(astroEventForDate);
            }
            else
            {
                TempDayList[i].IsActive = false;
                TempDayList[i].DayAstroEvent = null;
            }
        }
    }

    private PlanetInZodiac CreateBlankPlanetInZodiac(Planet planet, DateTime? date = null)
    {
        return new PlanetInZodiac()
        {
            Planet = planet,
            NewZodiacSign = planet == Planet.Sun && date.HasValue ? SunZodiacSignYearlyCalendar(date.Value) : ZodiacSign.Aries,
            IsRetrograde = false,
            IsZodiacTransitioning = false,
            TransitionTime = DateTime.MinValue,
            PlanetInZodiacInfo = string.Empty
        };
    }

    public ZodiacSign SunZodiacSignYearlyCalendar(DateTime date)
    {
        var zodiacSignStartDates = new Dictionary<ZodiacSign, DateTime>
        {
            {ZodiacSign.Aquarius, new DateTime(date.Year, 1, 20)},
            {ZodiacSign.Pisces, new DateTime(date.Year, 2, 19)},
            {ZodiacSign.Aries, new DateTime(date.Year, 3, 21)},
            {ZodiacSign.Taurus, new DateTime(date.Year, 4, 20)},
            {ZodiacSign.Gemini, new DateTime(date.Year, 5, 21)},
            {ZodiacSign.Cancer, new DateTime(date.Year, 6, 21)},
            {ZodiacSign.Leo, new DateTime(date.Year, 7, 23)},
            {ZodiacSign.Virgo, new DateTime(date.Year, 8, 23)},
            {ZodiacSign.Libra, new DateTime(date.Year, 9, 23)},
            {ZodiacSign.Scorpio, new DateTime(date.Year, 10, 23)},
            {ZodiacSign.Sagittarius, new DateTime(date.Year, 11, 22)},
            {ZodiacSign.Capricorn, new DateTime(date.Year, 12, 22)}
        };

        if (date.Month == 1 && date.Day <= 19)
        {
            return ZodiacSign.Capricorn;
        }

        foreach (var signStartDate in zodiacSignStartDates.OrderByDescending(kv => kv.Value))
        {
            if (date >= signStartDate.Value)
            {
                return signStartDate.Key;
            }
        }

        return ZodiacSign.Capricorn;
    }

    public void PopulatePickers()
    {
        this.ZodiacSigns = new ObservableCollection<ZodiacSign>(Enum.GetValues(typeof(ZodiacSign)).Cast<ZodiacSign>());

        foreach (MoonDaySymbol moonDay in Enum.GetValues(typeof(MoonDaySymbol)))
        {
            this.MoonDayPicker.Items.Add(moonDay.ToString());
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Update astro events based on the selected values
    }

    private async void LoadFromUrlButton_Clicked(object sender, EventArgs e)
    {
        var appActions = new Services.AppActions();
        string url = "https://raw.githubusercontent.com/Krismarcus/AstroAppJSON/main/astrodb.json";
        var appDB = await appActions.LoadDBFromUrlAsync(url);

        if (appDB != null)
        {
            App.AppData.AppDB = appDB;
            this.ActiveAstroEvents = appDB.AstroEventsDB;
            UpdateList(DateTime.Now.Year, DateTime.Now.Month);
            await Application.Current.MainPage.DisplayAlert("Success", "Database loaded from URL successfully", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to load database from URL", "OK");
        }
    }

    private async void NextButton_Clicked(object sender, EventArgs e)
    {
        if (month == 12)
        {
            year++;
            month = 1;
        }
        else
        {
            month++;
        }

        await Task.Run(() => UpdateList(year, month));
    }

    private async void PrevButton_Clicked(object sender, EventArgs e)
    {
        if (month == 1)
        {
            year--;
            month = 12;
        }
        else
        {
            month--;
        }

        await Task.Run(() => UpdateList(year, month));
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        App.AppData.AppDB.AstroEventsDB = ActiveAstroEvents;
        var appActions = new Services.AppActions();
        await appActions.SaveAstroEventsDBAsync(App.AppData.AppDB);
        await Application.Current.MainPage.DisplayAlert("Success", "Calendar data saved successfully", "OK");
    }

    private async void DeleteCurrentMonthEventsButton_Clicked(object sender, EventArgs e)
    {
        bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
            "Confirm Deletion",
            "Are you sure you want to delete all events for the current month?",
            "Yes",
            "No");

        if (isConfirmed)
        {
            var eventsToRemove = ActiveAstroEvents.Where(e => e.Date.Month == month && e.Date.Year == year).ToList();

            foreach (var astroEvent in eventsToRemove)
            {
                ActiveAstroEvents.Remove(astroEvent);
            }

            var appActions = new Services.AppActions();
            App.AppData.AppDB.AstroEventsDB = ActiveAstroEvents;
            await appActions.SaveAstroEventsDBAsync(App.AppData.AppDB);

            UpdateList(year, month);

            await Application.Current.MainPage.DisplayAlert("Success", "Events for the current month have been deleted.", "OK");
        }
    }

    private async void DeleteAllEventsButton_Clicked(object sender, EventArgs e)
    {
        bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
            "Confirm Deletion",
            "Are you sure you want to delete all events?",
            "Yes",
            "No");

        if (isConfirmed)
        {
            ActiveAstroEvents.Clear();

            var appActions = new Services.AppActions();
            App.AppData.AppDB.AstroEventsDB = ActiveAstroEvents;
            await appActions.SaveAstroEventsDBAsync(App.AppData.AppDB);

            UpdateList(year, month);

            await Application.Current.MainPage.DisplayAlert("Success", "All events have been deleted.", "OK");
        }
    }

    private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedIndex >= 0)
        {
            var selectedMonth = picker.SelectedIndex + 1;
            UpdateList(year, selectedMonth);
        }
    }
}
