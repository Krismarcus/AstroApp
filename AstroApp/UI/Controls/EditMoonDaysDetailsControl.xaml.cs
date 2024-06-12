using AstroApp.Data.Models;

namespace AstroApp.UI.Controls;

public partial class EditMoonDaysControl : ContentView
{
    private MoonDayDetails moonDaydetails;

    public MoonDayDetails MoonDayDetails
    {
        get { return moonDaydetails; }
        set
        {
            if (moonDaydetails != value)
            {
                moonDaydetails = value;
                OnPropertyChanged(nameof(MoonDayDetails));

            }
        }
    }

    public EditMoonDaysControl()
	{
		InitializeComponent();
        BindingContext = this;
    }

    internal void AddMoonDayDetails(MoonDayDetails moonDay)
    {
        this.MoonDayDetails = moonDay;
    }
}