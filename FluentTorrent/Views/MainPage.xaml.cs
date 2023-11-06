using System.Diagnostics;
using CommunityToolkit.WinUI.UI.Controls;
using FluentTorrent.Models;
using FluentTorrent.Services;
using FluentTorrent.ViewModels;
using FluentTorrent.Views.Modals;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace FluentTorrent.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    #region Grid Events
    private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        TorrentGrid.SelectedItems.Clear();
        TorrentGrid.SelectedItem = null;
    }

    private void TorrentGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
        if (!(sender is DataGridRow row) || !(row.DataContext is TorrentItem torrentItem))
        {
            return;
        }

        var flyout = new MenuFlyout();

        var flyout_resume = new MenuFlyoutItem { Text = "Resume", Icon = new FontIcon { Glyph = "\uE768" } };
        flyout_resume.Click += async (s, args) =>
        {
            await torrentItem.Start();
        };

        var flyout_pause = new MenuFlyoutItem { Text = "Pause", Icon = new FontIcon { Glyph = "\uE769" } };
        flyout_pause.Click += async (s, args) =>
        {
            await torrentItem.Pause();
        };

        var flyout_sep1 = new MenuFlyoutSeparator();

        var flyout_delete = new MenuFlyoutItem { Text = "Delete", Icon = new FontIcon { Glyph = "\uE74D" } };
        flyout_delete.Click += (s, args) =>
        {
            // Handle Delete click for the specific TorrentItem
        };

        // Add items to the flyout
        flyout.Items.Add(flyout_resume);
        flyout.Items.Add(flyout_pause);
        flyout.Items.Add(flyout_sep1);
        flyout.Items.Add(flyout_delete);

        // Show the MenuFlyout
        flyout.ShowAt(row, e.GetPosition(row));
    }

    private void TorrentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        DataGridRow row = e.Row;

        row.RightTapped += TorrentGrid_RightTapped;
    }
    private void TorrentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            var selectedTorrentItem = e.AddedItems[0] as TorrentItem; // Assuming your DataGrid is bound to a collection of TorrentItem objects

            Debug.WriteLine($"Torrent item: {selectedTorrentItem}");
        }
    }

    #endregion

    #region AppBar Buttons
    private async void AppBarButton_AddTorrentFile(object sender, RoutedEventArgs e)
    {
        var filePicker = new FileOpenPicker();
        filePicker.FileTypeFilter.Add(".torrent");

        InitializeWithWindow.Initialize(filePicker, WindowNative.GetWindowHandle(App.MainWindow));
        StorageFile file = await filePicker.PickSingleFileAsync();

        if (file != null)
        {
            await ViewModel.TorrentDataService.AddTorrentFile(file.Path);
            Debug.WriteLine($"DataService length {ViewModel.TorrentDataService.GetAllTorrents().ToArray().Length}");
        }
    }

    private async void AppBarButton_AddTorrentMagnet(object sender, RoutedEventArgs e)
    {
        var dialogContent = new AddMagnetDialogContent();
        var dialog = new ContentDialog()
        {
            XamlRoot = XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Add Torrent Links",
            PrimaryButtonText = "Download",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = dialogContent
        };

        if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        {
            var _ = dialogContent.GetMagnetLinks();
            // TODO: Do something with magnet links
        }
    }

    private async void AppBarButton_DeleteTorrent(object sender, RoutedEventArgs e)
    {
        var dialogContent = new DeleteDialogContent();
        var dialog = new ContentDialog()
        {
            XamlRoot = XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Remove Torrent",
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = dialogContent
        };

        if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        {
            var _ = dialogContent.ShouldDeleteFiles();
            // TODO: Delete torrent and file
        }
    }
    #endregion

}
