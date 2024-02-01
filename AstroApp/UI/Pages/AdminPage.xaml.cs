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
                astroEventForDate = new AstroEvent() { Date = currentDate, EventText = "", MoonDay = new MoonDay() { NewMoonDay = 0, TransitionTime = new DateTime() } };
                astroEventForDate.PlanetInZodiacs = new ObservableCollection<PlanetInZodiac> {
                     new PlanetInZodiac() { Planet = Data.Enums.Planet.Sun },
                     new PlanetInZodiac() { Planet = Data.Enums.Planet.Moon },
                     new PlanetInZodiac() { Planet = Data.Enums.Planet.Mercury },
                     new PlanetInZodiac() { Planet = Data.Enums.Planet.Venus },
                     new PlanetInZodiac() { Planet = Data.Enums.Planet.Mars }
                    };
                this.ActiveAstroEvents.Add(astroEventForDate);
                editDayCard.AddAstroEvent(astroEventForDate);
            }
            this.EventList.Add(editDayCard);
        }
    }
}