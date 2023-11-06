using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTorrent.Contracts.Services;
using FluentTorrent.Contracts.ViewModels;
using FluentTorrent.Models;

namespace FluentTorrent.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    public readonly ITorrentDataService TorrentDataService;

    public ObservableCollection<TorrentItem> Source { get; } = new ObservableCollection<TorrentItem>();

    public MainViewModel(ITorrentDataService torrentDataService)
    {
        TorrentDataService = torrentDataService;
        TorrentDataService.DataUpdated += OnDataUpdated;
        App.TorrentEngine.StatsUpdate += TorrentEngine_StatsUpdate;
    }
    private void OnDataUpdated(object? sender, EventArgs e) => RefreshData();
    public void OnNavigatedTo(object parameter) => RefreshData();

    private void RefreshData()
    {
        Source.Clear();

        var torrents = TorrentDataService.GetAllTorrents();

        foreach (var torrent in torrents)
        {
            Source.Add(torrent);
        }
    }

    private void TorrentEngine_StatsUpdate(object? sender, MonoTorrent.Client.StatsUpdateEventArgs e)
    {
        foreach (var torrent in Source)
        {
            torrent.NotifyPropertiesChanged();
        }
    }

    public void OnNavigatedFrom()
    {
    }
}