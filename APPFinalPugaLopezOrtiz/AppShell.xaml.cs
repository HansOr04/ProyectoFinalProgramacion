using APPFinalPugaLopezOrtiz.Views;

namespace APPFinalPugaLopezOrtiz
{
    // AppShell.xaml.cs 
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("register", typeof(RegisterPage));
            Routing.RegisterRoute("departamento-detail", typeof(DepartamentoDetailPage));
            Routing.RegisterRoute("create-departamento", typeof(CreateDepartamentoPage));
            Routing.RegisterRoute("edit-departamento", typeof(EditDepartamentoPage));
        }
    }
}