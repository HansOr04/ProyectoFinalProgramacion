using APPPugaOrtizLopez.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.Services
{

    public interface IApiPublicService
    {
        Task<List<Calles>> GetCallesAsync();
        List<string> GetCiudades();
        Task<List<string>> GetCallesByCiudadAsync(string ciudad);
    }

    public class ApiPublicService : IApiPublicService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://67966dc0bedc5d43a6c53bd7.mockapi.io/";
        private List<Calles> _callesCache;

        public ApiPublicService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<List<Calles>> GetCallesAsync()
        {
            try
            {
                if (_callesCache != null) return _callesCache;

                var response = await _httpClient.GetAsync("CallesEcuador");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _callesCache = JsonSerializer.Deserialize<List<Calles>>(content);
                return _callesCache;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Calles>();
            }
        }

        public List<string> GetCiudades()
        {
            if (_callesCache == null) return new List<string>();
            return _callesCache.Select(c => c.Ciudad).Distinct().ToList();
        }

        public async Task<List<string>> GetCallesByCiudadAsync(string ciudad)
        {
            var calles = await GetCallesAsync();
            return calles.Where(c => c.Ciudad == ciudad)
                        .Select(c => c.Calle)
                        .ToList();
        }
    }
}
