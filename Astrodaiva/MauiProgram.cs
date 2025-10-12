using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Astrodaiva.Services;
using Astrodaiva.UI.Tools;

namespace Astrodaiva
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("GillSans.otf", "GillSans");
                    fonts.AddFont("GillSans-Bold.ttf", "GillSansBold");
                });

#if ANDROID || IOS
            builder.Services.AddSingleton<IOrientationService, OrientationService>();
#else
            builder.Services.AddSingleton<IOrientationService, NoopOrientationService>();
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            ServiceHelper.Initialize(app.Services);
            return app;
        }
    }
    
}
