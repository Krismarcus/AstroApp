using Astrodaiva.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools
{
    public static class ServiceHelper
    {
        public static IServiceProvider Services { get; private set; } = default!;

        public static void Initialize(IServiceProvider services) => Services = services;

        public static T GetRequiredService<T>() where T : notnull =>
            Services.GetRequiredService<T>();
    }
    public class NoopOrientationService : IOrientationService
    {
        public void LockLandscape() { }
        public void LockPortrait() { }
        public void Unlock() { }
    }

}
