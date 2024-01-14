using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.ComponentModel;

namespace AstroApp.UI.Controls;

public partial class CalendarPlanetEventControl : ContentView
{
    private Planet planet1;
    public Planet Planet1
    {
        get { return planet1; }
        set
        {
            if (planet1 != value)
            {
                planet1 = value;
                OnPropertyChanged(nameof(Planet1));
            }
        }
    }

    private Planet planet2;
    public Planet Planet2
    {
        get { return planet2; }
        set
        {
            if (planet2 != value)
            {
                planet2 = value;
                OnPropertyChanged(nameof(Planet2));
            }
        }
    }

    private AspectSymbol aspectsymbol;
    public AspectSymbol AspectSymbol
    {
        get { return aspectsymbol; }
        set
        {
            if (aspectsymbol != value)
            {
                aspectsymbol = value;
                OnPropertyChanged(nameof(AspectSymbol));
            }
        }
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