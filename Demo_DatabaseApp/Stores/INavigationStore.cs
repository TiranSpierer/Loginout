// Created by Tiran Spierer
// Created at 11/01/2023
// Class purpose:

using Demo_DatabaseApp.ViewModels.Interfaces;
using Demo_DatabaseApp.ViewModels.Surviews;
using Prism.Events;

namespace Demo_DatabaseApp.Stores;

public interface INavigationStore
{
    INavigableViewModel CurrentViewModel { get; set; }
}