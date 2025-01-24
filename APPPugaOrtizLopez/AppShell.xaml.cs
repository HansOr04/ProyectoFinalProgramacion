namespace APPPugaOrtizLopez
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("Login", typeof(Views.Auth.LoginPage));
            Routing.RegisterRoute("Register", typeof(Views.Auth.RegisterPage));
            Routing.RegisterRoute("AllFlats", typeof(Views.Flats.AllFlatsPage));
            Routing.RegisterRoute("CreateFlat", typeof(Views.Flats.CreateFlatPage));
            Routing.RegisterRoute("FlatDetails", typeof(Views.Flats.FlatDetailsPage));
        }
    }
}