using System.ComponentModel;

namespace FluentTorrent.Contracts.ViewModels;

public interface INavigationAware
{
    event PropertyChangedEventHandler PropertyChanged;

    void OnNavigatedTo(object parameter);

    void OnNavigatedFrom();
}
