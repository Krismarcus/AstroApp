using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.Services
{
    public class OrientationService : IOrientationService
    {
        public event EventHandler<AppDisplayOrientation> OrientationChanged;

        public AppDisplayOrientation CurrentOrientation { get; private set; }

        public void StartListening()
        {
            DeviceDisplay.Current.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
            UpdateOrientation(DeviceDisplay.Current.MainDisplayInfo);
        }

        public void StopListening()
        {
            DeviceDisplay.Current.MainDisplayInfoChanged -= OnMainDisplayInfoChanged;
        }

        private void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            UpdateOrientation(e.DisplayInfo);
        }

        private void UpdateOrientation(DisplayInfo displayInfo)
        {
            var newOrientation = displayInfo.Orientation switch
            {
                Microsoft.Maui.Devices.DisplayOrientation.Portrait => AppDisplayOrientation.Portrait,
                Microsoft.Maui.Devices.DisplayOrientation.Landscape => AppDisplayOrientation.Landscape,
                _ => AppDisplayOrientation.Unknown
            };

            if (CurrentOrientation != newOrientation)
            {
                CurrentOrientation = newOrientation;
                OrientationChanged?.Invoke(this, newOrientation);
            }
        }

        public void LockLandscape()
        {
#if ANDROID
            LockLandscapeAndroid();
#endif
        }

        public void LockPortrait()
        {
#if ANDROID
            LockPortraitAndroid();
#endif
        }

        public void Unlock()
        {
#if ANDROID
            UnlockAndroid();
#endif
        }

#if ANDROID
        private void LockLandscapeAndroid()
        {
            if (Microsoft.Maui.ApplicationModel.Platform.CurrentActivity != null)
            {
                Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            }
        }

        private void LockPortraitAndroid()
        {
            if (Microsoft.Maui.ApplicationModel.Platform.CurrentActivity != null)
            {
                Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            }
        }

        private void UnlockAndroid()
        {
            if (Microsoft.Maui.ApplicationModel.Platform.CurrentActivity != null)
            {
                Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Unspecified;
            }
        }
#endif
    }
}
