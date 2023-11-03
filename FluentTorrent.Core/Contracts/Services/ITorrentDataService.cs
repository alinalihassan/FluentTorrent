using FluentTorrent.Models;

namespace FluentTorrent.Contracts.Services;
public interface ITorrentDataService
{
    Task<IEnumerable<Torrent>> GetGridDataAsync();
}
