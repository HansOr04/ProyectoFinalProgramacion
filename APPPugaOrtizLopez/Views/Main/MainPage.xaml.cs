using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Main;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}