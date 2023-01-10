using System;
using Demo_DatabaseApp.ViewModels.Surviews;

namespace Demo_DatabaseApp.Stores
{
    public class NavigationStore
    {
        private ViewModelBase _currentViewModel = null!;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public event Action CurrentViewModelChanged = null!;
    }
}