namespace APPFinalPugaLopezOrtiz.Services
{
    public class ComentarioService
    {
        private readonly ApiService _apiService;
        private readonly UsuarioService _usuarioService;

        public ComentarioService(ApiService apiService, UsuarioService usuarioService)
        {
            _apiService = apiService;
            _usuarioService = usuarioService;
        }

        public Task<List<ComentarioResponse>> GetAllComentarios()
        {
            try { return _apiService.GetAllComentarios(); }
            catch { return Task.FromResult(new List<ComentarioResponse>()); }
        }

        public Task<ComentarioResponse?> GetComentarioById(int id)
        {
            try { return _apiService.GetComentarioById(id); }
            catch { return Task.FromResult<ComentarioResponse?>(null); }
        }

        public async Task<ComentarioResponse?> CreateComentario(string contenido, int departamentoId)
        {
            try
            {
                var userId = _usuarioService.GetCurrentUserId();
                if (!userId.HasValue) return null;

                return await _apiService.CreateComentario(new CreateComentarioRequest
                {
                    Contenido = contenido,
                    UsuarioId = userId.Value,
                    DepartamentoId = departamentoId
                });
            }
            catch { return null; }
        }

        public async Task<bool> UpdateComentario(int comentarioId, string contenido)
        {
            try
            {
                var userId = _usuarioService.GetCurrentUserId();
                if (!userId.HasValue) return false;

                await _apiService.UpdateComentario(comentarioId, new UpdateComentarioRequest
                {
                    Contenido = contenido,
                    UsuarioId = userId.Value
                });
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteComentario(int comentarioId)
        {
            try
            {
                var userId = _usuarioService.GetCurrentUserId();
                if (!userId.HasValue) return false;

                await _apiService.DeleteComentario(comentarioId, userId.Value);
                return true;
            }
            catch { return false; }
        }

        public bool CanEditComentario(ComentarioResponse comentario) =>
            comentario != null && _usuarioService.IsOwner(comentario.Usuario.UsuarioId);

        public bool CanDeleteComentario(ComentarioResponse comentario, DepartamentoResponse departamento)
        {
            if (comentario == null || departamento == null) return false;
            var userId = _usuarioService.GetCurrentUserId();
            return userId.HasValue && (userId.Value == comentario.Usuario.UsuarioId || userId.Value == departamento.Usuario.UsuarioId);
        }
    }
}