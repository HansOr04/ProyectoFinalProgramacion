using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Guardados;

public partial class CallesGuardadasPage : ContentPage
{
    public CallesGuardadasPage(ListaCallesGuardadasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}