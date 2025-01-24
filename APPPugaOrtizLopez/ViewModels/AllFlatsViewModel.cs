using CommunityToolkit.Mvvm.Input;
using APPPugaOrtizLopez.Services;
using APPPugaOrtizLopez.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using APPPugaOrtizLopez.Models.ModelsResponse;

namespace APPPugaOrtizLopez.ViewModels
{
    public partial class AllFlatsViewModel : ObservableObject
    {
        private readonly IDepartamentoService _departamentoService;

        private ObservableCollection<DepartamentoResponse> _departamentos;
        public ObservableCollection<DepartamentoResponse> Departamentos
        {
            get => _departamentos;
            set => SetProperty(ref _departamentos, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public AllFlatsViewModel(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
            _departamentos = new ObservableCollection<DepartamentoResponse>();
            LoadDepartamentos();
        }

        [RelayCommand]
        private async Task LoadDepartamentos()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                var departamentos = await _departamentoService.GetAllDepartamentosAsync();
                Departamentos.Clear();
                foreach (var departamento in departamentos)
                {
                    Departamentos.Add(departamento);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading departamentos: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToCreateDepartamento()
        {
            await Shell.Current.GoToAsync("CreateFlat");
        }

        [RelayCommand]
        private async Task NavigateToDetails(DepartamentoResponse departamento)
        {
            if (departamento == null) return;
            var parameters = new Dictionary<string, object>
           {
               { "Departamento", departamento }
           };
            await Shell.Current.GoToAsync("FlatDetails", parameters);
        }
    }
}