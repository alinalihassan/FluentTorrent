using FluentTorrent.ViewModels;
using FluentTorrent.Views.Modals;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
}
