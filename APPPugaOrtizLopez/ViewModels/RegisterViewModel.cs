using CommunityToolkit.Mvvm.Input;
using APPPugaOrtizLopez.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace APPPugaOrtizLopez.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
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

        public RegisterViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (!ValidateForm()) return;

            try
            {
                IsLoading = true;
                var response = await _userService.RegisterAsync(_nombre, _email, _password);

                if (response != null)
                {
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

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(_nombre) ||
                string.IsNullOrWhiteSpace(_email) ||
                string.IsNullOrWhiteSpace(_password) ||
                string.IsNullOrWhiteSpace(_confirmPassword))
            {
                ErrorMessage = "Complete todos los campos obligatorios";
                return false;
            }

            if (_password != _confirmPassword)
            {
                ErrorMessage = "Las contraseñas no coinciden";
                return false;
            }

            if (!IsValidEmail(_email))
            {
                ErrorMessage = "Email inválido";
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}