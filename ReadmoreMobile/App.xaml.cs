using ReadmoreMobile.Views;

namespace ReadmoreMobile
{
    public partial class App : Application
    {
        public App(LoginPage loginPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(loginPage);
        }
    }
}
