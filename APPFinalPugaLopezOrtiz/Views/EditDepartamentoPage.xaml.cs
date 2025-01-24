using APPFinalPugaLopezOrtiz.ViewModels;

namespace APPFinalPugaLopezOrtiz.Views;

public partial class EditDepartamentoPage : ContentPage
{
    private readonly EditDepartamentoViewModel _viewModel;

    public EditDepartamentoPage(EditDepartamentoViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadCommand.Execute(null);
    }
}