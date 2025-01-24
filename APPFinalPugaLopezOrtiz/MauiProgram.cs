// MauiProgram.cs
using Microsoft.Extensions.Logging;
using APPFinalPugaLopezOrtiz.Services;
using APPFinalPugaLopezOrtiz.ViewModels;
using APPFinalPugaLopezOrtiz.Views;

namespace APPFinalPugaLopezOrtiz
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<UsuarioService>();
            builder.Services.AddSingleton<DepartamentoService>();
            builder.Services.AddSingleton<ComentarioService>();

            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<DepartamentosViewModel>();
            builder.Services.AddTransient<DepartamentoDetailViewModel>();
            builder.Services.AddTransient<CreateDepartamentoViewModel>();
            builder.Services.AddTransient<EditDepartamentoViewModel>();

            // Pages
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<DepartamentosPage>();
            builder.Services.AddTransient<DepartamentoDetailPage>();
            builder.Services.AddTransient<CreateDepartamentoPage>();
            builder.Services.AddTransient<EditDepartamentoPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}