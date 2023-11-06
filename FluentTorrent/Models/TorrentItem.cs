using System.ComponentModel;
using System.Diagnostics;
using MonoTorrent;
using MonoTorrent.Client;

namespace FluentTorrent.Models;
public class TorrentItem : IAsyncDisposable, INotifyPropertyChanged
{
    private readonly TorrentManager _manager;

    public string Name => _manager.Torrent.Name;
    public double Size => _manager.Torrent.Size;
    public string Comment => _manager.Torrent.Comment;
    public TorrentState State => _manager.State;
    public double Progress => _manager.Progress;
    public double DownloadedBytes => _manager.Torrent.Size * (_manager.Progress / 100); // _manager.Monitor.DataBytesDownloaded;
    public double UploadedBytes => _manager.Monitor.DataBytesUploaded;
    public double RemainingBytes => _manager.Torrent.Size - (_manager.Torrent.Size * (_manager.Progress / 100));
    public double DownloadSpeed => _manager.Monitor.DownloadSpeed;
    public double UploadSpeed => _manager.Monitor.UploadSpeed;

    private TorrentItem(TorrentManager manager)
    {
        _manager = manager;
    }

    // Implement the INotifyPropertyChanged interface
    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertiesChanged()
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        });
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
    }

    public async Task Pause()
    {
        await _manager.PauseAsync();
    }

    public async Task Stop()
    {
        await _manager.StopAsync();
    }

    public async Task Delete(bool shouldDeleteFiles)
    {
        await _manager.StopAsync();

        Debug.WriteLine($"Deleting with shouldDeleteFiles = {shouldDeleteFiles}");
        await App.TorrentEngine.RemoveAsync(_manager, shouldDeleteFiles ? RemoveMode.CacheDataAndDownloadedData : RemoveMode.CacheDataOnly);

        if (shouldDeleteFiles)
        {
            // TODO: Delete the folders
            Debug.WriteLine($"Save path {_manager.SavePath}");
            Debug.WriteLine($"Files: {Directory.GetFiles(_manager.SavePath, "*", SearchOption.AllDirectories)}");
            //if (!Directory.GetFiles(_manager.SavePath, "*", SearchOption.AllDirectories).Any())
            //{
            //    Directory.Delete(_manager.SavePath, true);
            //}
        }

        Debug.WriteLine($"Deleted {Name}");
    }

    public async ValueTask DisposeAsync()
    {
        await _manager.StopAsync();
    }
}