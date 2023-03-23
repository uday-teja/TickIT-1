using Caliburn.Micro;
using System.Reflection.Metadata;

namespace TickIT.App.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {
        #region Fields

        private readonly SimpleContainer _container;
        private bool _isProgressRingActive = false;

        #endregion

        #region Properties
        public bool IsProgressRingActive
        {
            get
            {
                return _isProgressRingActive;
            }
            set
            {
                _isProgressRingActive = value;
                NotifyOfPropertyChange(nameof(IsProgressRingActive));
            }
        }
        #endregion

        public MainViewModel(SimpleContainer container)
        {
            _container = container;
            DisplayHomeView();
        }

        public async void DisplayHomeView()
        {
            IsProgressRingActive = true;
            HomeViewModel homeViewModel = _container.GetInstance<HomeViewModel>();
            HomeViewModel.ActiveHomeViewModelId = homeViewModel.GetHashCode();
            await ActivateItemAsync(homeViewModel);
            IsProgressRingActive = false;
        }
    }
}
