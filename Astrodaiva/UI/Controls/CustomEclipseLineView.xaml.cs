using Astrodaiva.Data.Models;
using Astrodaiva.UI.Tools;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;

namespace Astrodaiva.UI.Controls;

public partial class CustomEclipseLineView : ContentView
{
    public event Action<List<EclipseSegment>>? EclipseLineTapped;
    public event Action<EclipseSegment>? MarkerTapped;

    public static readonly BindableProperty SegmentsProperty =
        BindableProperty.Create(
            nameof(Segments),
            typeof(ObservableCollection<EclipseSegment>),
            typeof(CustomEclipseLineView),
            defaultValue: null,
            propertyChanged: (b, o, n) => ((CustomEclipseLineView)b).RequestBuild());

    public ObservableCollection<EclipseSegment>? Segments
    {
        get => (ObservableCollection<EclipseSegment>?)GetValue(SegmentsProperty);
        set => SetValue(SegmentsProperty, value);
    }

    // Keep baseline geometry in ONE place so markers always align
    const double LineY = 17;
    const double LineH = 10;
    const double MarkerSize = 28;

    bool _buildQueued;

    public CustomEclipseLineView()
    {
        InitializeComponent();

        SizeChanged += (_, __) => RequestBuild();
        HandlerChanged += (_, __) => RequestBuild();
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        RequestBuild();
    }

    void RequestBuild()
    {
        if (_buildQueued) return;
        _buildQueued = true;

        Dispatcher.Dispatch(() =>
        {
            _buildQueued = false;
            BuildSegments();
        });
    }

    void BuildSegments()
    {
        if (SegmentContainer == null)
            return;

        if (Segments == null || Segments.Count == 0)
        {
            SegmentContainer.Children.Clear();
            return;
        }

        // If not measured yet, try again next UI tick
        if (SegmentContainer.Width <= 0 || SegmentContainer.Height <= 0)
        {
            RequestBuild();
            return;
        }

        SegmentContainer.Children.Clear();

        // 1) baseline (always re-added after clear)
        var baselineBorder = CreateBaseLine();
        SegmentContainer.Children.Add(baselineBorder);

        // 2) markers (always re-added after clear)
        foreach (var seg in Segments.OrderBy(s => s.StartDate))
            SegmentContainer.Children.Add(CreateMarker(seg));
    }

    Border CreateBaseLine()
    {
        var line = new BoxView
        {
            Color = ColorManager.GetResourceColor("ShadedBackground", Colors.Black),
            HeightRequest = LineH,
            CornerRadius = 2
        };

        var border = new Border
        {
            StrokeThickness = 0,
            Padding = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 2 },
            Content = line
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) =>
        {
            SegmentSelectionManager.Instance.SelectSegment(border);
            OnEclipseLineTapped();
        };
        border.GestureRecognizers.Add(tap);

        // Full width baseline using WidthProportional
        AbsoluteLayout.SetLayoutBounds(border, new Rect(0, LineY, 1, LineH));
        AbsoluteLayout.SetLayoutFlags(border, AbsoluteLayoutFlags.WidthProportional);

        return border;
    }

    View CreateMarker(EclipseSegment seg)
    {
        int yearDays = DateTime.IsLeapYear(seg.StartDate.Year) ? 366 : 365;

        // 0..1 inclusive range, stable at edges
        double pos = (double)(seg.StartDate.DayOfYear - 1) / (yearDays - 1);

        // Center marker vertically on the baseline
        double markerY = LineY + (LineH / 2) - (MarkerSize / 2);

        var border = new Border
        {
            WidthRequest = MarkerSize,
            HeightRequest = MarkerSize,
            StrokeThickness = 0,
            Stroke = new SolidColorBrush(Colors.DarkBlue),
            StrokeShape = new RoundRectangle { CornerRadius = MarkerSize / 2 },
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
            SegmentSelectionManager.Instance.SelectSegment(border);
            MarkerTapped?.Invoke(seg);
        };
        border.GestureRecognizers.Add(tap);

        // IMPORTANT:
        // X is proportional, Y is absolute -> perfect alignment
        AbsoluteLayout.SetLayoutBounds(border, new Rect(pos, markerY, MarkerSize, MarkerSize));
        AbsoluteLayout.SetLayoutFlags(border, AbsoluteLayoutFlags.XProportional);

        // Center the marker around the X proportional position
        border.AnchorX = 0.5;
        border.AnchorY = 0;

        return border;
    }

    void OnEclipseLineTapped()
    {
        if (Segments == null || Segments.Count == 0)
            return;

        EclipseLineTapped?.Invoke(Segments.OrderBy(s => s.StartDate).ToList());
    }
}
