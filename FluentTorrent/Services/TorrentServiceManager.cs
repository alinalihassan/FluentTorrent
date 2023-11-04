using System.Collections.ObjectModel;
using FluentTorrent.Contracts.Services;
using MonoTorrent;
using MonoTorrent.Client;

namespace FluentTorrent.Services;
public class TorrentServiceManager: IDisposable, ITorrentServiceManager
{
    private readonly ObservableCollection<ITorrentService> _services;
    public IReadOnlyList<ITorrentService> Services => _services;

    public TorrentServiceManager()
    {
        _services = new ObservableCollection<ITorrentService>();
    }

    public async Task<TorrentManager> AddTorrentFile(string path)
    {
        var manager = await App.TorrentEngine.AddAsync(path, App.DownloadsFolder);
        var service = new TorrentService(manager);
        _services.Add(service);
        _ = service.Start();

        return manager;
    }
    public async Task<TorrentManager> AddMagnetLink(string links)
    {
        MagnetLink.TryParse(links, out var link);
        var manager = await App.TorrentEngine.AddAsync(link, App.DownloadsFolder);

        var service = new TorrentService(manager);
        _services.Add(service);
        _ = service.Start();

        return manager;
    }

    public ITorrentService? FindTorrentService(TorrentManager torrentManager)
    {
        foreach (var service in _services)
        {
            if (service.torrentManager == torrentManager)
            {
                return service;
            }
        }

        return null;
    }

    public ObservableCollection<TorrentManager> GetTorrentManagers()
    {
        var list = new ObservableCollection<TorrentManager>();

        foreach (var service in _services)
        {
            list.Add(service.torrentManager);
        }

        return list;
    }

    public ObservableCollection<ITorrentService> GetTorrentServices() => _services;

    public void Dispose()
    {
        foreach (var service in _services)
        {
            service.Stop();
        }
    }
}