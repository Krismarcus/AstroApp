using Astrodaiva.Data.Enums;
using Astrodaiva.Data.Models;
using Astrodaiva.UI.Tools;
using Astrodaiva.UI.Tools.Converters;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;

namespace Astrodaiva.UI.Controls;

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

    public event EventHandler<ZodiacSegment> SegmentClicked;    

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

        // Calculate the total duration in days
        DateTime minDate = ZodiacSegments.Min(s => s.ZodiacStartDate);
        DateTime maxDate = ZodiacSegments.Max(s => s.ZodiacEndDate);
        double totalDays = (maxDate - minDate).TotalDays;

        foreach (var segment in ZodiacSegments)
        {
            double segmentDuration = (segment.ZodiacEndDate - segment.ZodiacStartDate).TotalDays;
            double widthFraction = segmentDuration / totalDays;

            var columnDefinition = new ColumnDefinition { Width = new GridLength(widthFraction, GridUnitType.Star) };
            MainGrid.ColumnDefinitions.Add(columnDefinition);

            var cellGrid = CreateSegmentCell(segment);
            MainGrid.Add(cellGrid, MainGrid.ColumnDefinitions.Count - 1, 0);
        }
    }

    private Grid CreateSegmentCell(ZodiacSegment segment)
    {
        var border = new Border
        {
            BackgroundColor = GetColorForZodiacSign(segment.ZodiacSign),
            StrokeThickness = 0,
            VerticalOptions = LayoutOptions.FillAndExpand,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) }
        };

        var textColor = GetResourceColor("PrimaryLightText");

        var label = new Label
        {
            Text = segment.ZodiacStartDate.Day.ToString(),
            TextColor = textColor,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(10, 0, 0, 0)
        };

        var image = new Image
        {
            HeightRequest = 25,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        image.BindingContext = segment;
        image.SetBinding(Image.SourceProperty, new Binding("ZodiacSign", BindingMode.Default, new EnumToImageConverter()));

        var cellGrid = new Grid
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Padding = new Thickness(0)
        };

        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) => OnSegmentTapped(segment, border);
        border.GestureRecognizers.Add(tapGestureRecognizer);
        label.GestureRecognizers.Add(tapGestureRecognizer);
        image.GestureRecognizers.Add(tapGestureRecognizer);

        if (segment.Duration > 40)
        {

            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            cellGrid.Add(border, 0, 0);
            Grid.SetColumnSpan(border, 3);
            cellGrid.Add(label, 0, 0);
            cellGrid.Add(image, 1, 0);

            return cellGrid;
        }

        else if (25 < segment.Duration && segment.Duration <= 40)
        {
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });            
            cellGrid.Add(border, 0, 0);
            Grid.SetColumnSpan(border, 2);            
            cellGrid.Add(label, 0, 0);
            cellGrid.Add(image, 1, 0);

            return cellGrid;
        }

        else if (10 < segment.Duration && segment.Duration <= 25)
        {
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            cellGrid.Add(border, 0, 0);
            cellGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Row for the image
            cellGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Row for the label          
            Grid.SetRowSpan(border, 2);
            cellGrid.Add(image, 0, 1);
            label.HorizontalTextAlignment = TextAlignment.Start;
            label.VerticalTextAlignment = TextAlignment.Center;            
            cellGrid.Add(label, 0, 0);

            return cellGrid;
        }
        
        else
        {            
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            cellGrid.Add(border, 0, 0);
            Grid.SetColumnSpan(border, 1);            
            cellGrid.Add(label, 0, 0);

            return cellGrid;
        }
    }

    private Color GetResourceColor(string key)
    {
    Color textColor = new Color();
    if (Application.Current.Resources.TryGetValue(key, out var colorValue) && colorValue is Color myColor)
        {
        textColor = myColor;
        }
    return textColor;
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
    private void OnSegmentTapped(ZodiacSegment segment, Border border)
    {
        SegmentSelectionManager.Instance.SelectSegment(border);
        SegmentClicked?.Invoke(this, segment);        
    }
}