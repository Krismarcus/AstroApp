using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Controls;

public partial class CustomZodiacLineView : ContentView
{
    public static readonly BindableProperty ZodiacSegmentsProperty = BindableProperty.Create(
            nameof(ZodiacSegments),
            typeof(ObservableCollection<ZodiacSegment>),
            typeof(CustomZodiacLineView),
            new ObservableCollection<ZodiacSegment>(),
            propertyChanged: OnZodiacSegmentsChanged);

    public ObservableCollection<ZodiacSegment> ZodiacSegments
    {
        get => (ObservableCollection<ZodiacSegment>)GetValue(ZodiacSegmentsProperty);
        set => SetValue(ZodiacSegmentsProperty, value);
    }

    public CustomZodiacLineView()
    {
        InitializeComponent();
    }

    private static void OnZodiacSegmentsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomZodiacLineView)bindable;
        control.BuildSegments();
    }

    private void BuildSegments()
    {
        MainGrid.ColumnDefinitions.Clear();
        MainGrid.Children.Clear();

        int columnIndex = 0;
        foreach (var segment in ZodiacSegments)
        {
            var columnDefinition = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            MainGrid.ColumnDefinitions.Add(columnDefinition);

            var boxView = new BoxView
            {
                Color = GetColorForZodiacSign(segment.ZodiacSign),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var label = new Label
            {
                Text = segment.ZodiacSign.ToString(),
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            // Adding a Grid to hold both the BoxView and the Label
            var cellGrid = new Grid();
            cellGrid.Children.Add(boxView);
            cellGrid.Children.Add(label);

            MainGrid.Add(cellGrid, columnIndex, 0);
            columnIndex++;
        }
    }

    private Color GetColorForZodiacSign(ZodiacSign sign)
    {
        // Define colors for each zodiac sign here
        switch (sign)
        {
            case ZodiacSign.Aries: return Color.FromRgb(194, 41, 54);
            case ZodiacSign.Taurus: return Color.FromRgb(198, 136, 19);
            case ZodiacSign.Gemini: return Color.FromRgb(143, 145, 49);
            case ZodiacSign.Cancer: return Color.FromRgb(52, 114, 136);
            case ZodiacSign.Leo: return Color.FromRgb(194, 41, 54);
            case ZodiacSign.Virgo: return Color.FromRgb(198, 136, 19);
            case ZodiacSign.Libra: return Color.FromRgb(143, 145, 49);
            case ZodiacSign.Scorpio: return Color.FromRgb(52, 114, 136);
            case ZodiacSign.Sagittarius: return Color.FromRgb(194, 41, 54);
            case ZodiacSign.Capricorn: return Color.FromRgb(198, 136, 19);
            case ZodiacSign.Aquarius: return Color.FromRgb(143, 145, 49);
            case ZodiacSign.Pisces: return Color.FromRgb(52, 114, 136);            
            // Add cases for other signs
            default: return Colors.Gray;
        }
    }
}