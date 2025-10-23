using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.Services
{
    public class NoopOrientationService : IOrientationService
    {
        public event EventHandler<AppDisplayOrientation> OrientationChanged;

        public AppDisplayOrientation CurrentOrientation => AppDisplayOrientation.Unknown;

        public void StartListening()
        {
            // No operation for platforms that don't support orientation changes
        }

        public void StopListening()
        {
            // No operation for platforms that don't support orientation changes
        }

        public void LockLandscape()
        {
            // No operation for platforms that don't support orientation locking
        }

        public void LockPortrait()
        {
            // No operation for platforms that don't support orientation locking
        }

        public void Unlock()
        {
            // No operation for platforms that don't support orientation unlocking
        }
    }
}
