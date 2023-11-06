using System.Diagnostics;
using FluentTorrent.Contracts.Services;
using FluentTorrent.Models;
using MonoTorrent.Client;

namespace FluentTorrent.Services;

public class TorrentDataService : ITorrentDataService
{
    private readonly List<TorrentItem> _allTorrents = new();
    public event EventHandler DataUpdated;

    public TorrentDataService()
    {

    }
    private void OnDataUpdated()
    {
        DataUpdated?.Invoke(this, EventArgs.Empty);
    }

    public async Task<TorrentItem> AddTorrentFile(string path)
    {
        var torrent = await TorrentItem.AddTorrentFile(path);
        _allTorrents.Add(torrent);
        OnDataUpdated();

        return torrent;
    }
    public async Task<TorrentItem> AddMagnetLink(string links)
    {
        var torrent = await TorrentItem.AddMagnetLink(links);
        _allTorrents.Add(torrent);
        OnDataUpdated();

        return torrent;
    }

    public async Task RemoveTorrent(TorrentItem torrentItem, bool shouldDeleteFiles)
    {
        _allTorrents.Remove(torrentItem);
        _ = torrentItem.Delete(shouldDeleteFiles);
        Debug.WriteLine($"Torrent: {_allTorrents.ToArray().Length}");
        Debug.WriteLine($"Torrent: {_allTorrents.ToArray().Length}");
    } 

    public IEnumerable<TorrentItem> GetAllTorrents()
    {
        return _allTorrents;
    }
}
