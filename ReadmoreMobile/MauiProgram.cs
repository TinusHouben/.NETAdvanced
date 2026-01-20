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

        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddTransient<AuthHandler>();

#if ANDROID
        var baseUrl = "http://10.0.2.2:5031/";
#else
        var baseUrl = "http://localhost:5031/";
#endif

        builder.Services
            .AddHttpClient("api", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<AuthHandler>();

        builder.Services.AddTransient<AuthApi>();
        builder.Services.AddTransient<BooksApi>();

        builder.Services.AddSingleton<IAuthService, AuthService>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<BooksViewModel>();
        builder.Services.AddTransient<BooksPage>();

        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
