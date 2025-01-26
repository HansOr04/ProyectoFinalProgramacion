using APPPugaOrtizLopez.Services;
using APPPugaOrtizLopez.ViewModels;
using Microsoft.Extensions.Logging;

namespace APPPugaOrtizLopez
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
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IDepartamentoService, DepartamentoService>();
            builder.Services.AddSingleton<IComentarioService, ComentarioService>();

            // ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<IApiPublicService, ApiPublicService>();
            builder.Services.AddSingleton<AllFlatsViewModel>();
            builder.Services.AddTransient<CreateFlatViewModel>();
            builder.Services.AddTransient<FlatDetailsViewModel>();

            // Views
            builder.Services.AddSingleton<Views.Main.MainPage>();
            builder.Services.AddSingleton<Views.Auth.LoginPage>();
            builder.Services.AddSingleton<Views.Auth.RegisterPage>();
            builder.Services.AddSingleton<Views.Flats.AllFlatsPage>();
            builder.Services.AddTransient<Views.Flats.CreateFlatPage>();
            builder.Services.AddTransient<Views.Flats.FlatDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
