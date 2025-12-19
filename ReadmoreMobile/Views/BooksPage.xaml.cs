using ReadmoreMobile.ViewModels;

namespace ReadmoreMobile.Views;

public partial class BooksPage : ContentPage
{
    public BooksPage(BooksViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Loaded += async (_, _) => await vm.LoadAsync();
    }
}
