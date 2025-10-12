using Android.Content.PM;
using Microsoft.Maui.ApplicationModel;

namespace Astrodaiva.Services
{
    public class OrientationService : IOrientationService
    {
        public void LockLandscape()
        {
            var activity = Platform.CurrentActivity;
            if (activity != null)
                activity.RequestedOrientation = ScreenOrientation.SensorLandscape;
        }

        public void LockPortrait()
        {
            var activity = Platform.CurrentActivity;
            if (activity != null)
                activity.RequestedOrientation = ScreenOrientation.SensorPortrait;
        }

        public void Unlock()
        {
            var activity = Platform.CurrentActivity;
            if (activity != null)
                activity.RequestedOrientation = ScreenOrientation.Unspecified;
        }
    }
}
