using AstroApp.Data.Models;

namespace AstroApp;

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

    public AdminPage()
	{
		InitializeComponent();
        this.BindingContext = this;
    }
}