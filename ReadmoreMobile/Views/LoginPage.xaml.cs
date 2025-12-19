using ReadmoreMobile.ViewModels;

namespace ReadmoreMobile.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
