using Redux;
using xappido.Mobile.Core.ViewModels;
using System;

namespace aiala.mobile.ViewModels
{
    public class MasterDetailViewModel : ViewModelBase
    {
        protected readonly IStore<ApplicationState> _appStore;

        public MasterDetailViewModel(IStore<ApplicationState> appStore)
        {
            _appStore = appStore;
            _appStore.Subscribe(state =>
            {
                this.IsMasterPresented = state.IsMasterPresented;
            });
        }

        public bool IsMasterPresented
        {
            get
            {
                return _isMasterPresented;
            }
            set
            {
                _isMasterPresented = value;
                RaisePropertyChanged(() => IsMasterPresented);
            }
        }
        private bool _isMasterPresented;
    }
}
