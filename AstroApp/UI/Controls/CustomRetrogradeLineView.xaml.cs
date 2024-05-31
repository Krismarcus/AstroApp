using AstroApp.Data.Models;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace AstroApp.UI.Controls;

public partial class CustomRetrogradeLineView : ContentView
{
    public static readonly BindableProperty RetrogradeSegmentsProperty = BindableProperty.Create(
            nameof(RetrogradeSegments),
            typeof(ObservableCollection<RetrogradeSegment>),
            typeof(CustomRetrogradeLineView),
            new ObservableCollection<RetrogradeSegment>(),
            propertyChanged: OnRetrogradeSegmentsChanged);

    public ObservableCollection<RetrogradeSegment> RetrogradeSegments
    {
        get => (ObservableCollection<RetrogradeSegment>)GetValue(RetrogradeSegmentsProperty);
        set => SetValue(RetrogradeSegmentsProperty, value);
    }

    private static void OnRetrogradeSegmentsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomRetrogradeLineView)bindable;
        control.BuildSegments();
        
    }

    public event EventHandler<RetrogradeSegment> SegmentClicked;

    public CustomRetrogradeLineView()
	{
		InitializeComponent();
	}

    private void BuildSegments()
    {
        MainGrid.ColumnDefinitions.Clear();
        MainGrid.Children.Clear();

        // Calculate the total duration in days
        DateTime minDate = RetrogradeSegments.Min(s => s.RetrogradeStartDate);
        DateTime maxDate = RetrogradeSegments.Max(s => s.RetrogradeEndDate);
        double totalDays = (maxDate - minDate).TotalDays;

        foreach (var segment in RetrogradeSegments)
        {
            double segmentDuration = (segment.RetrogradeEndDate - segment.RetrogradeStartDate).TotalDays;
            double widthFraction = segmentDuration / totalDays;

            var columnDefinition = new ColumnDefinition { Width = new GridLength(widthFraction, GridUnitType.Star) };            
            MainGrid.ColumnDefinitions.Add(columnDefinition);

            if (segment.IsRetrograde == true)
            {
                var cellGrid = CreateSegmentCell(segment);
                MainGrid.Add(cellGrid, MainGrid.ColumnDefinitions.Count - 1, 0);
            }
        }
    }

    private Grid CreateSegmentCell(RetrogradeSegment segment)
    {        
            var boxView = new Border
            {
                BackgroundColor = GetColorForZodiacSign(segment.IsRetrograde),
                StrokeThickness = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) }
            };

            var textColor = GetResourceColor("PrimaryLightText");

            var label = new Label
            {
                Text = segment.RetrogradeStartDate.Day.ToString(),
                TextColor = textColor,
                FontSize = 12,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };

            var label2 = new Label
            {
                Text = segment.RetrogradeEndDate.Day.ToString(),
                TextColor = textColor,
                FontSize = 12,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };

            var image = new Image
            {
                Source = "retrograde.png",
                HeightRequest = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };


            var cellGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => OnSegmentTapped(segment);
            boxView.GestureRecognizers.Add(tapGestureRecognizer);

            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            cellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            cellGrid.Add(boxView, 0, 0);
            Grid.SetColumnSpan(boxView, 3);
            cellGrid.Add(label, 0, 0);
            cellGrid.Add(image, 1, 0);
            cellGrid.Add(label2, 2, 0);

            return cellGrid;
        
    }

    private Color GetColorForZodiacSign(bool isRetrograde)
    {
        if (isRetrograde == true)
        {
            return Colors.DimGray;
        }
        return Colors.Transparent;
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

    private void OnSegmentTapped(RetrogradeSegment segment)
    {
        SegmentClicked?.Invoke(this, segment);
    }
}