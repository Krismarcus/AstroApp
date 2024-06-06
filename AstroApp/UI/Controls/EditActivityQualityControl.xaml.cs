using AstroApp.Data.Enums;
using Microsoft.Maui.Controls;
using System;

namespace AstroApp.UI.Controls
{
    public partial class EditActivityQualityControl : ContentView
    {
        public static readonly BindableProperty ActivityQualityProperty =
        BindableProperty.Create(
            nameof(ActivityQuality),
            typeof(ActivityQuality),
            typeof(EditActivityQualityControl),
            ActivityQuality.Neutral,
            BindingMode.TwoWay,
            propertyChanged: OnActivityQualityChanged);

        public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(EditActivityQualityControl),
            default(ImageSource),
            BindingMode.OneWay);

        public ActivityQuality ActivityQuality
        {
            get => (ActivityQuality)GetValue(ActivityQualityProperty);
            set => SetValue(ActivityQualityProperty, value);
        }

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }


        public EditActivityQualityControl()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private static void OnActivityQualityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EditActivityQualityControl)bindable;
            control.UpdateBackground();
        }

        private void UpdateBackground()
        {
            OnPropertyChanged(nameof(ActivityQuality));
        }

        private void OnActivityIconTapped(object sender, EventArgs e)
        {
            CycleActivityQuality();
        }

        private void CycleActivityQuality()
        {
            var currentQuality = ActivityQuality;
            var nextQuality = (int)currentQuality + 1;

            if (nextQuality > (int)ActivityQuality.Bad)
            {
                nextQuality = (int)ActivityQuality.Neutral;
            }

            ActivityQuality = (ActivityQuality)nextQuality;
        }
    }
}
