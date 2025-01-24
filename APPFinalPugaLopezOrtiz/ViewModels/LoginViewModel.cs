using APPFinalPugaLopezOrtiz.Services;
using APPFinalPugaLopezOrtiz.Views;
using System.Windows.Input;

namespace APPFinalPugaLopezOrtiz.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public LoginViewModel(UsuarioService usuarioService) : base(usuarioService)
        {
            Title = "Login";
            LoginCommand = new Command(async () => await ExecuteAsync(Login));
            GoToRegisterCommand = new Command(async () => await ExecuteAsync(GoToRegister));
        }

        private async Task Login()
        {
            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Complete todos los campos", "OK");
                    return;
                }
                var success = await UserService.Login(Email, Password);
                if (success)
                {
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error detallado", $"Error al iniciar sesión con email: {Email}\nVerifique las credenciales y la conexión al servidor.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error de conexión: {ex.Message}", "OK");
            }
        }

        private async Task GoToRegister()
        {
            await Application.Current.MainPage.Navigation.PushAsync(
                new RegisterPage(new RegisterViewModel(UserService)));
        }
    }
}