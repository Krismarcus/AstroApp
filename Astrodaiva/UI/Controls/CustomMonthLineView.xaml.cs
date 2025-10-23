using Astrodaiva.Data.Models;
using Astrodaiva.UI.Pages;
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
        var boxColor = GetResourceColor("PrimaryBackground");
        var textColor = GetResourceColor("ShadedBackground");
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
            Margin = new Thickness(0, 0, 0, 0)
        };

        var cellGrid = new Grid
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => OnMonthSegmentTapped(segment);
        cellGrid.GestureRecognizers.Add(tapGesture);

        cellGrid.Add(boxView);
        cellGrid.Add(label);

        return cellGrid;
    }

    private async void OnMonthSegmentTapped(MonthSegment segment)
    {
        var month = segment.MonthStartDate.Month;
        var year = segment.MonthStartDate.Year;
        await Shell.Current.GoToAsync($"//main?month={month}&year={year}");
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
}
