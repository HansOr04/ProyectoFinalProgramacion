using CommunityToolkit.Mvvm.Input;
using APPPugaOrtizLopez.Services;
using APPPugaOrtizLopez.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace APPPugaOrtizLopez.ViewModels
{
    public partial class CreateFlatViewModel : ObservableObject
    {
        private readonly IDepartamentoService _departamentoService;

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        private string _ciudad;
        public string Ciudad
        {
            get => _ciudad;
            set => SetProperty(ref _ciudad, value);
        }

        private string _localizacion;
        public string Localizacion
        {
            get => _localizacion;
            set => SetProperty(ref _localizacion, value);
        }

        private int _habitaciones;
        public int Habitaciones
        {
            get => _habitaciones;
            set => SetProperty(ref _habitaciones, value);
        }

        private int _baños;
        public int Baños
        {
            get => _baños;
            set => SetProperty(ref _baños, value);
        }

        private string _lugaresCercanos;
        public string LugaresCercanos
        {
            get => _lugaresCercanos;
            set => SetProperty(ref _lugaresCercanos, value);
        }

        private string _imagenUrl;
        public string ImagenUrl
        {
            get => _imagenUrl;
            set => SetProperty(ref _imagenUrl, value);
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

        public CreateFlatViewModel() { } // For XAML preview

        public CreateFlatViewModel(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        [RelayCommand]
        private async Task SaveDepartamento()
        {
            if (!ValidateForm()) return;

            try
            {
                IsLoading = true;
                var userId = Preferences.Default.Get("UserId", "0");
                var response = await _departamentoService.CreateDepartamentoAsync(
                    _titulo,
                    _descripcion,
                    _localizacion,
                    _ciudad,
                    _habitaciones,
                    _baños,
                    _lugaresCercanos,
                    _imagenUrl,
                    int.Parse(userId)
                );

                if (response != null)
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(_titulo) ||
                string.IsNullOrWhiteSpace(_descripcion) ||
                string.IsNullOrWhiteSpace(_ciudad) ||
                string.IsNullOrWhiteSpace(_localizacion))
            {
                ErrorMessage = "Complete todos los campos obligatorios";
                return false;
            }

            if (_habitaciones <= 0 || _baños <= 0)
            {
                ErrorMessage = "El número de habitaciones y baños debe ser mayor a 0";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}