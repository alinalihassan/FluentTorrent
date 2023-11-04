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

    public FastResume? resume
    {
    get; set; }

    public TorrentItem(TorrentManager torrent, CancellationToken cancellationToken)
    {
        this.torrent = torrent;
        this.cancellationToken = cancellationToken;
    }
}
