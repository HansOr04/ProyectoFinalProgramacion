using CommunityToolkit.Mvvm.Input;
using APPPugaOrtizLopez.Services;
using APPPugaOrtizLopez.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace APPPugaOrtizLopez.ViewModels
{
    public partial class CreateFlatViewModel : ObservableObject
    {
        private readonly IDepartamentoService _departamentoService;
        private readonly Cloudinary _cloudinary;

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        private string _ciudad;
        public string Ciudad
        {
            get => _ciudad;
            set => SetProperty(ref _ciudad, value);
        }

        private string _localizacion;
        public string Localizacion
        {
            get => _localizacion;
            set => SetProperty(ref _localizacion, value);
        }

        private int _habitaciones;
        public int Habitaciones
        {
            get => _habitaciones;
            set => SetProperty(ref _habitaciones, value);
        }

        private int _baños;
        public int Baños
        {
            get => _baños;
            set => SetProperty(ref _baños, value);
        }

        private string _lugaresCercanos;
        public string LugaresCercanos
        {
            get => _lugaresCercanos;
            set => SetProperty(ref _lugaresCercanos, value);
        }

        private string _imagenUrl;
        public string ImagenUrl
        {
            get => _imagenUrl;
            set => SetProperty(ref _imagenUrl, value);
        }

        private FileResult _selectedFile;
        public FileResult SelectedFile
        {
            get => _selectedFile;
            set => SetProperty(ref _selectedFile, value);
        }

        private ImageSource _selectedImage;
        public ImageSource SelectedImage
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value);
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

        public CreateFlatViewModel() { }

        public CreateFlatViewModel(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;

            Account account = new Account(
                "dzerzykxk",  
                "425821271237374",
                "g34Np2Ey0zNXJHkmciHirA6Ei3Q"
            );
            _cloudinary = new Cloudinary(account);
        }

        private async Task<string> UploadToCloudinary(FileResult file)
        {
            using var stream = await file.OpenReadAsync();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(1024).Height(1024).Crop("limit")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        [RelayCommand]
        private async Task SaveDepartamento()
        {
            if (!ValidateForm()) return;

            try
            {
                IsLoading = true;
                var userId = Preferences.Default.Get("UserId", "0");

                if (SelectedFile != null)
                {
                    ImagenUrl = await UploadToCloudinary(SelectedFile);
                }

                var response = await _departamentoService.CreateDepartamentoAsync(
                    _titulo,
                    _descripcion,
                    _localizacion,
                    _ciudad,
                    _habitaciones,
                    _baños,
                    _lugaresCercanos,
                    _imagenUrl,
                    int.Parse(userId)
                );

                if (response != null)
                {
                    await Shell.Current.GoToAsync("..");
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
            if (string.IsNullOrWhiteSpace(_titulo) ||
                string.IsNullOrWhiteSpace(_descripcion) ||
                string.IsNullOrWhiteSpace(_ciudad) ||
                string.IsNullOrWhiteSpace(_localizacion))
            {
                ErrorMessage = "Complete todos los campos obligatorios";
                return false;
            }

            if (_habitaciones <= 0 || _baños <= 0)
            {
                ErrorMessage = "El número de habitaciones y baños debe ser mayor a 0";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task PickImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Seleccionar imagen",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedFile = result;
                        SelectedImage = ImageSource.FromFile(result.FullPath);
                    }
                    else
                    {
                        ErrorMessage = "Por favor selecciona una imagen JPG o PNG";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al cargar la imagen: {ex.Message}";
            }
        }
    }
}