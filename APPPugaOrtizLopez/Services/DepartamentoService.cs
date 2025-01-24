using System.Net.Http.Json;
using APPPugaOrtizLopez.Models;
using APPPugaOrtizLopez.Models.ModelsResponse;

namespace APPPugaOrtizLopez.Services
{
    public interface IDepartamentoService
    {
        Task<List<DepartamentoResponse>> GetAllDepartamentosAsync();
        Task<DepartamentoResponse> GetDepartamentoByIdAsync(int id);
        Task<DepartamentoResponse> CreateDepartamentoAsync(string titulo, string descripcion, string localizacion, string ciudad, int habitaciones, int baños, string lugaresCercanos, string imagenUrl, int usuarioId);
        Task<bool> UpdateDepartamentoAsync(int id, string titulo, string descripcion, string localizacion, string ciudad, int habitaciones, int baños, string lugaresCercanos, string imagenUrl, int usuarioId);
        Task<bool> DeleteDepartamentoAsync(int departamentoId, int usuarioId);
    }

    public class DepartamentoService : IDepartamentoService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7284/api/Departamento";

        public DepartamentoService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<DepartamentoResponse>> GetAllDepartamentosAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<DepartamentoResponse>>(BaseUrl);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting departamentos: {ex.Message}");
            }
        }

        public async Task<DepartamentoResponse> GetDepartamentoByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<DepartamentoResponse>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting departamento: {ex.Message}");
            }
        }

        public async Task<DepartamentoResponse> CreateDepartamentoAsync(string titulo, string descripcion, string localizacion,
            string ciudad, int habitaciones, int baños, string lugaresCercanos, string imagenUrl, int usuarioId)
        {
            try
            {
                var request = new
                {
                    Titulo = titulo,
                    Descripcion = descripcion,
                    Localizacion = localizacion,
                    Ciudad = ciudad,
                    Habitaciones = habitaciones,
                    Baños = baños,
                    LugaresCercanos = lugaresCercanos,
                    ImagenUrl = imagenUrl,
                    UsuarioId = usuarioId
                };

                var response = await _httpClient.PostAsJsonAsync(BaseUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<DepartamentoResponse>();
                }

                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating departamento: {ex.Message}");
            }
        }

        public async Task<bool> UpdateDepartamentoAsync(int id, string titulo, string descripcion, string localizacion,
            string ciudad, int habitaciones, int baños, string lugaresCercanos, string imagenUrl, int usuarioId)
        {
            try
            {
                var request = new
                {
                    Titulo = titulo,
                    Descripcion = descripcion,
                    Localizacion = localizacion,
                    Ciudad = ciudad,
                    Habitaciones = habitaciones,
                    Baños = baños,
                    LugaresCercanos = lugaresCercanos,
                    ImagenUrl = imagenUrl,
                    UsuarioId = usuarioId
                };

                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating departamento: {ex.Message}");
            }
        }

        public async Task<bool> DeleteDepartamentoAsync(int departamentoId, int usuarioId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{departamentoId}?usuarioId={usuarioId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting departamento: {ex.Message}");
            }
        }
    }
}