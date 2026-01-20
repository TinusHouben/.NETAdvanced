using ReadmoreMobile.ViewModels;

namespace ReadmoreMobile.Views;

public partial class BooksPage : ContentPage
{
    private readonly BooksViewModel _viewModel;

    public BooksPage(BooksViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.Books.Count == 0)
            await _viewModel.LoadAsync();
    }
}
