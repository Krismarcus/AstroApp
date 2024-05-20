using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AstroApp.UI.Controls;

public partial class EditPlanetInZodiacControl : ContentView
{
    public static readonly BindableProperty PlanetInZodiacUnitProperty = BindableProperty.Create(
            nameof(PlanetInZodiacUnit),
            typeof(PlanetInZodiac),
            typeof(EditPlanetInZodiacControl),
            default(PlanetInZodiac),
            BindingMode.TwoWay);


    public PlanetInZodiac PlanetInZodiacUnit
    {
        get => (PlanetInZodiac)GetValue(PlanetInZodiacUnitProperty);
        set => SetValue(PlanetInZodiacUnitProperty, value);
    }

    public EditPlanetInZodiacControl()
    {
        InitializeComponent();                
        PopulatePicker();
        this.BindingContext = this;
    }   

    private void PopulatePicker()
    {
        foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
        {
            this.ZodiacPicker.Items.Add(zodiacSign.ToString());
        }
    }
}