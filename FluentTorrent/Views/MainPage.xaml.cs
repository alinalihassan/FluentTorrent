using CommunityToolkit.WinUI.UI.Controls;
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
        if (sender is DataGridRow row)
        {
            MenuFlyout flyout = new MenuFlyout();

            // Make items
            MenuFlyoutItem flyout_resume = new MenuFlyoutItem { Text = "Resume" };
            MenuFlyoutItem flyout_pause = new MenuFlyoutItem { Text = "Pause" };
            MenuFlyoutSeparator flyout_sep1 = new MenuFlyoutSeparator();
            MenuFlyoutItem flyout_delete = new MenuFlyoutItem { Text = "Delete" };

            // Add items
            flyout.Items.Add(flyout_resume);
            flyout.Items.Add(flyout_pause);
            flyout.Items.Add(flyout_sep1);
            flyout.Items.Add(flyout_delete);

            // Show the MenuFlyout
            flyout.ShowAt(row, e.GetPosition(row));
        }
    }

    private void TorrentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        DataGridRow row = e.Row;

        row.RightTapped += TorrentGrid_RightTapped;
    }
    #endregion

    #region AppBar Buttons
    private async void AppBarButton_AddTorrent(object sender, RoutedEventArgs e)
    {
        var filePicker = new FileOpenPicker();
        filePicker.FileTypeFilter.Add(".torrent");

        InitializeWithWindow.Initialize(filePicker, WindowNative.GetWindowHandle(App.MainWindow));
        StorageFile file = await filePicker.PickSingleFileAsync();

        if (file != null)
        {
            // Do something with the file.
        }
    }

    private async void AppBarButton_DeleteTorrent(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog()
        {
            XamlRoot = XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Remove Torrent",
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = new DeleteDialogContent()
        };

        await dialog.ShowAsync();
    }
    #endregion
}
