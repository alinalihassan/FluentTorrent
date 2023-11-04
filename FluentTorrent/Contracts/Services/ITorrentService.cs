using MonoTorrent.Client;

namespace FluentTorrent.Contracts.Services;
public interface ITorrentService
{
    public TorrentManager torrentManager
    {
        get;
    }

    Task Start();
    Task Resume();
    Task Stop();
}