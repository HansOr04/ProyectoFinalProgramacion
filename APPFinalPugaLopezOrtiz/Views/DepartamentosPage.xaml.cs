using APPFinalPugaLopezOrtiz.ViewModels;

namespace APPFinalPugaLopezOrtiz.Views;

public partial class DepartamentosPage : ContentPage
{
    private readonly DepartamentosViewModel _viewModel;

    public DepartamentosPage(DepartamentosViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadDepartamentosCommand.Execute(null);
    }
}