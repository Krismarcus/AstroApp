using AstroApp.Data.Models;

namespace AstroApp.UI.Controls;

public partial class EditMoonDaysControl : ContentView
{
    private MoonDay moonDay;

    public MoonDay MoonDay
    {
        get { return moonDay; }
        set
        {
            if (moonDay != value)
            {
                moonDay = value;
                OnPropertyChanged(nameof(MoonDay));

            }
        }
    }

    public EditMoonDaysControl()
	{
		InitializeComponent();
        BindingContext = this;
    }

    internal void AddMoonDayDetails(MoonDay moonDay)
    {
        this.MoonDay = moonDay;
    }
}