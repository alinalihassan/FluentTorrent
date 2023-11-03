using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTorrent.Contracts.Services;
using FluentTorrent.Contracts.ViewModels;
using FluentTorrent.Models;

namespace FluentTorrent.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{

    private readonly ITorrentDataService _torrentDataService;

    public ObservableCollection<Torrent> Source { get; } = new ObservableCollection<Torrent>();

    public MainViewModel(ITorrentDataService torrentDataService)
    {
        _torrentDataService = torrentDataService;
    }
    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _torrentDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}