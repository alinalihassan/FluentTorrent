using FluentTorrent.Contracts.Services;
using MonoTorrent.Client;

namespace FluentTorrent.Services;

public class TorrentDataService : ITorrentDataService
{
    private List<TorrentManager> _allTorrents;

    public TorrentDataService()
    {

    }

    private static IEnumerable<TorrentManager> AllTorrents()
    {
        return new List<TorrentManager>()
        {
            
        };
    }

    public async Task<IEnumerable<TorrentManager>> GetGridDataAsync()
    {
        _allTorrents ??= new List<TorrentManager>(AllTorrents());

        await Task.CompletedTask;
        return _allTorrents;
    }
}
