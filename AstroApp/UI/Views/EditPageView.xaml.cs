using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AstroApp.UI.Views;

public partial class EditPageView : ContentView, INotifyPropertyChanged
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

    public EditPageView()
	{
		InitializeComponent();        
        Initialize();
        this.BindingContext = this;
    }  

    public async void Initialize()
    {
        var appActions = new Services.AppActions();        
        this.ActiveAstroEvents = await appActions.LoadAstroEventsAsync();        
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

        var appActions = new Services.AppActions();        
        this.PlanetInZodiacsDetails = await appActions.LoadPlanetInZodiacsDetailsAsync();

        // Iterate through each AstroEvent in ActiveAstroEvents
        foreach (var astroEvent in ActiveAstroEvents)
        {
            // For each AstroEvent, iterate through its PlanetInZodiacs
            foreach (var planetInZodiac in astroEvent.PlanetInZodiacs)
            {
                // Find the matching PlanetInZodiac in PlanetInZodiacsDetails
                var matchingDetail = PlanetInZodiacsDetails.FirstOrDefault(
                    p => p.Planet == planetInZodiac.Planet && p.ZodiacSign == planetInZodiac.ZodiacSign);

                // If a match is found, update the PlanetInZodiacInfo
                if (matchingDetail != null)
                {
                    planetInZodiac.PlanetInZodiacInfo = matchingDetail.PlanetInZodiacInfo;
                }
            }
        }

        appActions.SaveAstroEventsAsync(ActiveAstroEvents);
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
                     new PlanetInZodiac() { },
                     new PlanetInZodiac() { },
                     new PlanetInZodiac() { },
                     new PlanetInZodiac() { },
                     new PlanetInZodiac() { }
                    };
                this.ActiveAstroEvents.Add(astroEventForDate);
                editDayCard.AddAstroEvent(astroEventForDate);
            }
            this.EventList.Add(editDayCard);
        }
    }

}