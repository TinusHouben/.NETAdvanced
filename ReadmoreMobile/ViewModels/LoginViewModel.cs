using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadmoreMobile.Models;
using ReadmoreMobile.Services;

namespace ReadmoreMobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly AuthApi _auth;
    private readonly TokenStore _store;

    private string email = "";
    public string Email { get => email; set => SetProperty(ref email, value); }

    private string password = "";
    public string Password { get => password; set => SetProperty(ref password, value); }

    private string error = "";
    public string Error { get => error; set => SetProperty(ref error, value); }

    public ICommand LoginCommand { get; }

    public LoginViewModel(AuthApi auth, TokenStore store)
    {
        _auth = auth;
        _store = store;
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
        await Application.Current.MainPage.DisplayAlert("OK", $"Ingelogd als {data.Email}", "OK");
    }
}
