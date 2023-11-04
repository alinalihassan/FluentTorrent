using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTorrent.Contracts.Services;
using FluentTorrent.Contracts.ViewModels;
using MonoTorrent.Client;

namespace FluentTorrent.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly ITorrentServiceManager _torrentServiceManager;

    public ObservableCollection<TorrentManager> Source { get; } = new ObservableCollection<TorrentManager>();

    public MainViewModel(ITorrentServiceManager torrentServiceManager)
    {
        _torrentServiceManager = torrentServiceManager;
    }
    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        await Task.Delay(500);
        var torrentManagers = _torrentServiceManager.GetTorrentManagers();

        foreach ( var torrentManager in torrentManagers )
        {
            Source.Add(torrentManager);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}