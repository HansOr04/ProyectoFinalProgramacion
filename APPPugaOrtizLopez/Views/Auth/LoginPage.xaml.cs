using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}