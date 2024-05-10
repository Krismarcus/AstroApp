using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using AstroApp.UI.Tools.Converters;
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
                VerticalOptions = LayoutOptions.FillAndExpand,
                CornerRadius = 20
            };

            var label = new Label
            {
                Text = segment.ZodiacStartDate.Day.ToString(),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var image = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            image.BindingContext = segment; // Set each image's context to its corresponding segment
            image.SetBinding(Image.SourceProperty, new Binding("ZodiacSign", BindingMode.Default, new EnumToImageConverter()));

            // Adding a Grid to hold both the BoxView and the Label
            var cellGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Setup rows within the cellGrid to allocate space for each control
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) });

            // Add controls to the cellGrid
            cellGrid.Add(boxView, 0, 0);    // BoxView spans both rows
            Grid.SetColumnSpan(boxView, 2);
            cellGrid.Add(label, 0, 0);      // Label is in the second row
            cellGrid.Add(image, 1, 0);      // Image is in the first row

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