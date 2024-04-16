using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Pages;

public partial class YearPage : ContentPage
{
    public ObservableCollection<AstroEvent> ActiveAstroEvents { get; set; }

    public YearPage()
	{        
        InitializeComponent();
        Initialize();
        this.BindingContext = this;        
	}

    private void Initialize()
    {
       this.ActiveAstroEvents = App.AppData.AppDB.AstroEventsDB;     
    }    
}