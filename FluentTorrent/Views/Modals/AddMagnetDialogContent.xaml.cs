using Microsoft.UI.Xaml.Controls;

namespace FluentTorrent.Views.Modals;
public sealed partial class AddMagnetDialogContent : Page
{
    public AddMagnetDialogContent()
    {
        this.InitializeComponent();
    }

    public string[] GetMagnetLinks()
    {
        return DialogMagnetLinks.Text.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(line => line.Trim())
                                         .Where(line => !string.IsNullOrWhiteSpace(line))
                                         .ToArray();
    }
}
