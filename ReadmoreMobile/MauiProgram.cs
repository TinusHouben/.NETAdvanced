using Microsoft.Extensions.Logging;
using ReadmoreMobile.Services;
using ReadmoreMobile.ViewModels;
using ReadmoreMobile.Views;

namespace ReadmoreMobile;

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

        builder.Services.AddSingleton(sp =>
        {
#if ANDROID
            var baseUrl = "http://10.0.2.2:5031";
#else
            var baseUrl = "http://localhost:5031";
#endif
            var http = new HttpClient();
            http.BaseAddress = new Uri(baseUrl);
            return http;
        });

        builder.Services.AddSingleton<IAuthService, AuthService>();

        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<BooksPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
