using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AstroApp.UI.Pages;

public partial class AdminPage : ContentPage, INotifyPropertyChanged
{
    private string monthName;

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

    private int month, year;

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

    private int skipDayIndex;

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

    public ObservableCollection<PlanetInZodiac> PlanetInZodiacsDetails { get; set; }

    public ObservableCollection<EditDayControl> TempDayList { get; set; }

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
        UpdateList(DateTime.Now.Year, DateTime.Now.Month);
    }

    private void UpdateList(int year, int month)
    {
        this.year = year;
        this.month = month;
        DateTime startOfMonth = new DateTime(year, month, 1);
        this.MonthName = startOfMonth.ToString("MMMM");
        int days = DateTime.DaysInMonth(year, month);
        PopulateList(days);
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

    private async void PopulateList(int days)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
        });

        await Task.Run(() =>
        {
            if (TempDayList == null)
            {
                TempDayList = new ObservableCollection<EditDayControl>();
            }

            TempDayList.Clear(); // Clear the list before populating new data

            var tempList = new List<EditDayControl>();

            for (int i = 1; i <= days; i++)
            {
                DateTime currentDate = new DateTime(year, month, i);
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

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ActiveAstroEvents.Add(astroEventForDate);
                    });
                }

                var editDayCard = new EditDayControl();
                Device.BeginInvokeOnMainThread(() => editDayCard.AddAstroEvent(astroEventForDate));
                tempList.Add(editDayCard);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var item in tempList)
                {
                    TempDayList.Add(item);
                }
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            });
        });
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
        int currentMoonDayValue = SelectedMoonDay;
        int newMoonDay = SkipDayIndex;
        int skipDay = SkipDayIndex - 1;

        foreach (var astroEvent in ActiveAstroEvents)
        {
            if (astroEvent.Date.Month == month && astroEvent.Date.Year == year)
            {
                if (SelectedMarsZodiac != null)
                {
                    astroEvent.MarsInZodiac ??= new PlanetInZodiac();
                    astroEvent.MarsInZodiac.Planet = Planet.Mars;
                    astroEvent.MarsInZodiac.NewZodiacSign = (ZodiacSign)SelectedMarsZodiac;
                }

                if (SelectedVenusZodiac != null)
                {
                    astroEvent.VenusInZodiac ??= new PlanetInZodiac();
                    astroEvent.VenusInZodiac.Planet = Planet.Venus;
                    astroEvent.VenusInZodiac.NewZodiacSign = (ZodiacSign)SelectedVenusZodiac;
                }

                if (SelectedMercuryZodiac != null)
                {
                    astroEvent.MercuryInZodiac ??= new PlanetInZodiac();
                    astroEvent.MercuryInZodiac.Planet = Planet.Mercury;
                    astroEvent.MercuryInZodiac.NewZodiacSign = (ZodiacSign)SelectedMercuryZodiac;
                }

                if (SelectedJupiterZodiac != null)
                {
                    astroEvent.JupiterInZodiac ??= new PlanetInZodiac();
                    astroEvent.JupiterInZodiac.Planet = Planet.Jupiter;
                    astroEvent.JupiterInZodiac.NewZodiacSign = (ZodiacSign)SelectedJupiterZodiac;
                }

                if (SelectedSaturnZodiac != null)
                {
                    astroEvent.SaturnInZodiac ??= new PlanetInZodiac();
                    astroEvent.SaturnInZodiac.Planet = Planet.Saturn;
                    astroEvent.SaturnInZodiac.NewZodiacSign = (ZodiacSign)SelectedSaturnZodiac;
                }

                if (SelectedUranusZodiac != null)
                {
                    astroEvent.UranusInZodiac ??= new PlanetInZodiac();
                    astroEvent.UranusInZodiac.Planet = Planet.Uranus;
                    astroEvent.UranusInZodiac.NewZodiacSign = (ZodiacSign)SelectedUranusZodiac;
                }

                if (SelectedNeptuneZodiac != null)
                {
                    astroEvent.NeptuneInZodiac ??= new PlanetInZodiac();
                    astroEvent.NeptuneInZodiac.Planet = Planet.Neptune;
                    astroEvent.NeptuneInZodiac.NewZodiacSign = (ZodiacSign)SelectedNeptuneZodiac;
                }

                if (SelectedPlutoZodiac != null)
                {
                    astroEvent.PlutoInZodiac ??= new PlanetInZodiac();
                    astroEvent.PlutoInZodiac.Planet = Planet.Pluto;
                    astroEvent.PlutoInZodiac.NewZodiacSign = (ZodiacSign)SelectedPlutoZodiac;
                }

                if (SelectedSelenaZodiac != null)
                {
                    astroEvent.SelenaInZodiac ??= new PlanetInZodiac();
                    astroEvent.SelenaInZodiac.Planet = Planet.Selena;
                    astroEvent.SelenaInZodiac.NewZodiacSign = (ZodiacSign)SelectedSelenaZodiac;
                }

                if (SelectedLilitZodiac != null)
                {
                    astroEvent.LilithInZodiac ??= new PlanetInZodiac();
                    astroEvent.LilithInZodiac.Planet = Planet.Lilith;
                    astroEvent.LilithInZodiac.NewZodiacSign = (ZodiacSign)SelectedLilitZodiac;
                }

                if (SelectedRahuZodiac != null)
                {
                    astroEvent.RahuInZodiac ??= new PlanetInZodiac();
                    astroEvent.RahuInZodiac.Planet = Planet.Rahu;
                    astroEvent.RahuInZodiac.NewZodiacSign = (ZodiacSign)SelectedRahuZodiac;
                }

                if (SelectedKetuZodiac != null)
                {
                    astroEvent.KetuInZodiac ??= new PlanetInZodiac();
                    astroEvent.KetuInZodiac.Planet = Planet.Ketu;
                    astroEvent.KetuInZodiac.NewZodiacSign = (ZodiacSign)SelectedKetuZodiac;
                }

                if (SelectedMoonDay != 0 && SkipDayIndex != 0)
                {
                    astroEvent.MoonDay.IsTripleMoonDay = false;
                    astroEvent.MoonDay.NewMoonDay = currentMoonDayValue;
                    if (astroEvent.Date.Day == SkipDayIndex)
                    {
                        astroEvent.MoonDay.IsTripleMoonDay = true;
                    }
                    currentMoonDayValue = IncrementMoonDay(currentMoonDayValue, astroEvent.Date.Day, skipDay, Is29MoonDayCycle);

                    astroEvent.MoonPhase = CalculatePhaseForDay(astroEvent.Date, newMoonDay);
                }
            }
        }

        UpdateList(year, month);

        VenusInZodiacPicker.SelectedItem = null;
        MarsInZodiacPicker.SelectedItem = null;
        MercuryInZodiacPicker.SelectedItem = null;
        JupiterInZodiacPicker.SelectedItem = null;
        SaturnInZodiacPicker.SelectedItem = null;
        UranusInZodiacPicker.SelectedItem = null;
        NeptuneInZodiacPicker.SelectedItem = null;
        PlutoInZodiacPicker.SelectedItem = null;
        SelenaInZodiacPicker.SelectedItem = null;
        LilitInZodiacPicker.SelectedItem = null;
        RahuInZodiacPicker.SelectedItem = null;
        KetuInZodiacPicker.SelectedItem = null;
        MoonDayPicker.SelectedIndex = 0;
        SkipDayIndex = 0;
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

    private int IncrementMoonDay(int currentMoonDay, int date, int skipDay, bool is29DayCycle)
    {
        int maxDay = is29DayCycle ? 29 : 30;

        int nextMoonDay = currentMoonDay == maxDay ? 1 : currentMoonDay + 1;

        if (date == skipDay)
        {
            nextMoonDay = nextMoonDay == maxDay ? 1 : nextMoonDay + 1;
        }

        return nextMoonDay;
    }

    private int CalculatePhaseForDay(DateTime currentDay, int newMoonDay)
    {
        int dayOfMonth = (currentDay.Day - newMoonDay + 30) % 29;
        MoonPhaseSymbol phase;

        if (dayOfMonth >= 1 && dayOfMonth < 7)
        {
            phase = MoonPhaseSymbol.NewMoon;
        }
        else if (dayOfMonth >= 7 && dayOfMonth < 15)
        {
            phase = MoonPhaseSymbol.FirstQuarter;
        }
        else if (dayOfMonth >= 15 && dayOfMonth < 22)
        {
            phase = MoonPhaseSymbol.FullMoon;
        }
        else if (dayOfMonth >= 22 && dayOfMonth <= 31)
        {
            phase = MoonPhaseSymbol.ThirdQuarter;
        }
        else
        {
            phase = MoonPhaseSymbol.None;
        }

        return (int)phase;
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
