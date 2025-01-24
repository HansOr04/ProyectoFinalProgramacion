using APPFinalPugaLopezOrtiz.ViewModels;

namespace APPFinalPugaLopezOrtiz.Views;

public partial class CreateDepartamentoPage : ContentPage
{
    public CreateDepartamentoPage(CreateDepartamentoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}