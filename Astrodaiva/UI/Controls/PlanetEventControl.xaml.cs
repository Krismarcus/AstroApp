using Astrodaiva.Data.Enums;
using System.ComponentModel;

namespace Astrodaiva.UI.Controls;

public partial class PlanetEventControl : ContentView
{
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