
namespace APPFinalPugaLopezOrtiz.Services
{
    public class UsuarioService
    {
        private readonly ApiService _apiService;
        private static UsuarioResponse? _currentUser;

        public UsuarioService(ApiService apiService) => _apiService = apiService;
        public bool IsAuthenticated => _currentUser != null;
        public int? GetCurrentUserId() => _currentUser?.UsuarioId;

        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var response = await _apiService.Login(email, password);
                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo conectar con el servidor", "OK");
                    return false;
                }
                _currentUser = new UsuarioResponse { UsuarioId = response.UsuarioId, Nombre = response.Nombre, Email = response.Email };
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error en el login: {ex.Message}", "OK");
                return false;
            }
        }

        public async Task<bool> Register(string nombre, string email, string password)
        {
            try
            {
                var response = await _apiService.Register(nombre, email, password);
                if (response == null) return false;
                _currentUser = response;
                return true;
            }
            catch { return false; }
        }

        public void Logout() => _currentUser = null;
        public bool IsOwner(int resourceUserId) => _currentUser?.UsuarioId == resourceUserId;
    }
}