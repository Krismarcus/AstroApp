using Astrodaiva.Data.Models;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Astrodaiva.UI.Controls;

public partial class CustomMonthLineView : ContentView
{
    public static readonly BindableProperty MonthSegmentsProperty = BindableProperty.Create(
            nameof(MonthSegments),
            typeof(ObservableCollection<MonthSegment>),
            typeof(CustomMonthLineView),
            new ObservableCollection<MonthSegment>(),
            propertyChanged: OnMonthSegmentsChanged);

    public ObservableCollection<MonthSegment> MonthSegments
    {
        get => (ObservableCollection<MonthSegment>)GetValue(MonthSegmentsProperty);
        set => SetValue(MonthSegmentsProperty, value);
    }    

    public CustomMonthLineView()
    {
        InitializeComponent();
    }

    private static void OnMonthSegmentsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomMonthLineView)bindable;
        control.BuildSegments();
    }

    private void BuildSegments()
    {
        MainGrid.ColumnDefinitions.Clear();
        MainGrid.Children.Clear();

        // Calculate the total duration in days
        DateTime minDate = MonthSegments.Min(s => s.MonthStartDate);
        DateTime maxDate = MonthSegments.Max(s => s.MonthEndDate);
        double totalDays = (maxDate - minDate).TotalDays;

        foreach (var segment in MonthSegments)
        {
            double segmentDuration = (segment.MonthEndDate - segment.MonthStartDate).TotalDays;
            double widthFraction = segmentDuration / totalDays;

            var columnDefinition = new ColumnDefinition { Width = new GridLength(widthFraction, GridUnitType.Star) };
            MainGrid.ColumnDefinitions.Add(columnDefinition);

            var cellGrid = CreateSegmentCell(segment);
            MainGrid.Add(cellGrid, MainGrid.ColumnDefinitions.Count - 1, 0);
        }
    }

    private Grid CreateSegmentCell(MonthSegment segment)
    {
        var boxColor = GetResourceColor("PrimaryLightText");
        var textColor = GetResourceColor("PrimaryBackground");
        CultureInfo lithuanianCulture = new CultureInfo("lt-LT");

        var boxView = new Border
        {            
            BackgroundColor = boxColor,
            StrokeThickness = 0,
            VerticalOptions = LayoutOptions.FillAndExpand,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) }
        };        

        var label = new Label
        {
            Text = segment.MonthStartDate.ToString("MMMM", lithuanianCulture),
            TextColor = textColor,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(10, 0, 0, 0)
        };        

        var cellGrid = new Grid
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };       

        cellGrid.Add(boxView);        
        cellGrid.Add(label);        

        return cellGrid;
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


    //private Color GetColorForZodiacSign(ZodiacSign sign)
    //{
    //    // Define colors for each zodiac sign here
    //    switch (sign)
    //    {
    //        case ZodiacSign.Aries: return Color.FromRgb(194, 41, 54);
    //        case ZodiacSign.Taurus: return Color.FromRgb(198, 136, 19);
    //        case ZodiacSign.Gemini: return Color.FromRgb(143, 145, 49);
    //        case ZodiacSign.Cancer: return Color.FromRgb(52, 114, 136);
    //        case ZodiacSign.Leo: return Color.FromRgb(194, 41, 54);
    //        case ZodiacSign.Virgo: return Color.FromRgb(198, 136, 19);
    //        case ZodiacSign.Libra: return Color.FromRgb(143, 145, 49);
    //        case ZodiacSign.Scorpio: return Color.FromRgb(52, 114, 136);
    //        case ZodiacSign.Sagittarius: return Color.FromRgb(194, 41, 54);
    //        case ZodiacSign.Capricorn: return Color.FromRgb(198, 136, 19);
    //        case ZodiacSign.Aquarius: return Color.FromRgb(143, 145, 49);
    //        case ZodiacSign.Pisces: return Color.FromRgb(52, 114, 136);
    //        // Add cases for other signs
    //        default: return Colors.Gray;
    //    }
    //}    
}