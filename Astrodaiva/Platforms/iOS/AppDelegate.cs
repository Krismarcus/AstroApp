using Astrodaiva.Platforms.iOS;
using Foundation;
using UIKit;

namespace Astrodaiva
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(
        UIApplication application,
        UIWindow forWindow)
        => Astrodaiva.Services.OrientationState.SupportedMask;
    }
}
