using Android.Graphics.Fonts;
using Astrodaiva.Data.Models;
using Astrodaiva.UI.Tools;
using Java.Time.Format;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;

namespace Astrodaiva.UI.Controls;

public partial class CustomEclipseLineView : ContentView
{
    public event Action<List<EclipseSegment>> EclipseLineTapped;
    public event Action<EclipseSegment>? MarkerTapped;

    public static readonly BindableProperty SegmentsProperty =
        BindableProperty.Create(
            nameof(Segments),
            typeof(ObservableCollection<EclipseSegment>),
            typeof(CustomEclipseLineView),
            propertyChanged: (b, o, n) => ((CustomEclipseLineView)b).Redraw());

    public ObservableCollection<EclipseSegment> Segments
    {
        get => (ObservableCollection<EclipseSegment>)GetValue(SegmentsProperty);
        set => SetValue(SegmentsProperty, value);
    }

    public CustomEclipseLineView()
    {
        InitializeComponent();
        Loaded += (_, __) => Redraw();
        SizeChanged += (_, __) => Redraw();
    }

    async void Redraw()
    {
        if (Segments == null) return;

        int t = 0;
        while (SegmentContainer.Width < 10 && t < 30)
        {
            await Task.Delay(30);
            t++;
        }

        SegmentContainer.Children.Clear();
        DrawBaseLine();

        foreach (var seg in Segments)
            DrawMarker(seg);
    }

    // -------------------------
    // BASE YELLOW LINE (tappable)
    // -------------------------
    void DrawBaseLine()
    {
        var line = new BoxView
        {
            Color = ColorManager.GetResourceColor("ShadedBackground", Colors.Black),
            HeightRequest = 10,
            CornerRadius = 2
        };

        var eclipseLineBorder = new Border
        {
            StrokeThickness = 0,
            Padding = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 2 },
            Content = line
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) =>
        {
            // highlight the whole line
            SegmentSelectionManager.Instance.SelectSegment(eclipseLineBorder);

            // send all eclipses to YearPage
            OnEclipseLineTapped();
        };
        eclipseLineBorder.GestureRecognizers.Add(tap);

        // IMPORTANT: width = 1 with WidthProportional
        AbsoluteLayout.SetLayoutBounds(eclipseLineBorder, new Rect(0, 17, 1, 10));
        AbsoluteLayout.SetLayoutFlags(eclipseLineBorder, AbsoluteLayoutFlags.WidthProportional);

        SegmentContainer.Children.Add(eclipseLineBorder);
    }

    // -------------------------
    // CIRCLES FOR EACH ECLIPSE DAY
    // -------------------------
    void DrawMarker(EclipseSegment seg)
    {
        int yearDays = DateTime.IsLeapYear(seg.StartDate.Year) ? 366 : 365;
        double pos = (double)seg.StartDate.DayOfYear / yearDays;

        double size = 28;

        // border so we can highlight via SegmentSelectionManager
        var border = new Border
        {
            WidthRequest = size,
            HeightRequest = size,
            StrokeThickness = 0,
            Stroke = new SolidColorBrush(Colors.DarkBlue),
            StrokeShape = new RoundRectangle { CornerRadius = size / 2 },
            Background = new SolidColorBrush(ColorManager.GetResourceColor("GreyBackground", Colors.Black)),
            Padding = 0,
            Content = new Label
            {
                Text = seg.StartDate.Day.ToString(),
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = ColorManager.GetResourceColor("PrimaryLightText", Colors.Black),
            }
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) =>
        {
            // highlight just this marker
            SegmentSelectionManager.Instance.SelectSegment(border);

            // notify YearPage
            MarkerTapped?.Invoke(seg);
        };
        border.GestureRecognizers.Add(tap);

        // place in the middle above the line
        AbsoluteLayout.SetLayoutBounds(
            border,
            new Rect(pos * SegmentContainer.Width - size / 2, 5, size, size));
        AbsoluteLayout.SetLayoutFlags(border, AbsoluteLayoutFlags.None);

        SegmentContainer.Children.Add(border);
    }

    // called when baseline is tapped
    void OnEclipseLineTapped()
    {
        if (Segments == null || Segments.Count == 0)
            return;

        var list = Segments.OrderBy(s => s.StartDate).ToList();
        EclipseLineTapped?.Invoke(list);
    }
}
