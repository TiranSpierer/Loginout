// Created by Tiran Spierer
// Created at 11/01/2023
// Class purpose:

using Demo_DatabaseApp.ViewModels.Interfaces;
using Prism.Events;

namespace Demo_DatabaseApp.Stores;

public class NavigationStore<TEvent> : INavigationStore where TEvent : PubSubEvent, new()
{
    private readonly IEventAggregator    _ea;
    private          INavigableViewModel _currentViewModel = null!;

    public NavigationStore(IEventAggregator ea)
    {
        _ea = ea;
    }

    public INavigableViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel?.Dispose();
            _currentViewModel = value;
            _ea.GetEvent<TEvent>().Publish();
        }
    }
}