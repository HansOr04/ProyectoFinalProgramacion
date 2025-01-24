using APPFinalPugaLopezOrtiz.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APPFinalPugaLopezOrtiz.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusy;
        private string title;
        protected readonly UsuarioService UserService;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool IsAuthenticated => UserService.IsAuthenticated;

        public BaseViewModel(UsuarioService userService)
        {
            UserService = userService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected async Task ExecuteAsync(Func<Task> action)
        {
            try
            {
                IsBusy = true;
                await action();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
