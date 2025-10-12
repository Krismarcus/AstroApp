using System.Linq;
using UIKit;

namespace Astrodaiva.Services
{
    public class OrientationService : IOrientationService
    {
        public void LockLandscape()
        {
            OrientationState.SupportedMask = UIInterfaceOrientationMask.Landscape;

            var scene = UIApplication.SharedApplication.ConnectedScenes
                .OfType<UIWindowScene>()
                .FirstOrDefault();

            scene?.RequestGeometryUpdate(
                new UIWindowSceneGeometryPreferencesIOS(UIInterfaceOrientationMask.Landscape),
                null
            );
        }

        public void Unlock()
        {
            OrientationState.SupportedMask = UIInterfaceOrientationMask.All;

            var scene = UIApplication.SharedApplication.ConnectedScenes
                .OfType<UIWindowScene>()
                .FirstOrDefault();

            scene?.RequestGeometryUpdate(
                new UIWindowSceneGeometryPreferencesIOS(UIInterfaceOrientationMask.All),
                null
            );
        }
    }
}
