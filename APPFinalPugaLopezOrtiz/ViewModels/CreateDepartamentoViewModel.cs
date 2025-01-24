using APPFinalPugaLopezOrtiz.Services;
using System.Windows.Input;

namespace APPFinalPugaLopezOrtiz.ViewModels
{
    public class CreateDepartamentoViewModel : BaseViewModel
    {
        private string _titulo;
        private string _descripcion;
        private string _localizacion;
        private string _ciudad;
        private string _habitaciones;
        private string _baños;
        private string _lugaresCercanos;
        private string _imagenUrl;
        private readonly DepartamentoService _departamentoService;

        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        public string Localizacion
        {
            get => _localizacion;
            set => SetProperty(ref _localizacion, value);
        }

        public string Ciudad
        {
            get => _ciudad;
            set => SetProperty(ref _ciudad, value);
        }

        public string Habitaciones
        {
            get => _habitaciones;
            set => SetProperty(ref _habitaciones, value);
        }

        public string Baños
        {
            get => _baños;
            set => SetProperty(ref _baños, value);
        }

        public string LugaresCercanos
        {
            get => _lugaresCercanos;
            set => SetProperty(ref _lugaresCercanos, value);
        }

        public string ImagenUrl
        {
            get => _imagenUrl;
            set => SetProperty(ref _imagenUrl, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateDepartamentoViewModel(
            DepartamentoService departamentoService,
            UsuarioService usuarioService) : base(usuarioService)
        {
            _departamentoService = departamentoService;
            Title = "Crear Departamento";
            SaveCommand = new Command(async () => await ExecuteAsync(Save));
            CancelCommand = new Command(async () => await ExecuteAsync(Cancel));
        }

        private async Task Save()
        {
            if (!ValidateFields()) return;

            int.TryParse(Habitaciones, out int numHabitaciones);
            int.TryParse(Baños, out int numBaños);

            var departamento = await _departamentoService.CreateDepartamento(
                Titulo, Descripcion, Localizacion, Ciudad,
                numHabitaciones, numBaños, LugaresCercanos, ImagenUrl);

            if (departamento != null)
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(Titulo) ||
                string.IsNullOrWhiteSpace(Descripcion) ||
                string.IsNullOrWhiteSpace(Localizacion) ||
                string.IsNullOrWhiteSpace(Ciudad) ||
                string.IsNullOrWhiteSpace(Habitaciones) ||
                string.IsNullOrWhiteSpace(Baños))
            {
                Shell.Current.DisplayAlert("Error", "Complete los campos obligatorios", "OK");
                return false;
            }

            if (!int.TryParse(Habitaciones, out _) || !int.TryParse(Baños, out _))
            {
                Shell.Current.DisplayAlert("Error", "Ingrese números válidos", "OK");
                return false;
            }

            return true;
        }
    }
}