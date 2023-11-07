using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using FluentTorrent.Contracts.Services;
using FluentTorrent.Contracts.ViewModels;
using FluentTorrent.Models;

namespace FluentTorrent.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    public readonly ITorrentDataService TorrentDataService;

    public ObservableCollection<TorrentItem> Source { get; } = new ObservableCollection<TorrentItem>();
    public double SizeDownloaded => Source.Sum(item  => item.Manager.Monitor.DataBytesDownloaded);
    public double SizeUploaded => Source.Sum(item => item.Manager.Monitor.DataBytesUploaded);
    public double DownloadSpeed => App.TorrentEngine.TotalDownloadSpeed;
    public double UploadSpeed => App.TorrentEngine.TotalUploadSpeed;

    public MainViewModel(ITorrentDataService torrentDataService)
    {
        TorrentDataService = torrentDataService;
        TorrentDataService.DataUpdated += OnDataUpdated;
        App.TorrentEngine.StatsUpdate += TorrentEngine_StatsUpdate;
    }
    private void OnDataUpdated(object? sender, EventArgs e) => RefreshData();
    public void OnNavigatedTo(object parameter) => RefreshData();

    public void RefreshData()
    {
        Source.Clear();

        var torrents = TorrentDataService.GetAllTorrents();

        foreach (var torrent in torrents)
        {
            Source.Add(torrent);
        }

        OnPropertyChanged(nameof(SizeDownloaded));
        OnPropertyChanged(nameof(SizeUploaded));
        OnPropertyChanged(nameof(DownloadSpeed));
        OnPropertyChanged(nameof(UploadSpeed));
    }

    private async void TorrentEngine_StatsUpdate(object? sender, MonoTorrent.Client.StatsUpdateEventArgs e)
    {
        foreach (var torrent in Source)
        {
            torrent.NotifyPropertiesChanged();
        }

        await App.MainWindow.DispatcherQueue.EnqueueAsync(() =>
        {
            OnPropertyChanged(nameof(SizeDownloaded));
            OnPropertyChanged(nameof(SizeUploaded));
            OnPropertyChanged(nameof(DownloadSpeed));
            OnPropertyChanged(nameof(UploadSpeed));
        });
    }

    public void OnNavigatedFrom()
    {
    }
}