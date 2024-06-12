using AstroApp.Data.Models;
using AstroApp.UI.Controls;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Pages;

public partial class MoonDaysEditPage : ContentPage
{
    public ObservableCollection<MoonDayDetails> MoonDays { get; set; }

    public MoonDaysEditPage()
    {
        InitializeComponent();
        Initialize();
        this.BindingContext = this;
    }

    public async void Initialize()
    {
        //this.MoonDays = await appActions.LoadMoonDaysAsync();
        PopulateMoonDaysList();
    }

    private void PopulateMoonDaysList()
    {
        this.MoonDays = App.AppData.AppDB.MoonDayDetailsDB;
        if (this.MoonDays == null)
        {
            this.MoonDays = new ObservableCollection<MoonDayDetails>();

            for (int i = 1; i <= 30; i++)            
            {
                MoonDayDetails moonDay = new MoonDayDetails { MoonDay = i };
                MoonDays.Add(moonDay);
            }
        }

        for (int i = 0; i < MoonDays.Count; i++)
        {
            EditMoonDaysControl editMoonDaysControl = new EditMoonDaysControl();

            MoonDayDetails moonDay = MoonDays[i];
            if (moonDay != null)
            {
                editMoonDaysControl.AddMoonDayDetails(moonDay);
            }

            this.EventList.Add(editMoonDaysControl);
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var appActions = new Services.AppActions();
        await appActions.SaveMoonDaysAsync(MoonDays);
        await Application.Current.MainPage.DisplayAlert("Success", "Moon days saved succesfully", "OK");
    }    
}