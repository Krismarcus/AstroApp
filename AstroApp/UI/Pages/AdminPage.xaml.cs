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

    public int SelectedMoonDay {  get; set; }    
    public bool Is29MoonDayCycle { get; set; }

    public ObservableCollection<AstroEvent> ActiveAstroEvents { get; set; }

    public ObservableCollection<PlanetInZodiac> PlanetInZodiacsDetails { get; set; }

    public ObservableCollection<EditDayControl> TempDayList { get; set; }

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
        this.EventList.Clear();

        this.year = year;
        this.month = month;
        DateTime startOfMonth = new DateTime(year, month, 1);
        this.MonthName = startOfMonth.ToString("MMMM");
        int days = DateTime.DaysInMonth(year, month);
        PopulateList(days);
    }

    private void NextButton_Clicked(object sender, EventArgs e)
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

        UpdateList(year, month);
    }

    private void PrevButton_Clicked(object sender, EventArgs e)
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

        UpdateList(year, month);
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        App.AppData.AppDB.AstroEventsDB = ActiveAstroEvents;
        var appActions = new Services.AppActions();
        appActions.SaveAstroEventsDBAsync(App.AppData.AppDB);
        await Application.Current.MainPage.DisplayAlert("Success", "Calendar data saved succesfully", "OK");
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
            TempDayList.Clear();  

            for (int i = 1; i <= days; i++)
            {
                // Assuming EditDayControl can be instantiated in background thread; if not, adjust accordingly.
                EditDayControl editDayCard = new EditDayControl();

                DateTime currentDate = new DateTime(year, month, i);
                AstroEvent astroEventForDate = ActiveAstroEvents.FirstOrDefault(e => e.Date.Date == currentDate.Date);

                if (astroEventForDate != null)
                {
                    // Since this might update UI, ensure it runs on UI thread if necessary
                    Device.BeginInvokeOnMainThread(() => editDayCard.AddAstroEvent(astroEventForDate));
                }
                else
                {
                    astroEventForDate = new AstroEvent()
                    {
                        Date = currentDate,
                        SunInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Sun, NewZodiacSign = SunZodiacSignYearlyCalendar(currentDate), TransitionTime = new DateTime() },
                        MoonInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Moon, TransitionTime = new DateTime() },
                        VenusInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Venus, TransitionTime = new DateTime() },
                        MarsInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Mars, TransitionTime = new DateTime() },
                        MercuryInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Mercury, TransitionTime = new DateTime() },                        
                        EventText = "",
                        MoonDay = new MoonDay() { NewMoonDay = 0, TransitionTime = new DateTime() }
                    };
                    
                    // This might need to run on the UI thread too
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ActiveAstroEvents.Add(astroEventForDate);
                        editDayCard.AddAstroEvent(astroEventForDate);
                    });
                }
                TempDayList.Add(editDayCard);
            }

            // Update the main list on UI thread
            Device.BeginInvokeOnMainThread(() =>
            {
                this.EventList.Clear();
                foreach (var item in TempDayList)
                {
                    this.EventList.Add(item);
                }

                // Hide loading indicator
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            });
        });
    }

    public ZodiacSign SunZodiacSignYearlyCalendar(DateTime date)
    {
        // Define the zodiac sign start dates
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
        // Handle Capricorn specially since it spans the new year
        {ZodiacSign.Capricorn, new DateTime(date.Year, 12, 22)}
    };

        // Adjust for Capricorn spanning the year end and start
        if (date.Month == 1 && date.Day <= 19)
        {
            return ZodiacSign.Capricorn; // For dates from January 1 to January 19
        }

        // Determine the zodiac sign based on the date
        foreach (var signStartDate in zodiacSignStartDates.OrderByDescending(kv => kv.Value))
        {
            if (date >= signStartDate.Value)
            {
                return signStartDate.Key;
            }
        }

        // Default to Capricorn if none found, handles dates after December 22
        return ZodiacSign.Capricorn;

    }

    public void PopulatePickers()
    {        
        var zodiacSigns = Enum.GetValues(typeof(ZodiacSign));

        foreach (ZodiacSign zodiacSign in zodiacSigns)
        {
            VenusInZodiacPicker.Items.Add(zodiacSign.ToString());
            MarsInZodiacPicker.Items.Add(zodiacSign.ToString());
            MercuryInZodiacPicker.Items.Add(zodiacSign.ToString());
        }

        foreach (MoonDaySymbol moonDay in Enum.GetValues(typeof(MoonDaySymbol)))
        {
            this.MoonDayPicker.Items.Add(moonDay.ToString());
        }        
    }

    private void ZodiacSignPicker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedIndex != -1)
        {
            var selectedZodiacSign = (ZodiacSign)picker.SelectedIndex +1; // Adjusted assuming enum starts at 1

            if (picker == VenusInZodiacPicker)
            {
                foreach (var eventDay in ActiveAstroEvents)
                {                    
                    eventDay.VenusInZodiac.NewZodiacSign = selectedZodiacSign;
                }
            }
            else if (picker == MarsInZodiacPicker)
            {
                foreach (var eventDay in ActiveAstroEvents)
                {
                    eventDay.MarsInZodiac.NewZodiacSign = selectedZodiacSign;
                }
            }
            else if (picker == MercuryInZodiacPicker)
            {
                foreach (var eventDay in ActiveAstroEvents)
                {
                    eventDay.MercuryInZodiac.NewZodiacSign = selectedZodiacSign;
                }
            }
            
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

        int currentMoonDayValue = SelectedMoonDay; // Initial moon day value to start incrementing from
        int newMoonDay = SkipDayIndex;
        int skipDay = SkipDayIndex - 1;

        foreach (var astroEvent in ActiveAstroEvents)
        {
            // Check if the event is in the current month and year before updating.
            if (astroEvent.Date.Month == month && astroEvent.Date.Year == year)
            {
                astroEvent.MoonDay.IsTripleMoonDay = false;
                astroEvent.MoonDay.NewMoonDay = currentMoonDayValue;
                if (astroEvent.Date.Day == SkipDayIndex)
                {
                    astroEvent.MoonDay.IsTripleMoonDay = true;
                }
                // Now also passing is29DayCycle to the method.
                currentMoonDayValue = IncrementMoonDay(currentMoonDayValue, astroEvent.Date.Day, skipDay, Is29MoonDayCycle);

                astroEvent.MoonPhase = CalculatePhaseForDay(astroEvent.Date, newMoonDay);
            }
        }

        UpdateList(year, month); // Assume this updates your list display
    }    

    private int IncrementMoonDay(int currentMoonDay, int date, int skipDay, bool is29DayCycle)
    {
        // Determine the max day based on whether it's a 29-day cycle
        int maxDay = is29DayCycle ? 29 : 30;

        // Increment moon day by 1, reset to 1 if it reached the max day
        int nextMoonDay = currentMoonDay == maxDay ? 1 : currentMoonDay + 1;

        // Check if the next moon day is the day to skip
        if (date == skipDay)
        {
            // Skip the specified day by incrementing again, check for wrap-around
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
        else if (dayOfMonth >= 22 && dayOfMonth <= 31) // Adjust according to lunar cycle length
        {
            phase = MoonPhaseSymbol.ThirdQuarter;
        }
        else
        {
            // This is a simplification. The exact phase might need more nuanced calculation
            // especially for days close to the transition between phases or for handling lunar cycles
            // slightly shorter or longer than 29 days.
            phase = MoonPhaseSymbol.None; // Or some logic to handle edge cases
        }

        return (int)phase;

    }
}