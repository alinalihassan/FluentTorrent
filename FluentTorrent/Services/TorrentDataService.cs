using FluentTorrent.Contracts.Services;
using FluentTorrent.Models;
using MonoTorrent.Client;

namespace FluentTorrent.Services;

public class TorrentDataService : ITorrentDataService
{
    private List<TorrentItem> _allTorrents;

    public TorrentDataService()
    {

    }

    private static async Task<IEnumerable<TorrentItem>> AllTorrents()
    {
        return new List<TorrentItem>()
        {
            await TorrentItem.AddTorrentFile("C:\\Users\\super\\Downloads\\ubuntu-23.10.1-desktop-amd64.iso.torrent")
        };
    }

    public async Task<IEnumerable<TorrentItem>> GetGridDataAsync()
    {
        _allTorrents ??= new List<TorrentItem>(await AllTorrents());

        await Task.CompletedTask;
        return _allTorrents;
    }
}
