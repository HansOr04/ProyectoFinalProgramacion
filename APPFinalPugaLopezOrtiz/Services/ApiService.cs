using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace APPFinalPugaLopezOrtiz.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7284/api/";
        private string? _sessionCookie;

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
                UseProxy = false
            };
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        

        private void AddSessionCookie()
        {
            if (!string.IsNullOrEmpty(_sessionCookie))
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", _sessionCookie);
            }
        }

        public async Task<LoginResponse?> Login(string email, string password)
        {
            try
            {
                var request = new LoginRequest { Email = email, Password = password };
                var jsonContent = JsonContent.Create(request);
                var response = await _httpClient.PostAsync($"{BaseUrl}Usuario/login", jsonContent);

                Debug.WriteLine($"Status: {response.StatusCode}");
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Content: {content}");

                return response.IsSuccessStatusCode ?
                    JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex}");
                return null;
            }
        }

        public async Task<UsuarioResponse?> Register(string nombre, string email, string password)
        {
            try
            {
                var request = new RegisterRequest
                {
                    Nombre = nombre,
                    Email = email,
                    Password = password
                };
                var jsonContent = JsonContent.Create(request);
                var response = await _httpClient.PostAsync($"{BaseUrl}Usuario/register", jsonContent);
                var content = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode ?
                    JsonSerializer.Deserialize<UsuarioResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex}");
                return null;
            }
        }


        public async Task<List<DepartamentoResponse>> GetAllDepartamentos()
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.GetAsync($"{BaseUrl}Departamento");
                return response.IsSuccessStatusCode ?
                    await response.Content.ReadFromJsonAsync<List<DepartamentoResponse>>() ??
                    new List<DepartamentoResponse>() : new List<DepartamentoResponse>();
            }
            catch { return new List<DepartamentoResponse>(); }
        }

        public async Task<DepartamentoResponse?> GetDepartamentoById(int id)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.GetAsync($"{BaseUrl}Departamento/{id}");
                return response.IsSuccessStatusCode ?
                    await response.Content.ReadFromJsonAsync<DepartamentoResponse>() : null;
            }
            catch { return null; }
        }

        public async Task<DepartamentoResponse?> CreateDepartamento(CreateDepartamentoRequest departamento)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}Departamento", departamento);
                return response.IsSuccessStatusCode ?
                    await response.Content.ReadFromJsonAsync<DepartamentoResponse>() : null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateDepartamento(int id, UpdateDepartamentoRequest departamento)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}Departamento/{id}", departamento);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteDepartamento(int departamentoId, int usuarioId)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.DeleteAsync($"{BaseUrl}Departamento/{departamentoId}?usuarioId={usuarioId}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<ComentarioResponse>> GetAllComentarios()
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.GetAsync($"{BaseUrl}Comentario");
                return response.IsSuccessStatusCode ?
                    await response.Content.ReadFromJsonAsync<List<ComentarioResponse>>() ??
                    new List<ComentarioResponse>() : new List<ComentarioResponse>();
            }
            catch { return new List<ComentarioResponse>(); }
        }

        public async Task<ComentarioResponse?> GetComentarioById(int id)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.GetAsync($"{BaseUrl}Comentario/{id}");
                return response.IsSuccessStatusCode ?
                    await response.Content.ReadFromJsonAsync<ComentarioResponse>() : null;
            }
            catch { return null; }
        }

        public async Task<ComentarioResponse?> CreateComentario(CreateComentarioRequest comentario)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}Comentario", comentario);
                return response.IsSuccessStatusCode ?
                    await response.Content.ReadFromJsonAsync<ComentarioResponse>() : null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateComentario(int id, UpdateComentarioRequest comentario)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}Comentario/{id}", comentario);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteComentario(int comentarioId, int usuarioId)
        {
            try
            {
                AddSessionCookie();
                var response = await _httpClient.DeleteAsync($"{BaseUrl}Comentario/{comentarioId}?usuarioId={usuarioId}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponse
    {
        public required int UsuarioId { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
    }

    public class RegisterRequest
    {
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UsuarioResponse
    {
        public required int UsuarioId { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
    }

    public class DepartamentoResponse
    {
        public required int DepartamentoId { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public required string Localizacion { get; set; }
        public required string Ciudad { get; set; }
        public required int Habitaciones { get; set; }
        public required int Baños { get; set; }
        public string? LugaresCercanos { get; set; }
        public string? ImagenUrl { get; set; }
        public required UsuarioResponse Usuario { get; set; }
        public required List<ComentarioResponse> Comentarios { get; set; }
    }

    public class CreateDepartamentoRequest
    {
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public required string Localizacion { get; set; }
        public required string Ciudad { get; set; }
        public required int Habitaciones { get; set; }
        public required int Baños { get; set; }
        public string? LugaresCercanos { get; set; }
        public string? ImagenUrl { get; set; }
        public required int UsuarioId { get; set; }
    }

    public class UpdateDepartamentoRequest
    {
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public required string Localizacion { get; set; }
        public required string Ciudad { get; set; }
        public required int Habitaciones { get; set; }
        public required int Baños { get; set; }
        public string? LugaresCercanos { get; set; }
        public string? ImagenUrl { get; set; }
        public required int UsuarioId { get; set; }
    }

    public class ComentarioResponse
    {
        public required int ComentarioId { get; set; }
        public required string Contenido { get; set; }
        public required DateTime FechaCreacion { get; set; }
        public required DepartamentoInfo Departamento { get; set; }
        public required UsuarioResponse Usuario { get; set; }
    }

    public class DepartamentoInfo
    {
        public required int DepartamentoId { get; set; }
        public required string Titulo { get; set; }
    }

    public class CreateComentarioRequest
    {
        public required string Contenido { get; set; }
        public required int UsuarioId { get; set; }
        public required int DepartamentoId { get; set; }
    }

    public class UpdateComentarioRequest
    {
        public required string Contenido { get; set; }
        public required int UsuarioId { get; set; }
    }
}