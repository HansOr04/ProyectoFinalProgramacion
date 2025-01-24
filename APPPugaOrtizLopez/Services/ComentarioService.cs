using System.Net.Http.Json;
using APPPugaOrtizLopez.Models;
using APPPugaOrtizLopez.Models.ModelsResponse;

namespace APPPugaOrtizLopez.Services
{
    public interface IComentarioService
    {
        Task<List<ComentarioResponse>> GetAllComentariosAsync();
        Task<ComentarioResponse> GetComentarioByIdAsync(int id);
        Task<ComentarioResponse> CreateComentarioAsync(string contenido, int usuarioId, int departamentoId);
        Task<bool> UpdateComentarioAsync(int id, string contenido, int usuarioId);
        Task<bool> DeleteComentarioAsync(int comentarioId, int usuarioId);
    }

    public class ComentarioService : IComentarioService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000/api/Comentario";

        public ComentarioService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ComentarioResponse>> GetAllComentariosAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ComentarioResponse>>(BaseUrl);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting comentarios: {ex.Message}");
            }
        }

        public async Task<ComentarioResponse> GetComentarioByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ComentarioResponse>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting comentario: {ex.Message}");
            }
        }

        public async Task<ComentarioResponse> CreateComentarioAsync(string contenido, int usuarioId, int departamentoId)
        {
            try
            {
                var request = new { Contenido = contenido, UsuarioId = usuarioId, DepartamentoId = departamentoId };
                var response = await _httpClient.PostAsJsonAsync(BaseUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ComentarioResponse>();
                }

                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating comentario: {ex.Message}");
            }
        }

        public async Task<bool> UpdateComentarioAsync(int id, string contenido, int usuarioId)
        {
            try
            {
                var request = new { Contenido = contenido, UsuarioId = usuarioId };
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating comentario: {ex.Message}");
            }
        }

        public async Task<bool> DeleteComentarioAsync(int comentarioId, int usuarioId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{comentarioId}?usuarioId={usuarioId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting comentario: {ex.Message}");
            }
        }
    }
}