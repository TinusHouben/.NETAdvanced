using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReadmoreMobile.Services;
using ReadmoreMobile.ViewModels;
using ReadmoreMobile.Views;

namespace ReadmoreMobile
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

            builder.Configuration.AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: false);

            var apiBaseUrl = builder.Configuration["ApiBaseUrl"]!;

            builder.Services.AddSingleton<TokenStore>();
            builder.Services.AddTransient<AuthHandler>();

            builder.Services.AddHttpClient(
                    "api",
                    c => c.BaseAddress = new Uri(apiBaseUrl))
                .AddHttpMessageHandler<AuthHandler>();

            builder.Services.AddSingleton<AuthApi>();

            builder.Services.AddSingleton<BooksApi>();
            builder.Services.AddTransient<BooksViewModel>();
            builder.Services.AddTransient<BooksPage>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
