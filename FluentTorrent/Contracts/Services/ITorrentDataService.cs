using FluentTorrent.Models;
using MonoTorrent.Client;

namespace FluentTorrent.Contracts.Services;
public interface ITorrentDataService
{
    Task<IEnumerable<TorrentManager>> GetGridDataAsync();
}
