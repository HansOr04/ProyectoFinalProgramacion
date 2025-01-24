using APPFinalPugaLopezOrtiz.ViewModels;

namespace APPFinalPugaLopezOrtiz.Views;

public partial class DepartamentoDetailPage : ContentPage
{
    private readonly DepartamentoDetailViewModel _viewModel;

    public DepartamentoDetailPage(DepartamentoDetailViewModel viewModel)
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