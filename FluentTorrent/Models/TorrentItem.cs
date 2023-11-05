using MonoTorrent.Client;
using MonoTorrent;
using System.ComponentModel;
using System.Diagnostics;
using Windows.UI.Core;

namespace FluentTorrent.Models;
public class TorrentItem : IAsyncDisposable, INotifyPropertyChanged
{
    private readonly TorrentManager _manager;
    private double _progress;

    public string Name => _manager.Torrent.Name;
    public double Size => _manager.Torrent.Size;
    public string Comment => _manager.Torrent.Comment;
    public TorrentState State => _manager.State;

    public double Progress
    {
        get => _progress;
        set
        {
            if (_progress != value)
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
    }
    public double DownloadedBytes => _manager.Monitor.DataBytesDownloaded;
    public double UploadedBytes => _manager.Monitor.DataBytesUploaded;
    public double RemainingBytes => _manager.Bitfield.TrueCount;
    public double DownloadSpeed => _manager.Monitor.DownloadSpeed;
    public double UploadSpeed => _manager.Monitor.UploadSpeed;
    private TorrentItem(TorrentManager manager)
    {
        _manager = manager;
        _manager.PieceHashed += _manager_PieceHashed;
    }

    private void _manager_PieceHashed(object? sender, PieceHashedEventArgs e)
    {
        Debug.WriteLine($"Piece Hashed! {e.TorrentManager.Progress}");
        try
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                Progress = e.TorrentManager.Progress;
                //DownloadedBytes = e.TorrentManager.Monitor.DataBytesDownloaded;
                //UploadedBytes = e.TorrentManager.Progress;
                //RemainingBytes = e.TorrentManager.Progress;
                //DownloadSpeed = e.TorrentManager.Progress;
                //UploadSpeed = e.TorrentManager.Progress;
            });
        }
        catch
        {
            Debug.WriteLine("Oops");
        }
    }

    #region Static Instantiation Methods
    public static async Task<TorrentItem> AddTorrentFile(string path)
    {
        var torrentItem = new TorrentItem(await App.TorrentEngine.AddAsync(path, App.DownloadsFolder));

        // Start the torrent
        _ = torrentItem.Start();

        return torrentItem;
    }
    public static async Task<TorrentItem> AddMagnetLink(string links)
    {

        MagnetLink.TryParse(links, out var link);
        var torrentItem = new TorrentItem(await App.TorrentEngine.AddAsync(link, App.DownloadsFolder));

        // Start the torrent
        _ = torrentItem.Start();

        return torrentItem;
    }
    #endregion

    public async Task Start()
    {
        await _manager.StartAsync();

        while (Progress < 100)
        {
            Debug.WriteLine($"Downloading {Name}: {Progress:0.00}%");
            await Task.Delay(500); // Example delay
        }

        Debug.WriteLine($"Downloaded {Name}");
    }

    public async Task Pause()
    {
        await _manager.PauseAsync();
    }

    public async Task Stop()
    {
        await _manager.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _manager.StopAsync();
    }

    // Implement the INotifyPropertyChanged interface
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        Debug.WriteLine($"Property Changed {propertyName}");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}