using FluentTorrent.Contracts.Services;
using MonoTorrent;
using MonoTorrent.Client;

namespace FluentTorrent.Services;
public class TorrentServiceManager: IDisposable, ITorrentServiceManager
{
    private List<ITorrentService> _services;

    public TorrentServiceManager()
    {
        _services = new List<ITorrentService>();
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

    public void Dispose()
    {
        foreach (var service in _services)
        {
            service.Stop();
        }
    }
}