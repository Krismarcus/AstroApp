using AstroApp.Data.Models;

namespace AstroApp.UI.Controls;

public partial class EventDetailsPlanetInZodiacControl : ContentView
{	
	public EventDetailsPlanetInZodiacControl()
	{
		InitializeComponent();
	}   

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        hoverLabel.IsVisible = !hoverLabel.IsVisible; // Toggle visibility on tap
    }
}