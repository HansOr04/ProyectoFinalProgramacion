using CommunityToolkit.Mvvm.Input;
using APPPugaOrtizLopez.Services;
using APPPugaOrtizLopez.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace APPPugaOrtizLopez.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_password))
            {
                ErrorMessage = "Por favor complete todos los campos";
                return;
            }

            try
            {
                IsLoading = true;
                var response = await _userService.LoginAsync(_email, _password);

                if (response != null)
                {
                    Preferences.Default.Set("UserId", response.UsuarioId.ToString());
                    Preferences.Default.Set("UserName", response.Nombre);
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync("Register");
        }
    }
}