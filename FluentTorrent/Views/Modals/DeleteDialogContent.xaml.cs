﻿using Microsoft.UI.Xaml.Controls;

namespace FluentTorrent.Views.Modals;
public sealed partial class DeleteDialogContent : Page
{
    public DeleteDialogContent()
    {
        this.InitializeComponent();
    }

    public bool ShouldDeleteFiles()
    {
        return DialogDeleteFiles.IsChecked ?? false;
    }
}
