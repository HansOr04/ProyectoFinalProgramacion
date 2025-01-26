using APPPugaOrtizLopez.ViewModels;
namespace APPPugaOrtizLopez.Views.Flats;

public partial class AllFlatsPage : ContentPage
{
    private readonly AllFlatsViewModel _viewModel;

    public AllFlatsPage(AllFlatsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadDepartamentosCommand.Execute(null);
    }
}