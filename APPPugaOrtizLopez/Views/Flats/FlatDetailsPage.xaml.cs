using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Flats;

public partial class FlatDetailsPage : ContentPage
{
    public FlatDetailsPage(FlatDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}