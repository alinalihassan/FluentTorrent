using System.Diagnostics;
using CommunityToolkit.WinUI.UI.Controls;
using FluentTorrent.Models;
using FluentTorrent.ViewModels;
using FluentTorrent.Views.Modals;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using MonoTorrent.Client;
using Windows.System;

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

        MenuFlyoutItem flyout_toggle_state;
        if (torrentItem.State == TorrentState.Stopped || torrentItem.State == TorrentState.Stopping || torrentItem.State == TorrentState.Paused)
        {
            flyout_toggle_state = new MenuFlyoutItem { Text = "Resume", Icon = new FontIcon { Glyph = "\uE768" } };
            flyout_toggle_state.Click += async (s, args) =>
            {
                await torrentItem.Start();
            };
        }
        else
        {
            flyout_toggle_state = new MenuFlyoutItem { Text = "Pause", Icon = new FontIcon { Glyph = "\uE769" } };
            flyout_toggle_state.Click += async (s, args) =>
            {
                await torrentItem.Pause();
            };
        }
        
        // TODO: Make it Open the containing folder if it's not a single file
        var flyout_open_folder = new MenuFlyoutItem { Text = "Open Folder", Icon = new FontIcon { Glyph = "\uE8b7" } };
        flyout_open_folder.Click += async (s, args) =>
        {
            await Launcher.LaunchFolderAsync(await StorageFolder.GetFolderFromPathAsync(App.DownloadsFolder));
        };

        var flyout_sep1 = new MenuFlyoutSeparator();

        var flyout_delete = new MenuFlyoutItem { Text = "Delete", Icon = new FontIcon { Glyph = "\uE74D" } };
        flyout_delete.Click += async (s, args) =>
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
                var shouldDeleteFiles = dialogContent.ShouldDeleteFiles();
                await ViewModel.TorrentDataService.RemoveTorrent(torrentItem, shouldDeleteFiles);
                ViewModel.RefreshData();
            }
        };

        // Add items to the flyout
        flyout.Items.Add(flyout_toggle_state);
        flyout.Items.Add(flyout_open_folder);
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
