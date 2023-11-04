using FluentTorrent.Contracts.Services;
using FluentTorrent.Models;

namespace FluentTorrent.Services;

public class TorrentDataService : ITorrentDataService
{
    private List<TorrentItem> _allTorrents;

    public TorrentDataService()
    {

    }

    private static IEnumerable<TorrentItem> AllTorrents()
    {
        return new List<TorrentItem>()
        {
            
        };
    }

    public async Task<IEnumerable<TorrentItem>> GetGridDataAsync()
    {
        _allTorrents ??= new List<TorrentItem>(AllTorrents());

        await Task.CompletedTask;
        return _allTorrents;
    }
}
