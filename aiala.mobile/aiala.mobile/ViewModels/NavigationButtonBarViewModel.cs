using aiala.mobile.Models;
using Redux;
using System.Reactive.Linq;
using Xamarin.Forms;
using aiala.mobile.Actions;
using xappido.Mobile.Core.ViewModels;
using System;

namespace aiala.mobile.ViewModels
{
    public class NavigationButtonBarViewModel : ViewModelBase
    {
        protected readonly IStore<ApplicationState> _appStore;

        public NavigationButtonBarViewModel(IStore<ApplicationState> appStore)
        {
            _appStore = appStore;

            _appStore
                .DistinctUntilChanged(state => new { state.NavigationState })
                .Subscribe(state =>
                {
                    this.NavigationState = state.NavigationState;
                });
        }

        public NavigationState NavigationState
        {
            get
            {
                return _navigationState;
            }
            set
            {
                _navigationState = value;
                RaisePropertyChanged(() => NavigationState);
            }
        }
        private NavigationState _navigationState;

        public Command<NavigationState> NavigateCommand => new Command<NavigationState>((state) => _appStore.Dispatch(new NavigateAction(state)));
    }
}
