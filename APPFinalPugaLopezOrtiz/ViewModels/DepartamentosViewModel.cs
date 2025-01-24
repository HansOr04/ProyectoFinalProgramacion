using APPFinalPugaLopezOrtiz.Services;
using APPFinalPugaLopezOrtiz.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace APPFinalPugaLopezOrtiz.ViewModels
{
    public class DepartamentosViewModel : BaseViewModel
    {
        private ObservableCollection<DepartamentoResponse> departamentos;
        private readonly DepartamentoService _departamentoService;
        private readonly UsuarioService _usuarioService;

        public ObservableCollection<DepartamentoResponse> Departamentos
        {
            get => departamentos;
            set => SetProperty(ref departamentos, value);
        }

        public ICommand LoadDepartamentosCommand { get; }
        public ICommand NavigateToDetailCommand { get; }
        public ICommand NavigateToCreateCommand { get; }
        public ICommand LogoutCommand { get; }

        public DepartamentosViewModel(
    DepartamentoService departamentoService,
    UsuarioService usuarioService) : base(usuarioService)
        {
            _departamentoService = departamentoService;
            _usuarioService = usuarioService;
            Title = "Departamentos";
            Departamentos = new ObservableCollection<DepartamentoResponse>();
            LoadDepartamentosCommand = new Command(async () => await ExecuteAsync(LoadDepartamentos));
            NavigateToDetailCommand = new Command<DepartamentoResponse>(async (dept) => await ExecuteAsync(() => NavigateToDetail(dept)));
            NavigateToCreateCommand = new Command(async () => await ExecuteAsync(NavigateToCreate));
            LogoutCommand = new Command(async () => await ExecuteAsync(Logout));
        }

        private async Task LoadDepartamentos()
        {
            var depts = await _departamentoService.GetAllDepartamentos();
            Departamentos.Clear();
            foreach (var dept in depts)
            {
                Departamentos.Add(dept);
            }
        }

        private async Task NavigateToDetail(DepartamentoResponse departamento)
        {
            if (departamento == null) return;
            await Shell.Current.GoToAsync($"departamento-detail?id={departamento.DepartamentoId}");
        }

        private async Task NavigateToCreate()
        {
            if (!IsAuthenticated)
            {
                await Shell.Current.DisplayAlert("Error", "Debes iniciar sesión", "OK");
                return;
            }
            await Shell.Current.GoToAsync("create-departamento");
        }

        private async Task Logout()
        {
            var response = await Shell.Current.DisplayAlert(
                "Cerrar Sesión",
                "¿Confirmas cerrar sesión?",
                "Si", "No");

            if (response)
            {
                _usuarioService.Logout();
                await Shell.Current.GoToAsync("//login");
            }
        }
    }
}