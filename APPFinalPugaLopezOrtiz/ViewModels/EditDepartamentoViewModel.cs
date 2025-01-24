using APPFinalPugaLopezOrtiz.Services;
using System.Windows.Input;

namespace APPFinalPugaLopezOrtiz.ViewModels
{
    public class EditDepartamentoViewModel : BaseViewModel
    {
        private string _titulo;
        private string _descripcion;
        private string _localizacion;
        private string _ciudad;
        private string _habitaciones;
        private string _baños;
        private string _lugaresCercanos;
        private string _imagenUrl;
        private readonly int _departamentoId;
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

        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditDepartamentoViewModel(
            int departamentoId,
            DepartamentoService departamentoService,
            UsuarioService usuarioService) : base(usuarioService)
        {
            _departamentoId = departamentoId;
            _departamentoService = departamentoService;
            Title = "Editar Departamento";

            LoadCommand = new Command(async () => await ExecuteAsync(LoadDepartamento));
            SaveCommand = new Command(async () => await ExecuteAsync(Save));
            CancelCommand = new Command(async () => await ExecuteAsync(Cancel));
        }

        private async Task LoadDepartamento()
        {
            var departamento = await _departamentoService.GetDepartamentoById(_departamentoId);
            if (departamento != null)
            {
                Titulo = departamento.Titulo;
                Descripcion = departamento.Descripcion;
                Localizacion = departamento.Localizacion;
                Ciudad = departamento.Ciudad;
                Habitaciones = departamento.Habitaciones.ToString();
                Baños = departamento.Baños.ToString();
                LugaresCercanos = departamento.LugaresCercanos;
                ImagenUrl = departamento.ImagenUrl;
            }
        }

        private async Task Save()
        {
            if (!ValidateFields()) return;

            int.TryParse(Habitaciones, out int numHabitaciones);
            int.TryParse(Baños, out int numBaños);

            var success = await _departamentoService.UpdateDepartamento(
                _departamentoId, Titulo, Descripcion, Localizacion, Ciudad,
                numHabitaciones, numBaños, LugaresCercanos, ImagenUrl);

            if (success)
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