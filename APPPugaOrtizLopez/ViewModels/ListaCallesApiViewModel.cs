using APPPugaOrtizLopez.Models;
using APPPugaOrtizLopez.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.ViewModels
{
    public partial class ListaCallesApiViewModel : ObservableObject
    {
        private readonly IApiPublicService _apiPublicService;
        private readonly ICallesSqliteService _callesSqliteService;

        private ObservableCollection<Calles> _calles;
        public ObservableCollection<Calles> Calles
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


        public ListaCallesApiViewModel(IApiPublicService apiPublicService, ICallesSqliteService callesSqliteService)
        {
            _apiPublicService = apiPublicService;
            _callesSqliteService = callesSqliteService;
            Calles = new ObservableCollection<Calles>();
            LoadCalles();
        }

        [RelayCommand]
        private async Task LoadCalles()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                var callesApi = await _apiPublicService.GetCallesAsync();
                Calles.Clear();
                foreach (var calle in callesApi)
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
        private async Task GuardarCalle(Calles calle)
        {
            if (calle == null) return;

            try
            {
                IsLoading = true;
                var calleSqlite = new CalleSqlite
                {
                    Ciudad = calle.Ciudad,
                    Calle = calle.Calle,
                    FechaCreacion = DateTime.Now
                };

                bool success = await _callesSqliteService.AddCalleAsync(calleSqlite);
                if (success)
                {
                    ErrorMessage = "Calle guardada exitosamente";
                    await Task.Delay(2000);
                    ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al guardar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
