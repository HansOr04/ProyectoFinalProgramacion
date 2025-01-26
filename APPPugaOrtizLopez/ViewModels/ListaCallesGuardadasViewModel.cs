using APPPugaOrtizLopez.Models;
using APPPugaOrtizLopez.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.ViewModels
{
    public partial class ListaCallesGuardadasViewModel : ObservableObject
    {
        private readonly ICallesSqliteService _callesSqliteService;

        private ObservableCollection<CalleSqlite> _calles;
        public ObservableCollection<CalleSqlite> Calles
        {
            get => _calles;
            set => SetProperty(ref _calles, value);
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

        public ListaCallesGuardadasViewModel(ICallesSqliteService callesSqliteService)
        {
            _callesSqliteService = callesSqliteService;
            Calles = new ObservableCollection<CalleSqlite>();
            LoadCalles();
        }

        [RelayCommand]
        private async Task LoadCalles()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                var callesDb = await _callesSqliteService.GetCallesAsync();
                Calles.Clear();
                foreach (var calle in callesDb)
                {
                    Calles.Add(calle);
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
        private async Task DeleteCalle(CalleSqlite calle)
        {
            if (calle == null) return;

            try
            {
                IsLoading = true;
                bool success = await _callesSqliteService.DeleteCalleAsync(calle.Id);
                if (success)
                {
                    Calles.Remove(calle);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al eliminar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToListaCallesApi()
        {
            try
            {
                await Shell.Current.GoToAsync("//ListaCallesApi");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }
    }
}
