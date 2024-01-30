using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Controls;

public partial class PlanetInZodiacControl : ContentView
{    
	public PlanetInZodiacControl()
	{
		InitializeComponent();
        PopulatePicker();
    }

    public void PopulatePicker()
    {
        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.PlanetInZodiacPicker.Items.Add(zodiacSign.ToString());
        }
    }
}