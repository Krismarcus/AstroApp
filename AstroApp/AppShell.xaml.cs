namespace AstroApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public bool IsAdmin
        {
            get
            {
#if ADMIN || DEBUG || RELEASE
                return true;
#else
            return false;
#endif
            }
        }
    }
}
