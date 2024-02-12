using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Pages;

public partial class AdminPage : ContentPage
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
    public List<AstroEvent> ActiveAstroEvents { get; set; }

    public ObservableCollection<PlanetInZodiac> PlanetInZodiacsDetails { get; set; }

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
            this.ActiveAstroEvents = new List<AstroEvent> { };
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

    private void PopulateList(int days)
    {

        this.EventList.Clear();

        for (int i = 1; i <= days; i++)
        {
            EditDayControl editDayCard = new EditDayControl();

            // Find the AstroEvent for the current date
            DateTime currentDate = new DateTime(year, month, i);

            AstroEvent astroEventForDate = ActiveAstroEvents.FirstOrDefault(e => e.Date.Date == currentDate.Date);

            // If an astro event exists for the current date, set the ZodiacSign
            if (astroEventForDate != null)
            {
                editDayCard.AddAstroEvent(astroEventForDate); // You can choose which ZodiacSign property to use
            }

            else
            {
                astroEventForDate = new AstroEvent() {
                    Date = currentDate,
                    SunInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Sun, NewZodiacSign = SunZodiacSignYearlyCalendar(currentDate),  TransitionTime = new DateTime() },
                    MoonInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Moon, TransitionTime = new DateTime() }, 
                    VenusInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Venus, TransitionTime = new DateTime() }, 
                    MarsInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Mars, TransitionTime = new DateTime() }, 
                    MercuryInZodiac = new PlanetInZodiac() { Planet = Data.Enums.Planet.Mercury, TransitionTime = new DateTime() }, 
                    EventText = "", 
                    MoonDay = new MoonDay() { NewMoonDay = 0, TransitionTime = new DateTime() } };
                this.ActiveAstroEvents.Add(astroEventForDate);
                editDayCard.AddAstroEvent(astroEventForDate);
            }
            this.EventList.Add(editDayCard);
        }
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
        //foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        //{
        //    this.SunInZodiacPicker.Items.Add(zodiacSign.ToString());
        //}

        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.MoonInZodiacPicker.Items.Add(zodiacSign.ToString());
        }

        //foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        //{
        //    this.VenusInZodiacPicker.Items.Add(zodiacSign.ToString());
        //}

        //foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        //{
        //    this.MarsInZodiacPicker.Items.Add(zodiacSign.ToString());
        //}

        //foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        //{
        //    this.MercuryInZodiacPicker.Items.Add(zodiacSign.ToString());
        //}
    }

    private void ZodiacSignPicker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (MoonInZodiacPicker.SelectedIndex == -1)
            return;

        var selectedZodiacSign = (ZodiacSign)(MoonInZodiacPicker.SelectedIndex + 1); // Assuming enum starts at 1

        foreach (var eventDay in ActiveAstroEvents)
        {
            // Assuming you want to change the SunInZodiac for demonstration
            eventDay.MoonInZodiac.NewZodiacSign = selectedZodiacSign;
        }

        // Refresh the list to reflect changes
        UpdateList(year, month);
    }


}