using System.Net.Http.Json;
using APPPugaOrtizLopez.Models;
using APPPugaOrtizLopez.Models.ModelsResponse;

namespace APPPugaOrtizLopez.Services
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
        Task<UserResponse> RegisterAsync(string name, string email, string password);
        Task<bool> LogoutAsync();
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000/api/Usuario";

        public UserService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            try
            {
                var loginRequest = new LoginRequest { Email = email, Password = password };
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }

                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        public async Task<UserResponse> RegisterAsync(string name, string email, string password)
        {
            try
            {
                var registerRequest = new RegisterRequest
                {
                    Nombre = name,
                    Email = email,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", registerRequest);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserResponse>();
                }

                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Registration failed: {ex.Message}");
            }
        }

        public async Task<bool> LogoutAsync()
        {
            // Clear local auth state
            return await Task.FromResult(true);
        }
    }
}