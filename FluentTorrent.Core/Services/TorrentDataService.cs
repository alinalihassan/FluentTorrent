using FluentTorrent.Contracts.Services;
using FluentTorrent.Models;

namespace FluentTorrent.Services;

// This class holds sample data used by some generated pages to show how they can be used.
// TODO: The following classes have been created to display sample data. Delete these files once your app is using real data.
// 1. Contracts/Services/ISampleDataService.cs
// 2. Services/SampleDataService.cs
// 3. Models/SampleCompany.cs
// 4. Models/SampleOrder.cs
// 5. Models/SampleOrderDetail.cs
public class TorrentDataService : ITorrentDataService
{
    private List<Torrent> _allTorrents;

    public TorrentDataService()
    {
    }

    private static IEnumerable<Torrent> AllTorrents()
    {
        return new List<Torrent>()
        {
            new Torrent(){
                Id = 1,
                Name = "Cyberpunk",
                Url = "https://google.com"},

            new Torrent(){
                Id = 2,
                Name = "Cyberpunk",
                Url = "https://google.com"},
            new Torrent(){
                Id = 2,
                Name = "Cyberpunk",
                Url = "https://google.com"},
            new Torrent(){
                Id = 2,
                Name = "Cyberpunk",
                Url = "https://google.com"},
            new Torrent(){
                Id = 2,
                Name = "Cyberpunk",
                Url = "https://google.com"},
        };
    }

    public async Task<IEnumerable<Torrent>> GetGridDataAsync()
    {
        _allTorrents ??= new List<Torrent>(AllTorrents());

        await Task.CompletedTask;
        return _allTorrents;
    }
}
