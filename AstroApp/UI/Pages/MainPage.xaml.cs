using AstroApp.Data;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AstroApp.UI.Pages
{
    public partial class MainPage : ContentPage
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPage()
        {         
            InitializeComponent();
            Initialize();
            BindingContext = this;
        }

        private async Task Initialize()
        {
            App.AppData = new AppData();
            var appActions = new Services.AppActions();
            App.AppData.AppDB = await appActions.LoadDBAsync();           
            this.ActiveAstroEvents = App.AppData.AppDB.AstroEventsDB;
            if (this.ActiveAstroEvents == null)
            {
                this.ActiveAstroEvents =
                [
                    new AstroEvent()
                    {
                        Date = DateTime.Now,
                        MoonInZodiac = Data.Enums.ZodiacSign.Capricorn,
                        SunInZodiac = Data.Enums.ZodiacSign.Cancer,
                        MoonDay = new MoonDay() { NewMoonDay = 8, TransitionTime = DateTime.Now },
                        EventText = "Bandomasis tekstas",
                        MoonEclipse = false,
                        SunEclipse = false,
                        PlanetEvents = new ObservableCollection<PlanetEvent>()
                        {
                        new PlanetEvent()
                        {
                            Planet1 = Data.Enums.Planet.Jupiter,
                            Planet2 = Data.Enums.Planet.Venus,
                            AspectSymbol = Data.Enums.AspectSymbol.Opposition
                        }
                        }
                    },
                    new AstroEvent()
                    {
                        Date = DateTime.Now.AddDays(1),
                        MoonInZodiac = Data.Enums.ZodiacSign.Virgo,
                        SunInZodiac = Data.Enums.ZodiacSign.Cancer,
                        MoonDay = new MoonDay() { NewMoonDay = 3, TransitionTime = DateTime.Now },
                        EventText = "Bandomasis tekstas",
                        MoonEclipse = false,
                        SunEclipse = false,
                        PlanetEvents = new ObservableCollection<PlanetEvent>()
                        {
                        new PlanetEvent()
                        {
                            Planet1 = Data.Enums.Planet.Pluto,
                            Planet2 = Data.Enums.Planet.Neptune,
                            AspectSymbol = Data.Enums.AspectSymbol.Opposition
                        }
                        }
                    },
                    new AstroEvent()
                    {
                        Date = DateTime.Now.AddMonths(-1),
                        MoonInZodiac = Data.Enums.ZodiacSign.Leo,
                        SunInZodiac = Data.Enums.ZodiacSign.Cancer,
                        MoonDay = new MoonDay() { NewMoonDay = 4, TransitionTime = DateTime.Now },
                        EventText = "Bandomasis tekstas",
                        MoonEclipse = false,
                        SunEclipse = false,
                        PlanetEvents = new ObservableCollection<PlanetEvent>()
                        {
                        new PlanetEvent()
                        {
                            Planet1 = Data.Enums.Planet.Venus,
                            Planet2 = Data.Enums.Planet.Neptune,
                            AspectSymbol = Data.Enums.AspectSymbol.Opposition

                        }
                        }
                    },
                ];
            }

            UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
        }

        private void UpdateCalendar(int year, int month)
        {
            CalendarGrid.Clear();
            InitializeWeekdayLabels();

            this.year = year;
            this.month = month;
            DateTime startOfMonth = new DateTime(year, month, 1);
            this.MonthName = startOfMonth.ToString("MMMM");
            int days = DateTime.DaysInMonth(year, month);
            int dayOfWeek = ((int)startOfMonth.DayOfWeek + 6) % 7;
            PopulateCalendar(days, dayOfWeek);
        }

        private void PopulateCalendar(int days, int startColumn)
        {
            int row = 1, column = startColumn; //skip first row for weekdays

            for (int day = 1; day <= days; day++)
            {
                DayControl dayCard = new DayControl();
                dayCard.AddDayCardDayNumber(day);
                DateTime currentDate = new DateTime(year, month, day);
                AstroEvent astroEventForDate = ActiveAstroEvents.FirstOrDefault(e => e.Date.Date == currentDate.Date);

                if (astroEventForDate != null)
                {
                    dayCard.AddAstroEvent(astroEventForDate);
                }

                CalendarGrid.Add(dayCard, column, row);

                column = (column + 1) % 7;
                if (column == 0)
                {
                    row++;
                }
            }
        }

        private void InitializeWeekdayLabels()
        {
            string[] weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            for (int i = 0; i < 7; i++)
            {
                Label label = new Label
                {
                    Text = weekdays[i],
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 8,
                    TextColor = Color.FromRgb(214, 137, 16)
                };
                CalendarGrid.Add(label, i, 0);
            }
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

            UpdateCalendar(year, month);
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

            UpdateCalendar(year, month);
        }
    }
}
