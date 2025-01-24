using APPPugaOrtizLopez.ViewModels;

namespace APPPugaOrtizLopez.Views.Flats;

public partial class AllFlatsPage : ContentPage
{
    public AllFlatsPage(AllFlatsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AllFlatsViewModel viewModel)
        {
            viewModel.LoadDepartamentosCommand.Execute(null);
        }
    }
}