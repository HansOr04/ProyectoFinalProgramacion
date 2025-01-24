using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Flats;

public partial class CreateFlatPage : ContentPage
{
    public CreateFlatPage(CreateFlatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}