using AstroApp.Data.Enums;
using System.ComponentModel;

namespace AstroApp.UI.Controls;

public partial class PlanetEventControl : ContentView, INotifyPropertyChanged
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

    public PlanetEventControl()
    {
        InitializeComponent();
        PopulatePickers();
    }


    public void PopulatePickers()
    {
        foreach (Planet planet in Enum.GetValues(typeof(Planet)))
        {
            PlanetOnePicker.Items.Add(planet.ToString());
            PlanetTwoPicker.Items.Add(planet.ToString());
        }

        foreach (AspectSymbol aspect in Enum.GetValues(typeof(AspectSymbol)))
        {
            AspectPicker.Items.Add(aspect.ToString());
        }
    }
}