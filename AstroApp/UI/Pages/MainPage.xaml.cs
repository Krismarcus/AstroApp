using AstroApp.Data;
using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AstroApp.UI.Pages
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
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
        public ObservableCollection<AstroEvent> ActiveAstroEvents { get; set; }        

        private string activityProfile;

        public string ActivityProfile
        {
            get => activityProfile;
            set
            {
                if (activityProfile != value)
                {
                    activityProfile = value;
                    OnPropertyChanged(nameof(ActivityProfile));
                    UpdateBackgroundColors();
                }
            }
        }
        public Color NoPresetBackgroundColor { get; set; } = Colors.Orange;
        public Color GardeningBackgroundColor { get; set; } = Colors.Transparent;
        public Color LoveBackgroundColor { get; set; } = Colors.Transparent;

        private void UpdateBackgroundColors()
        {
            GardeningBackgroundColor = ActivityProfile == "gardening" ? Color.FromRgba(245, 197, 120, 255) : Colors.Transparent;
            LoveBackgroundColor = ActivityProfile == "love" ? Color.FromRgba(245, 197, 120, 255) : Colors.Transparent;
            NoPresetBackgroundColor = ActivityProfile == "nopreset" ? Color.FromRgba(245, 197, 120, 255) : Colors.Transparent;

            // Notify the UI to update
            OnPropertyChanged(nameof(GardeningBackgroundColor));
            OnPropertyChanged(nameof(LoveBackgroundColor));
            OnPropertyChanged(nameof(NoPresetBackgroundColor));
        }

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
            ActivityProfile = "nopreset";
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
            this.year = year;
            this.month = month;
            DateTime startOfMonth = new DateTime(year, month, 1);
            this.MonthName = startOfMonth.ToString("MMMM");
            int days = DateTime.DaysInMonth(year, month);
            int dayOfWeek = ((int)startOfMonth.DayOfWeek + 6) % 7;
            PopulateCalendar(days, dayOfWeek);
            InitializeWeekdayLabels();
        }

        private void PopulateCalendar(int days, int startColumn)
        {
            CalendarGrid.Children.Clear();
            // Assuming you have already called InitializeWeekdayLabels elsewhere and it properly sets up the first row for weekday labels
            // Start adding row definitions for days, assuming the first row is reserved for weekday labels
            for (int i = 0; i < 6; i++) // 6 rows for days
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            int currentRow = 1; // Start from row 1 to leave space for weekday labels at row 0
            int currentColumn = startColumn; // Start column based on the first day of the month

            for (int day = 1; day <= days; day++)
            {
                var currentDate = new DateTime(year, month, day);
                var astroEventForDate = ActiveAstroEvents.FirstOrDefault(e => e.Date.Date == currentDate);

                DayControl dayCard = new DayControl
                {
                    // Initialize with relevant data
                    DayAstroEvent = astroEventForDate,
                    DayNumber = day
                };
                dayCard.LocateDayCardGrid(currentDate);

                // Assuming LocateDayCardGrid correctly updates CalendarRow and CalendarColumn
                Grid.SetRow(dayCard, dayCard.CalendarRow);
                Grid.SetColumn(dayCard, dayCard.CalendarColumn);

                if (!CalendarGrid.Children.Contains(dayCard))
                {
                    CalendarGrid.Children.Add(dayCard);
                }
                dayCard.SetBorderColor(ActivityProfile);
                // Update property or trigger binding refresh if necessary

                // Move to the next cell
                currentColumn++;
                if (currentColumn > 6) // End of the week, move to next row
                {
                    currentColumn = 0;
                    currentRow++;
                }
            }

            // Ensure there's enough row definitions based on how many days and start day
            while (CalendarGrid.RowDefinitions.Count < currentRow + 1)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
        }

        private void InitializeWeekdayLabels()
        {
            string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            for (int i = 0; i < 7; i++)
            {
                Label label = new Label
                {
                    Text = weekdays[i],
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 14,
                    TextColor = Color.FromRgba(54, 130, 181, 255)
                };
                CalendarGrid.Add(label, i, 0);
            }
        }

        private void NextMonth_Clicked(object sender, TappedEventArgs e)
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

        private void GardeningRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "gardening";
            UpdateCalendar(year, month);
        }

        private void LoveRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "love";
            UpdateCalendar(year, month);
        }

        private void NoPresetRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "nopreset";
            UpdateCalendar(year, month);
        }

        private void PrevMonth_Clicked(object sender, TappedEventArgs e)
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
