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
            Routing.RegisterRoute("CreateFlat", typeof(Views.Flats.CreateFlatPage));
            Routing.RegisterRoute("FlatDetails", typeof(Views.Flats.FlatDetailsPage));
        }
    }
}