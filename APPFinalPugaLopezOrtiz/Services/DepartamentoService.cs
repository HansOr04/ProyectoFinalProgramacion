    namespace APPFinalPugaLopezOrtiz.Services
    {
        public class DepartamentoService
        {
            private readonly ApiService _apiService;
            private readonly UsuarioService _usuarioService;

            public DepartamentoService(ApiService apiService, UsuarioService usuarioService)
            {
                _apiService = apiService;
                _usuarioService = usuarioService;
            }

            public Task<List<DepartamentoResponse>> GetAllDepartamentos()
            {
                try { return _apiService.GetAllDepartamentos(); }
                catch { return Task.FromResult(new List<DepartamentoResponse>()); }
            }

            public Task<DepartamentoResponse?> GetDepartamentoById(int id)
            {
                try { return _apiService.GetDepartamentoById(id); }
                catch { return Task.FromResult<DepartamentoResponse?>(null); }
            }

            public async Task<DepartamentoResponse?> CreateDepartamento(string titulo, string descripcion,
                string localizacion, string ciudad, int habitaciones, int baños,
                string? lugaresCercanos, string? imagenUrl)
            {
                try
                {
                    var userId = _usuarioService.GetCurrentUserId();
                    if (!userId.HasValue) return null;

                    var request = new CreateDepartamentoRequest
                    {
                        Titulo = titulo,
                        Descripcion = descripcion,
                        Localizacion = localizacion,
                        Ciudad = ciudad,
                        Habitaciones = habitaciones,
                        Baños = baños,
                        LugaresCercanos = lugaresCercanos,
                        ImagenUrl = imagenUrl,
                        UsuarioId = userId.Value
                    };

                    return await _apiService.CreateDepartamento(request);
                }
                catch { return null; }
            }

            public async Task<bool> UpdateDepartamento(int departamentoId, string titulo, string descripcion,
                string localizacion, string ciudad, int habitaciones, int baños,
                string? lugaresCercanos, string? imagenUrl)
            {
                try
                {
                    var userId = _usuarioService.GetCurrentUserId();
                    if (!userId.HasValue) return false;

                    var request = new UpdateDepartamentoRequest
                    {
                        Titulo = titulo,
                        Descripcion = descripcion,
                        Localizacion = localizacion,
                        Ciudad = ciudad,
                        Habitaciones = habitaciones,
                        Baños = baños,
                        LugaresCercanos = lugaresCercanos,
                        ImagenUrl = imagenUrl,
                        UsuarioId = userId.Value
                    };

                    return await _apiService.UpdateDepartamento(departamentoId, request);
                }
                catch { return false; }
            }

            public async Task<bool> DeleteDepartamento(int departamentoId)
            {
                try
                {
                    var userId = _usuarioService.GetCurrentUserId();
                    if (!userId.HasValue) return false;

                    return await _apiService.DeleteDepartamento(departamentoId, userId.Value);
                }
                catch { return false; }
            }

            public bool CanEditDepartamento(DepartamentoResponse? departamento) =>
                departamento?.Usuario != null && _usuarioService.IsOwner(departamento.Usuario.UsuarioId);

            public bool CanDeleteDepartamento(DepartamentoResponse? departamento) =>
                CanEditDepartamento(departamento);
        }
    }
