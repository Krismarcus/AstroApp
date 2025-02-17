using AstroApp.Data;
using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace AstroApp.UI.Pages
{
    [QueryProperty(nameof(Month), "month")]
    [QueryProperty(nameof(Year), "year")]
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

        private int month;
        public int Month
        {
            get => month;
            set
            {
                if (month != value)
                {
                    month = value;
                    OnPropertyChanged(nameof(Month));
                    UpdateCalendar(Year, Month);
                }
            }
        }

        private int year;
        public int Year
        {
            get => year;
            set
            {
                if (year != value)
                {
                    year = value;
                    OnPropertyChanged(nameof(Year));
                    UpdateCalendar(Year, Month);
                }
            }
        }

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

        private string activityProfileLt = "Palankios/Nepalankios Dienos";

        public string ActivityProfileLt
        {
            get => activityProfileLt;
            set
            {
                if (activityProfileLt != value)
                {
                    activityProfileLt = value;
                    OnPropertyChanged(nameof(ActivityProfileLt));
                    UpdateBackgroundColors();
                }
            }
        }

        private Color noPresetBackgroundColor;

        public Color NoPresetBackgroundColor
        {
            get => noPresetBackgroundColor;
            set
            {
                if (noPresetBackgroundColor != value)
                {
                    noPresetBackgroundColor = value;
                    OnPropertyChanged(nameof(NoPresetBackgroundColor));
                }
            }
        }

        public Color BarberBackgroundColor { get; set; } = Colors.Transparent;
        public Color BeautyBackgroundColor { get; set; } = Colors.Transparent;
        public Color BuyStuffBackgroundColor { get; set; } = Colors.Transparent;
        public Color ContractsBackgroundColor { get; set; } = Colors.Transparent;
        public Color ImportantTasksBackgroundColor { get; set; } = Colors.Transparent;
        public Color GardeningBackgroundColor { get; set; } = Colors.Transparent;
        public Color LoveBackgroundColor { get; set; } = Colors.Transparent;
        public Color MeetingsBackgroundColor { get; set; } = Colors.Transparent;
        public Color NewIdeasBackgroundColor { get; set; } = Colors.Transparent;
        public Color TechBackgroundColor { get; set; } = Colors.Transparent;
        public Color TravelBackgroundColor { get; set; } = Colors.Transparent;

        private bool isProfileGridVisible = false;        

        private Color weekdaysColor = Color.FromRgb(254, 234, 181); // Default color

        public Color WeekdaysColor
        {
            get => weekdaysColor;
            set
            {
                if (weekdaysColor != value)
                {
                    weekdaysColor = value;
                    OnPropertyChanged(nameof(WeekdaysColor));
                }
            }
        }

        private Color activityColor = Color.FromRgb(254, 234, 181); // Default color

        public Color ActivityColor
        {
            get => activityColor;
            set
            {
                if (activityColor != value)
                {
                    activityColor = value;
                    OnPropertyChanged(nameof(ActivityColor));
                }
            }
        }


        private void UpdateBackgroundColors()
        {
            NoPresetBackgroundColor = ActivityProfile == "nopreset" ? ActivityColor : Colors.Transparent;
            BarberBackgroundColor = ActivityProfile == "barber" ? ActivityColor : Colors.Transparent;
            BeautyBackgroundColor = ActivityProfile == "beauty" ? ActivityColor : Colors.Transparent;
            BuyStuffBackgroundColor = ActivityProfile == "buystuff" ? ActivityColor : Colors.Transparent;
            ContractsBackgroundColor = ActivityProfile == "contracts" ? ActivityColor : Colors.Transparent;
            ImportantTasksBackgroundColor = ActivityProfile == "importanttasks" ? ActivityColor : Colors.Transparent;
            GardeningBackgroundColor = ActivityProfile == "gardening" ? ActivityColor : Colors.Transparent;            
            LoveBackgroundColor = ActivityProfile == "love" ? ActivityColor : Colors.Transparent;
            MeetingsBackgroundColor = ActivityProfile == "meetings" ? ActivityColor : Colors.Transparent;
            NewIdeasBackgroundColor = ActivityProfile == "newideas" ? ActivityColor : Colors.Transparent;
            TechBackgroundColor = ActivityProfile == "tech" ? ActivityColor : Colors.Transparent;
            TravelBackgroundColor = ActivityProfile == "travel" ? activityColor : Colors.Transparent;
            

            // Notify the UI to update
            OnPropertyChanged(nameof(NoPresetBackgroundColor));
            OnPropertyChanged(nameof(BarberBackgroundColor));
            OnPropertyChanged(nameof(BeautyBackgroundColor));
            OnPropertyChanged(nameof(BuyStuffBackgroundColor));
            OnPropertyChanged(nameof(ContractsBackgroundColor));
            OnPropertyChanged(nameof(ImportantTasksBackgroundColor));
            OnPropertyChanged(nameof(GardeningBackgroundColor));
            OnPropertyChanged(nameof(LoveBackgroundColor));
            OnPropertyChanged(nameof(MeetingsBackgroundColor));
            OnPropertyChanged(nameof(NewIdeasBackgroundColor));
            OnPropertyChanged(nameof(TechBackgroundColor));
            OnPropertyChanged(nameof(TravelBackgroundColor));
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
            string monthnameLT = startOfMonth.ToString("MMMM", App.AppData.CultureInfo);
            this.MonthName = char.ToUpperInvariant(monthnameLT[0]) + monthnameLT.Substring(1);
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
                dayCard.OnOffProfile(ActivityProfile != "nopreset");
                dayCard.ChangeActivityProfile(ActivityProfile);
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
            string[] weekdays = { "Pr", "A", "T", "K", "Pn", "Š", "S" };
            for (int i = 0; i < 7; i++)
            {
                Label label = new Label
                {
                    Text = weekdays[i],
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                    // TextColor is now bound to WeekdaysColor.
                };
                Binding textColorBinding = new Binding
                {
                    Source = this, // MainPage is the BindingContext
                    Path = nameof(WeekdaysColor)
                };
                label.SetBinding(Label.TextColorProperty, textColorBinding);
                CalendarGrid.Add(label, i, 0);
            }
            //string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            //for (int i = 0; i < 7; i++)
            //{
            //    Label label = new Label
            //    {
            //        Text = weekdays[i],
            //        HorizontalOptions = LayoutOptions.Center,
            //        VerticalOptions = LayoutOptions.Center,
            //        FontSize = 14,
            //        TextColor = WeekdaysColor
            //    };
            //    CalendarGrid.Add(label, i, 0);
            //}
        }
        private async void PrevMonth_Clicked(object sender, TappedEventArgs e)
        {
            await leftArrow.TranslateTo(-10, 0, 100); // Move 10 units to the left over 100ms
            await leftArrow.TranslateTo(0, 0, 100); // Move back to original position            

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

        private async void NextMonth_Clicked(object sender, TappedEventArgs e)
        {
            await rightArrow.TranslateTo(10, 0, 100); // Move 10 units to the right over 100ms
            await rightArrow.TranslateTo(0, 0, 100); // Move back to original position

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
        private void BarberRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "barber";
            ActivityProfileLt = "Kirpykla";
            UpdateCalendar(year, month);
        }

        private void BeautyRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "beauty";
            ActivityProfileLt = "Grožis";
            UpdateCalendar(year, month);
        }

        private void BuyStuffRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "buystuff";
            ActivityProfileLt = "Pirkiniai";
            UpdateCalendar(year, month);
        }

        private void ContractsRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "contracts";
            ActivityProfileLt = "Sutartys";
            UpdateCalendar(year, month);
        }

        private void ImportantTasksRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "importanttasks";
            ActivityProfileLt = "Svarbūs darbai";
            UpdateCalendar(year, month);
        }

        private void GardeningRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "gardening";
            ActivityProfileLt = "Sodininkystė";
            UpdateCalendar(year, month);
        }

        private void LoveRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "love";
            ActivityProfileLt = "Meilė";
            UpdateCalendar(year, month);
        }

        private void MeetingsRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "meetings";
            ActivityProfileLt = "Susitikimai";
            UpdateCalendar(year, month);
        }

        private void NewIdeasRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "newideas";
            ActivityProfileLt = "Mokymasis";
            UpdateCalendar(year, month);
        }

        private void TechnologiesRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "tech";
            ActivityProfileLt = "Technika";
            UpdateCalendar(year, month);
        }

        private void TravelRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "travel";
            ActivityProfileLt = "Kelionės";
            UpdateCalendar(year, month);
        }

        private void NoPresetRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "nopreset";
            ActivityProfileLt = "Palankios/Nepalankios Dienos";
            UpdateCalendar(year, month);
        }

        private async void ToggleGridAndAnimateArrow()
        {
            if (isProfileGridVisible)
            {
                // Animate both arrows down before hiding the grid
                var arrowDownLeft = leftArrowImage.TranslateTo(0, 10, 100);
                var arrowDownRight = rightArrowImage.TranslateTo(0, 10, 100);
                await Task.WhenAll(arrowDownLeft, arrowDownRight);

                var arrowReturnLeft = leftArrowImage.TranslateTo(0, 0, 100);
                var arrowReturnRight = rightArrowImage.TranslateTo(0, 0, 100);
                await Task.WhenAll(arrowReturnLeft, arrowReturnRight);

                // Hide the grid
                await profileGrid.TranslateTo(0, profileGrid.Height, 250, Easing.SinIn);
                profileGrid.IsVisible = false;

                // Rotate both arrows back to their original orientation
                var rotateBackLeft = leftArrowImage.RotateTo(0, 200);
                var rotateBackRight = rightArrowImage.RotateTo(0, 200);
                await Task.WhenAll(rotateBackLeft, rotateBackRight);
            }
            else
            {
                // Make the grid visible before animating it
                profileGrid.IsVisible = true;
                profileGrid.TranslationY = profileGrid.Height;  // Start off-screen

                // Animate both arrows up before showing the grid
                var arrowUpLeft = leftArrowImage.TranslateTo(0, -10, 100);
                var arrowUpRight = rightArrowImage.TranslateTo(0, -10, 100);
                await Task.WhenAll(arrowUpLeft, arrowUpRight);

                var arrowReturnLeft = leftArrowImage.TranslateTo(0, 0, 100);
                var arrowReturnRight = rightArrowImage.TranslateTo(0, 0, 100);
                await Task.WhenAll(arrowReturnLeft, arrowReturnRight);

                // Animate the grid into view
                await profileGrid.TranslateTo(0, 0, 250, Easing.SinOut);

                // Rotate both arrows 180 degrees to indicate the grid is open
                var rotateLeft = leftArrowImage.RotateTo(180, 200);
                var rotateRight = rightArrowImage.RotateTo(180, 200);
                await Task.WhenAll(rotateLeft, rotateRight);
            }

            isProfileGridVisible = !isProfileGridVisible;
        }

        private void OnArrowTapped(object sender, EventArgs e)
        {
            ToggleGridAndAnimateArrow();
        }

        private async void OnMonthLabelTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(YearPage)}");
        }

        private async void HideGridWithAnimation()
        {
            // Animate both arrows up before hiding the grid
            Task arrowUpLeft = leftArrowImage.TranslateTo(0, -30, 200); // Adjust these values as needed
            Task arrowUpRight = rightArrowImage.TranslateTo(0, -30, 200); // Adjust these values as needed
            await Task.WhenAll(arrowUpLeft, arrowUpRight);

            // Animate both arrows down to return to the original position
            Task arrowDownLeft = leftArrowImage.TranslateTo(0, 0, 200);
            Task arrowDownRight = rightArrowImage.TranslateTo(0, 0, 200);
            await Task.WhenAll(arrowDownLeft, arrowDownRight);

            // Rotate both arrows back to their original orientation (0 degrees)
            Task rotateBackLeft = leftArrowImage.RotateTo(0, 200);
            Task rotateBackRight = rightArrowImage.RotateTo(0, 200);
            await Task.WhenAll(rotateBackLeft, rotateBackRight);

            // Hide the Grid
            profileGrid.IsVisible = false;
        }        
    }    
}
