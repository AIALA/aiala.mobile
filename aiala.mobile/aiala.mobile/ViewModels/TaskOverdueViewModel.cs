using aiala.mobile.Models;
using Redux;
using System.Reactive.Linq;
using Xamarin.Forms;
using aiala.mobile.Actions;
using xappido.Mobile.Core.ViewModels;
using System;

namespace aiala.mobile.ViewModels
{
    public class TaskOverdueViewModel : ViewModelBase, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;

        public TaskOverdueViewModel(IStore<ApplicationState> appStore)
        {
            _appStore = appStore;

            _appStore
                .DistinctUntilChanged(state => new { state.Selected })
                .Subscribe(state =>
                {
                    this.CurrentTask = state.Selected?.Task;
                });
        }

        public DayTask CurrentTask
        {
            get
            {
                return _currentTask;
            }
            set
            {
                _currentTask = value;
                RaisePropertyChanged(() => CurrentTask);
            }
        }
        private DayTask _currentTask;

        public Command AddTaskOffsetCommand => new Command(() => _appStore.Dispatch(new TaskOffsetAction(this.CurrentTask.Id)));
    }
}
