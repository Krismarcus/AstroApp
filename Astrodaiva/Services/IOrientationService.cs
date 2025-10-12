using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.Services
{
    public interface IOrientationService
    {
        void LockLandscape();
        void LockPortrait();
        void Unlock();
    }
}
