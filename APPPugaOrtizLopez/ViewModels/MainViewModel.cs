using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace APPPugaOrtizLopez.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private bool _isButtonEnabled = true;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => SetProperty(ref _isButtonEnabled, value);
        }

        [RelayCommand]
        private async Task StartApp()
        {
            try
            {
                IsButtonEnabled = false;
                await Shell.Current.GoToAsync("//Login");
            }
            finally
            {
                IsButtonEnabled = true;
            }
        }
    }
}