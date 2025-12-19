using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using ReadmoreMobile.Models;
using ReadmoreMobile.Services;
using ReadmoreMobile.Views;

namespace ReadmoreMobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly AuthApi _auth;
    private readonly TokenStore _store;
    private readonly IServiceProvider _services;

    private string email = "";
    public string Email { get => email; set => SetProperty(ref email, value); }

    private string password = "";
    public string Password { get => password; set => SetProperty(ref password, value); }

    private string error = "";
    public string Error { get => error; set => SetProperty(ref error, value); }

    public ICommand LoginCommand { get; }

    public LoginViewModel(AuthApi auth, TokenStore store, IServiceProvider services)
    {
        _auth = auth;
        _store = store;
        _services = services;
        LoginCommand = new Command(async () => await LoginAsync());
    }

    private async Task LoginAsync()
    {
        Error = "";

        var (data, err) = await _auth.LoginAsync(new LoginRequestDto
        {
            Email = Email,
            Password = Password
        });

        if (data is null)
        {
            Error = err ?? "Login mislukt";
            return;
        }

        await _store.SaveAsync(data.Token, data.ExpiresAtUtc);

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var booksPage = _services.GetRequiredService<BooksPage>();

            if (Application.Current.MainPage is NavigationPage nav)
                await nav.Navigation.PushAsync(booksPage);
            else
                Application.Current.MainPage = new NavigationPage(booksPage);
        });
    }
}
