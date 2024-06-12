using Astrodaiva.Data;

namespace Astrodaiva
{
    public partial class App : Application
    {
        public static AppData AppData { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
