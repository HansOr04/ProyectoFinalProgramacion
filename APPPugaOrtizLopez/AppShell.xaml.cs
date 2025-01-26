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
    Routing.RegisterRoute("Register", typeof(Views.Auth.RegisterPage));
    Routing.RegisterRoute("CreateFlat", typeof(Views.Flats.CreateFlatPage));
    Routing.RegisterRoute("FlatDetails", typeof(Views.Flats.FlatDetailsPage));
    Routing.RegisterRoute("CallesGuardadas", typeof(Views.Guardados.CallesGuardadasPage));
    Routing.RegisterRoute("ListaCallesApi", typeof(Views.Guardados.ListaCallesApiPage));
}
    }
}