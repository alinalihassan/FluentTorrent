using FluentTorrent.Models;

namespace FluentTorrent.Contracts.Services;
public interface ITorrentDataService
{
    event EventHandler DataUpdated;
    Task<TorrentItem> AddTorrentFile(string path);
    Task<TorrentItem> AddMagnetLink(string links);
    IEnumerable<TorrentItem> GetAllTorrents();
}
