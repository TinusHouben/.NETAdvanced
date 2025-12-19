using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadmoreMobile.Services;
using ReadmoreWeb.Data.Models;
using System.Collections.ObjectModel;

namespace ReadmoreMobile.ViewModels;

public partial class BooksViewModel : ObservableObject
{
    private readonly BooksApi _api;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<Book> Books { get; } = new();

    public BooksViewModel(BooksApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        Books.Clear();
        foreach (var b in await _api.GetAsync())
            Books.Add(b);

        IsBusy = false;
    }
}
