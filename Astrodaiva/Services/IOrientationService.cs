using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.Services
{
    public interface IOrientationService
    {
        event EventHandler<AppDisplayOrientation> OrientationChanged;
        AppDisplayOrientation CurrentOrientation { get; }
        void StartListening();
        void StopListening();
        void LockLandscape();
        void LockPortrait();
        void Unlock();
    }

    public enum AppDisplayOrientation
    {
        Unknown,
        Portrait,
        Landscape
    }

}
