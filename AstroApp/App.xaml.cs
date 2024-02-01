using AstroApp.Data;

namespace AstroApp
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
