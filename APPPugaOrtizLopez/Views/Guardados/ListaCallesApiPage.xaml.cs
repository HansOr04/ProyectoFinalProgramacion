using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Guardados;

public partial class ListaCallesApiPage : ContentPage
{
    public ListaCallesApiPage(ListaCallesApiViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}