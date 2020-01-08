using aiala.mobile.Models;
using Redux;
using System.Reactive.Linq;
using Xamarin.Forms;
using aiala.mobile.Actions;
using xappido.Mobile.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace aiala.mobile.ViewModels
{
    public class SchedulerViewModel : ViewModelBase
    {
        protected readonly IStore<ApplicationState> _appStore;

        public SchedulerViewModel(IStore<ApplicationState> appStore)
        {
            _appStore = appStore;

            _appStore
                .DistinctUntilChanged(state => new { state.Selected })
                .Subscribe(state =>
                {
                    var tasks = state.Selected?.Schedule?.Tasks ?? new List<DayTask>();
                    this.Tasks = new List<DayTask>(tasks);
                });
        }

        public List<DayTask> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                RaisePropertyChanged(() => Tasks);
            }
        }
        private List<DayTask> _tasks;

    }
}
