using CommunityToolkit.Mvvm.Input;
using APPPugaOrtizLopez.Services;
using APPPugaOrtizLopez.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using APPPugaOrtizLopez.Models.ModelsResponse;

namespace APPPugaOrtizLopez.ViewModels
{
    [QueryProperty(nameof(Departamento), "Departamento")]
    public partial class FlatDetailsViewModel : ObservableObject
    {
        private readonly IDepartamentoService _departamentoService;
        private readonly IComentarioService _comentarioService;

        private DepartamentoResponse _departamento;
        public DepartamentoResponse Departamento
        {
            get => _departamento;
            set => SetProperty(ref _departamento, value);
        }

        private string _comentario;
        public string Comentario
        {
            get => _comentario;
            set => SetProperty(ref _comentario, value);
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
        public FlatDetailsViewModel() { }
        public FlatDetailsViewModel(IDepartamentoService departamentoService, IComentarioService comentarioService) {
            _departamentoService = departamentoService;
            _comentarioService = comentarioService;
        }
    
        [RelayCommand]
        private async Task AddComentario()
        {
            if (string.IsNullOrWhiteSpace(_comentario)) return;

            try
            {
                IsLoading = true;
                var userId = Preferences.Default.Get("UserId", "0");

                var response = await _comentarioService.CreateComentarioAsync(
                    _comentario,
                    int.Parse(userId),
                    Departamento.DepartamentoId
                );

                if (response != null)
                {
                    await LoadDepartamento();
                    Comentario = string.Empty;
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

        private async Task LoadDepartamento()
        {
            try
            {
                var departamento = await _departamentoService.GetDepartamentoByIdAsync(Departamento.DepartamentoId);
                if (departamento != null)
                {
                    Departamento = departamento;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }
}