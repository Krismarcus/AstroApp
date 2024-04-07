using AstroApp.Data;
using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

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

        private bool isProfileGridVisible = false;

        private Color _pageBackgroundColor1 = Color.FromRgb(6, 57, 112); // Default color

        public Color PageBackgroundColor1
        {
            get => _pageBackgroundColor1;
            set
            {
                if (_pageBackgroundColor1 != value)
                {
                    _pageBackgroundColor1 = value;
                    OnPropertyChanged(nameof(PageBackgroundColor1));
                }
            }
        }

        private Color _pageBackgroundColor2 = Color.FromRgb(30, 129, 176); // Default color

        public Color PageBackgroundColor2
        {
            get => _pageBackgroundColor2;
            set
            {
                if (_pageBackgroundColor2 != value)
                {
                    _pageBackgroundColor2 = value;
                    OnPropertyChanged(nameof(PageBackgroundColor2));
                }
            }
        }

        private Color monthNameColor = Color.FromRgb(254, 234, 181); // Default color

        public Color MonthNameColor
        {
            get => monthNameColor;
            set
            {
                if (monthNameColor != value)
                {
                    monthNameColor = value;
                    OnPropertyChanged(nameof(MonthNameColor));
                }
            }
        }

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
            BeautyBackgroundColor = ActivityProfile == "beauty" ? ActivityColor : Colors.Transparent;
            BuyStuffBackgroundColor = ActivityProfile == "buystuff" ? ActivityColor : Colors.Transparent;
            ContractsBackgroundColor = ActivityProfile == "contracts" ? ActivityColor : Colors.Transparent;
            ImportantTasksBackgroundColor = ActivityProfile == "importanttasks" ? ActivityColor : Colors.Transparent;
            GardeningBackgroundColor = ActivityProfile == "gardening" ? ActivityColor : Colors.Transparent;            
            LoveBackgroundColor = ActivityProfile == "love" ? ActivityColor : Colors.Transparent;
            MeetingsBackgroundColor = ActivityProfile == "meetings" ? ActivityColor : Colors.Transparent;
            NewIdeasBackgroundColor = ActivityProfile == "newideas" ? ActivityColor : Colors.Transparent;
            TechBackgroundColor = ActivityProfile == "tech" ? ActivityColor : Colors.Transparent;
            

            // Notify the UI to update
            OnPropertyChanged(nameof(NoPresetBackgroundColor));
            OnPropertyChanged(nameof(BeautyBackgroundColor));
            OnPropertyChanged(nameof(BuyStuffBackgroundColor));
            OnPropertyChanged(nameof(ContractsBackgroundColor));
            OnPropertyChanged(nameof(ImportantTasksBackgroundColor));
            OnPropertyChanged(nameof(GardeningBackgroundColor));
            OnPropertyChanged(nameof(LoveBackgroundColor));
            OnPropertyChanged(nameof(MeetingsBackgroundColor));
            OnPropertyChanged(nameof(NewIdeasBackgroundColor));
            OnPropertyChanged(nameof(TechBackgroundColor));
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
            this.MonthName = startOfMonth.ToString("MMMM", new CultureInfo("lt-LT"));
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
            UpdateCalendar(year, month);
        }

        private void BeautyRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "beauty";
            UpdateCalendar(year, month);
        }

        private void BuyStuffRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "buystuff";
            UpdateCalendar(year, month);
        }

        private void ContractsRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "contracts";
            UpdateCalendar(year, month);
        }

        private void ImportantTasksRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "importanttasks";
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

        private void MeetingsRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "meetings";
            UpdateCalendar(year, month);
        }

        private void NewIdeasRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "newideas";
            UpdateCalendar(year, month);
        }

        private void TechnologiesRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "tech";
            UpdateCalendar(year, month);
        }

        private void NoPresetRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            ActivityProfile = "nopreset";
            UpdateCalendar(year, month);
        }

        private async void ToggleGridAndAnimateArrow()
        {
            if (isProfileGridVisible)
            {
                // Move the arrow down before hiding the grid
                await arrowImage.TranslateTo(0, 10, 100); // Adjust these values as needed
                await arrowImage.TranslateTo(0, 0, 100); // Return to the original position

                // Hide the grid
                await profileGrid.TranslateTo(0, profileGrid.Height, 250, Easing.SinIn);
                profileGrid.IsVisible = false;

                // Rotate the arrow back to its original orientation
                await arrowImage.RotateTo(0, 200);
            }
            else
            {
                // Make the grid visible before animating it
                profileGrid.IsVisible = true;
                profileGrid.TranslationY = profileGrid.Height; // Start off-screen

                // Move the arrow up before showing the grid
                await arrowImage.TranslateTo(0, -10, 100); // Adjust these values as needed
                await arrowImage.TranslateTo(0, 0, 100); // Return to the original position

                // Animate the grid into view
                await profileGrid.TranslateTo(0, 0, 250, Easing.SinOut);

                // Rotate the arrow 180 degrees to indicate the grid is open
                await arrowImage.RotateTo(180, 200);
            }

            isProfileGridVisible = !isProfileGridVisible;
        }

        private void OnArrowTapped(object sender, EventArgs e)
        {
            ToggleGridAndAnimateArrow();
        }

        private async void HideGridWithAnimation()
        {
            // Step 1: Animate the arrow up
            await arrowImage.TranslateTo(0, -30, 200); // Adjust values as needed

            // Step 2: Animate the arrow down
            await arrowImage.TranslateTo(0, 0, 200); // Return to original position

            // Step 3: Rotate the arrow 180 degrees back
            await arrowImage.RotateTo(0, 200); // This completes the rotation back to 0 degrees

            // Hide the Grid
            profileGrid.IsVisible = false;
        }

        private void ApplyBackgroundColor_Clicked(object sender, EventArgs e)
        {
            // Get the HEX code from the entry
            string hexCode = hexColorEntry.Text;

            // Check if the HEX code is valid
            if (!string.IsNullOrWhiteSpace(hexCode) && hexCode.StartsWith("#") && (hexCode.Length == 7 || hexCode.Length == 9))
            {
                // Convert HEX to Color
                Color newBackgroundColor = Color.FromArgb(hexCode);

                // Update the BackgroundColor of the page or specific element
                PageBackgroundColor1 = newBackgroundColor;
            }
            else
            {
                // Handle invalid HEX code (optional)
                DisplayAlert("Error", "Invalid HEX code. Please enter a valid HEX color code.", "OK");
            }
        }

        private void ApplyBackgroundColor_Clicked2(object sender, EventArgs e)
        {
            // Get the HEX code from the entry
            string hexCode = hexColorEntry2.Text;

            // Check if the HEX code is valid
            if (!string.IsNullOrWhiteSpace(hexCode) && hexCode.StartsWith("#") && (hexCode.Length == 7 || hexCode.Length == 9))
            {
                // Convert HEX to Color
                Color newBackgroundColor = Color.FromArgb(hexCode);

                // Update the BackgroundColor of the page or specific element
                PageBackgroundColor2 = newBackgroundColor;
            }
            else
            {
                // Handle invalid HEX code (optional)
                DisplayAlert("Error", "Invalid HEX code. Please enter a valid HEX color code.", "OK");
            }
        }

        private void ApplyBackgroundColor_Clicked3(object sender, EventArgs e)
        {
            // Get the HEX code from the entry
            string hexCode = hexColorEntry3.Text;

            // Check if the HEX code is valid
            if (!string.IsNullOrWhiteSpace(hexCode) && hexCode.StartsWith("#") && (hexCode.Length == 7 || hexCode.Length == 9))
            {
                // Convert HEX to Color
                Color newBackgroundColor = Color.FromArgb(hexCode);

                // Update the BackgroundColor of the page or specific element
                MonthNameColor = newBackgroundColor;
            }
            else
            {
                // Handle invalid HEX code (optional)
                DisplayAlert("Error", "Invalid HEX code. Please enter a valid HEX color code.", "OK");
            }
        }

        private void ApplyBackgroundColor_Clicked4(object sender, EventArgs e)
        {
            // Get the HEX code from the entry
            string hexCode = hexColorEntry4.Text;

            // Check if the HEX code is valid
            if (!string.IsNullOrWhiteSpace(hexCode) && hexCode.StartsWith("#") && (hexCode.Length == 7 || hexCode.Length == 9))
            {
                // Convert HEX to Color
                Color newBackgroundColor = Color.FromArgb(hexCode);

                // Update the BackgroundColor of the page or specific element
                WeekdaysColor = newBackgroundColor;
            }
            else
            {
                // Handle invalid HEX code (optional)
                DisplayAlert("Error", "Invalid HEX code. Please enter a valid HEX color code.", "OK");
            }
        }


        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            this.GridColors.IsVisible = !this.GridColors.IsVisible;
        }

        private void ApplyBackgroundColor_Clicked5(object sender, EventArgs e)
        {
            // Get the HEX code from the entry
            string hexCode = hexColorEntry5.Text;

            // Check if the HEX code is valid
            if (!string.IsNullOrWhiteSpace(hexCode) && hexCode.StartsWith("#") && (hexCode.Length == 7 || hexCode.Length == 9))
            {
                // Convert HEX to Color
                Color newBackgroundColor = Color.FromArgb(hexCode);

                // Update the BackgroundColor of the page or specific element
                NoPresetBackgroundColor = newBackgroundColor;
            }
            else
            {
                // Handle invalid HEX code (optional)
                DisplayAlert("Error", "Invalid HEX code. Please enter a valid HEX color code.", "OK");
            }
        }
    }    
}
