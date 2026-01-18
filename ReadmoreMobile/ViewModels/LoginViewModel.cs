using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadmoreMobile.Services;

namespace ReadmoreMobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _auth;

    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string errorMessage = "";

    public LoginViewModel(IAuthService auth)
    {
        _auth = auth;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = "";

        try
        {
            var token = await _auth.LoginAsync(Email, Password);
            if (string.IsNullOrWhiteSpace(token))
            {
                ErrorMessage = "Login mislukt";
                return;
            }

            await Shell.Current.GoToAsync("books");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
