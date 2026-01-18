using ReadmoreMobile.Services;
using ReadmoreMobile.Views;

namespace ReadmoreMobile;

public partial class AppShell : Shell
{
    private readonly IAuthService _auth;

    public AppShell(IAuthService auth)
    {
        InitializeComponent();
        _auth = auth;

        Routing.RegisterRoute("books", typeof(BooksPage));

        Loaded += AppShell_Loaded;
    }

    private async void AppShell_Loaded(object? sender, EventArgs e)
    {
        var token = await _auth.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            await GoToAsync("books");
        }
    }
}
