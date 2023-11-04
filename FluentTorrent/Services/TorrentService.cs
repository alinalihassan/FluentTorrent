using FluentTorrent.Contracts.Services;
using MonoTorrent.Client;

namespace FluentTorrent.Services;
public class TorrentService : IAsyncDisposable, ITorrentService
{
    private CancellationTokenSource? _cancellationTokenSource;
    //private FastResume? _fastResume;
    public TorrentManager torrentManager
    {
    get; private set; }

    public TorrentService(TorrentManager newtorrentManager)
    {
        torrentManager = newtorrentManager;
    }

    public async Task Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        await torrentManager.StartAsync();

        while (!_cancellationTokenSource.IsCancellationRequested && !torrentManager.Complete)
        {
            System.Diagnostics.Debug.WriteLine($"Downloading {torrentManager}: {torrentManager.Progress:0.00}%");
            await Task.Delay(500); // Example delay
        }

        System.Diagnostics.Debug.WriteLine($"Downloaded {torrentManager}");
    }

    public async Task Resume()
    {
    
    }

    public async Task Stop()
    {
        _cancellationTokenSource?.Cancel();
        System.Diagnostics.Debug.WriteLine($"Canceled downloading {torrentManager}");
        await torrentManager.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Stop();
    }
}