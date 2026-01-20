using System.Collections.ObjectModel;
using System.Windows.Input;
using ReadmoreMobile.Services;

namespace ReadmoreMobile.ViewModels;

public class BooksViewModel : BaseViewModel
{
    private readonly BooksApi _api;

    public ObservableCollection<BookDto> Books { get; } = new();

    public ICommand LoadCommand { get; }

    public BooksViewModel(BooksApi api)
    {
        _api = api;
        LoadCommand = new Command(async () => await LoadAsync());
    }

    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            var items = await _api.GetBooksAsync();
            Books.Clear();
            foreach (var item in items)
                Books.Add(item);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
