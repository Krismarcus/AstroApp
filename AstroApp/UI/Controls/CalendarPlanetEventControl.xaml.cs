using AstroApp.Data.Models;
using System.ComponentModel;

namespace AstroApp.UI.Controls;

public partial class CalendarPlanetEventControl : ContentView
{
    public static readonly BindableProperty PlanetEventUnitProperty = BindableProperty.Create(nameof(PlanetEventUnit), typeof(PlanetEvent), typeof(CalendarPlanetEventControl));
    public PlanetEvent PlanetEventUnit
    {
        get => (PlanetEvent)GetValue(CalendarPlanetEventControl.PlanetEventUnitProperty);
        set => SetValue(CalendarPlanetEventControl.PlanetEventUnitProperty, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public CalendarPlanetEventControl()
    {
        InitializeComponent();
        this.BindingContext = this;
    }
}