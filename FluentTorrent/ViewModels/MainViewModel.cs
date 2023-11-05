using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTorrent.Contracts.Services;
using FluentTorrent.Contracts.ViewModels;
using FluentTorrent.Models;

namespace FluentTorrent.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly ITorrentDataService _torrentDataService;

    public ObservableCollection<TorrentItem> Source { get; } = new ObservableCollection<TorrentItem>();

    public MainViewModel(ITorrentDataService torrentDataService)
    {
        _torrentDataService = torrentDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        await Task.Delay(500);

        var torrents = await _torrentDataService.GetGridDataAsync();

        foreach ( var torrent in torrents)
        {
            Source.Add(torrent);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}