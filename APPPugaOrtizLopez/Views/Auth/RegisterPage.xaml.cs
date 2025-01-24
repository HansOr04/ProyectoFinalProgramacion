using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Auth;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}