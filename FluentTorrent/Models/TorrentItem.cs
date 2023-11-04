using MonoTorrent.Client;

namespace FluentTorrent.Models;
public class TorrentItem
{
    public TorrentManager torrent
    {
        get; set;
    }

    public CancellationToken cancellationToken
    {
        get; set;
    }

    TorrentItem(TorrentManager torrent, CancellationToken cancellationToken)
    {
        this.torrent = torrent;
        this.cancellationToken = cancellationToken;
    }
}
