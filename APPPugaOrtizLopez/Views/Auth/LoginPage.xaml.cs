using APPPugaOrtizLopez.ViewModels;
using System.Diagnostics;

namespace APPPugaOrtizLopez.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}