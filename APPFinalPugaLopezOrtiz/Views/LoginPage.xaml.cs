using APPFinalPugaLopezOrtiz.ViewModels;

namespace APPFinalPugaLopezOrtiz.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}