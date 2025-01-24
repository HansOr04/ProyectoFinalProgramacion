using APPFinalPugaLopezOrtiz.Services;
using APPFinalPugaLopezOrtiz.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace APPFinalPugaLopezOrtiz.ViewModels
{
    public class DepartamentoDetailViewModel : BaseViewModel
    {
        private DepartamentoResponse _departamento;
        private ObservableCollection<ComentarioResponse> _comentarios;
        private string _newComentario;
        private readonly int _departamentoId;
        private readonly DepartamentoService _departamentoService;
        private readonly ComentarioService _comentarioService;

        public DepartamentoResponse Departamento
        {
            get => _departamento;
            set => SetProperty(ref _departamento, value);
        }

        public ObservableCollection<ComentarioResponse> Comentarios
        {
            get => _comentarios;
            set => SetProperty(ref _comentarios, value);
        }

        public string NewComentario
        {
            get => _newComentario;
            set => SetProperty(ref _newComentario, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand AddComentarioCommand { get; }
        public ICommand EditComentarioCommand { get; }
        public ICommand DeleteComentarioCommand { get; }
        public ICommand EditDepartamentoCommand { get; }
        public ICommand DeleteDepartamentoCommand { get; }

        public DepartamentoDetailViewModel(
            int departamentoId,
            DepartamentoService departamentoService,
            ComentarioService comentarioService,
            UsuarioService usuarioService) : base(usuarioService)
        {
            _departamentoId = departamentoId;
            _departamentoService = departamentoService;
            _comentarioService = comentarioService;

            Comentarios = new ObservableCollection<ComentarioResponse>();

            LoadCommand = new Command(async () => await ExecuteAsync(LoadDepartamento));
            AddComentarioCommand = new Command(async () => await ExecuteAsync(AddComentario));
            EditComentarioCommand = new Command<ComentarioResponse>(async (c) => await ExecuteAsync(() => EditComentario(c)));
            DeleteComentarioCommand = new Command<ComentarioResponse>(async (c) => await ExecuteAsync(() => DeleteComentario(c)));
            EditDepartamentoCommand = new Command(async () => await ExecuteAsync(EditDepartamento));
            DeleteDepartamentoCommand = new Command(async () => await ExecuteAsync(DeleteDepartamento));
        }

        private async Task LoadDepartamento()
        {
            Departamento = await _departamentoService.GetDepartamentoById(_departamentoId);
            if (Departamento != null)
            {
                Title = Departamento.Titulo;
                Comentarios.Clear();
                foreach (var comentario in Departamento.Comentarios)
                {
                    Comentarios.Add(comentario);
                }
            }
        }

        private async Task AddComentario()
        {
            if (string.IsNullOrWhiteSpace(NewComentario)) return;

            if (!IsAuthenticated)
            {
                await Shell.Current.DisplayAlert("Error", "Debe iniciar sesión", "OK");
                return;
            }

            var comentario = await _comentarioService.CreateComentario(NewComentario, _departamentoId);
            if (comentario != null)
            {
                Comentarios.Add(comentario);
                NewComentario = string.Empty;
            }
        }

        private async Task EditComentario(ComentarioResponse comentario)
        {
            if (comentario == null) return;

            var content = await Shell.Current.DisplayPromptAsync(
                "Editar Comentario",
                "Edite su comentario:",
                initialValue: comentario.Contenido);

            if (string.IsNullOrWhiteSpace(content)) return;

            var success = await _comentarioService.UpdateComentario(comentario.ComentarioId, content);
            if (success)
            {
                await LoadDepartamento();
            }
        }

        private async Task DeleteComentario(ComentarioResponse comentario)
        {
            if (comentario == null) return;

            var confirm = await Shell.Current.DisplayAlert(
                "Confirmar",
                "¿Desea eliminar este comentario?",
                "Sí", "No");

            if (!confirm) return;

            var success = await _comentarioService.DeleteComentario(comentario.ComentarioId);
            if (success)
            {
                Comentarios.Remove(comentario);
            }
        }

        private async Task EditDepartamento()
        {
            await Shell.Current.GoToAsync($"edit-departamento?id={_departamentoId}");
        }

        private async Task DeleteDepartamento()
        {
            var confirm = await Shell.Current.DisplayAlert(
                "Confirmar",
                "¿Desea eliminar este departamento?",
                "Sí", "No");

            if (!confirm) return;

            var success = await _departamentoService.DeleteDepartamento(_departamentoId);
            if (success)
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        public bool CanEditDepartamento =>
            Departamento?.Usuario?.UsuarioId == UserService.GetCurrentUserId();

        public bool CanModifyComentario(ComentarioResponse comentario) =>
            IsAuthenticated &&
            (comentario?.Usuario?.UsuarioId == UserService.GetCurrentUserId() ||
             Departamento?.Usuario?.UsuarioId == UserService.GetCurrentUserId());
    }
}