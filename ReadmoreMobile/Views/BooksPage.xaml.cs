using ReadmoreMobile.Services;

namespace ReadmoreMobile.Views;

public partial class BooksPage : ContentPage
{
    private readonly IAuthService _auth;

    public BooksPage()
    {
        InitializeComponent();
        _auth = Application.Current!.Handler!.MauiContext!.Services.GetService<IAuthService>()!;
    }

    private async void Logout_Clicked(object sender, EventArgs e)
    {
        await _auth.LogoutAsync();
        await Shell.Current.GoToAsync("login");
    }
}
