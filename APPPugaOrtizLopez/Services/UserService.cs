using System.Net.Http.Json;
using APPPugaOrtizLopez.Models;
using APPPugaOrtizLopez.Models.ModelsResponse;
using System.Diagnostics;
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
        private const string BaseUrl = "https://localhost:7284/api/Usuario";

        public UserService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            try
            {
                var loginRequest = new LoginRequest { Email = email, Password = password };
                Debug.WriteLine($"Request: {System.Text.Json.JsonSerializer.Serialize(loginRequest)}");

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginRequest);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response Status: {response.StatusCode}");
                Debug.WriteLine($"Response Content: {content}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                throw new Exception(content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex}");
                throw;
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
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Register Response: {content}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserResponse>();
                }
                throw new Exception(content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Register Error: {ex}");
                throw;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            return await Task.FromResult(true);
        }
    }
}