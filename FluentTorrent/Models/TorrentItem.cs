using System.ComponentModel;
using System.Diagnostics;
using MonoTorrent;
using MonoTorrent.Client;

namespace FluentTorrent.Models;
public class TorrentItem : IAsyncDisposable, INotifyPropertyChanged
{
    public readonly TorrentManager Manager;

    public string Name
    {
        get
        {
            if (!string.IsNullOrEmpty(Manager.Torrent?.Name))
                return Manager.Torrent.Name;
            if (!string.IsNullOrEmpty(Manager.MagnetLink?.Name))
                return Manager.MagnetLink.Name;
            return "";
        }
    }
    public double Size => Manager.Torrent.Size;
    public string Comment => Manager.Torrent.Comment;
    public TorrentState State => Manager.State;
    public double Progress => Manager.Progress;
    public double DownloadedBytes => Manager.Torrent.Size * (Manager.Progress / 100); // _manager.Monitor.DataBytesDownloaded;
    public double UploadedBytes => Manager.Monitor.DataBytesUploaded;
    public double RemainingBytes => Manager.Torrent.Size - (Manager.Torrent.Size * (Manager.Progress / 100));
    public double DownloadSpeed => Manager.Monitor.DownloadSpeed;
    public double UploadSpeed => Manager.Monitor.UploadSpeed;

    private TorrentItem(TorrentManager manager)
    {
        Manager = manager;
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
        await Manager.StartAsync();
    }

    public async Task Pause()
    {
        await Manager.PauseAsync();
    }

    public async Task Stop()
    {
        await Manager.StopAsync();
    }

    public async Task Delete(bool shouldDeleteFiles)
    {
        await Manager.StopAsync();

        Debug.WriteLine($"Deleting with shouldDeleteFiles = {shouldDeleteFiles}");
        await App.TorrentEngine.RemoveAsync(Manager, shouldDeleteFiles ? RemoveMode.CacheDataAndDownloadedData : RemoveMode.CacheDataOnly);

        if (shouldDeleteFiles)
        {
            // TODO: Delete the folders
            Debug.WriteLine($"Save path {Manager.SavePath}");
            Debug.WriteLine($"Files: {Directory.GetFiles(Manager.SavePath, "*", SearchOption.AllDirectories)}");
            //if (!Directory.GetFiles(_manager.SavePath, "*", SearchOption.AllDirectories).Any())
            //{
            //    Directory.Delete(_manager.SavePath, true);
            //}
        }

        Debug.WriteLine($"Deleted {Name}");
    }

    public async ValueTask DisposeAsync()
    {
        await Manager.StopAsync();
    }
}