using APPPugaOrtizLopez.ViewModels;
namespace APPPugaOrtizLopez.Views.Flats;


public partial class CreateFlatPage : ContentPage
{
    private readonly CreateFlatViewModel _viewModel;

    public CreateFlatPage(CreateFlatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private async void OnCiudadSelected(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is string ciudad)
        {
            await _viewModel.CiudadChanged(ciudad);
        }
    }
}