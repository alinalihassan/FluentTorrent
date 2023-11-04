using System.Collections.ObjectModel;
using MonoTorrent.Client;

namespace FluentTorrent.Contracts.Services;
public interface ITorrentServiceManager
{
    Task<TorrentManager> AddTorrentFile(string path);
    Task<TorrentManager> AddMagnetLink(string links);
    ITorrentService? FindTorrentService(TorrentManager torrentManager);
    ObservableCollection<TorrentManager> GetTorrentManagers();
    ObservableCollection<ITorrentService> GetTorrentServices();
}