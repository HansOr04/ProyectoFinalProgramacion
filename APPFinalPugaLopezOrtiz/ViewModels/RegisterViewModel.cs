using APPFinalPugaLopezOrtiz.Services;
using System.Windows.Input;

namespace APPFinalPugaLopezOrtiz.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _nombre;
        private string _email;
        private string _password;
        private string _confirmPassword;

        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand GoToLoginCommand { get; }

        public RegisterViewModel(UsuarioService usuarioService) : base(usuarioService)
        {
            Title = "Registro";
            RegisterCommand = new Command(async () => await ExecuteAsync(Register));
            GoToLoginCommand = new Command(async () => await ExecuteAsync(GoToLogin));
        }

        private async Task Register()
        {
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Error", "Complete todos los campos", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
                return;
            }

            var success = await UserService.Register(Nombre, Email, Password);
            if (success)
            {
                await Shell.Current.DisplayAlert("Éxito", "Registro exitoso", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo registrar", "OK");
            }
        }

        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}