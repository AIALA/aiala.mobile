using System.Threading.Tasks;
using Redux;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.ViewModels;
using xappido.Mobile.State;
using System;
using aiala.mobile.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Xamarin.Forms;
using aiala.mobile.Actions;
using System.Linq;

namespace aiala.mobile.ViewModels
{
    public class HomeViewModel : ViewModelBase, INavigatableViewModel, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;
        protected readonly IStore<AuthState> _authStore;
        private readonly IStore<SystemAppState> _systemStore;

        public HomeViewModel(IStore<ApplicationState> appStore, IStore<AuthState> authStore, IStore<SystemAppState> systemStore)
        {
            _appStore = appStore;
            _authStore = authStore;
            _systemStore = systemStore;

            _appStore
                .DistinctUntilChanged(state => new { state.IsHttpRequestProcessing })
                .Subscribe(state =>
                {
                    System.Diagnostics.Debug.WriteLine($"Http Request in progress: {state.IsHttpRequestProcessing}");
                });

            _appStore
                .DistinctUntilChanged(state => new { state.User })
                .Subscribe(state =>
                {
                    this.Fullname = $"{state.User?.Firstname} {state.User?.Lastname}";
                });

            //_appStore
            //    .DistinctUntilChanged(state => new { state.Schedule, state.TaskFilter })
            //    .Subscribe(state =>
            //    {
            //        this.Filter = state.TaskFilter;
            //        this.CurrentSchedule = state.Schedule.GetCurrentSchedule();
            //        this.UpcomingTasks = this.CurrentSchedule.GetFilteredTasks(state.TaskFilter);
            //        this.CurrentTask = this.CurrentSchedule.GetCurrentTask();
            //    });

            _appStore
                .DistinctUntilChanged(state => new { state.IsScheduleLoading })
                .Subscribe(state =>
                {
                    this.IsScheduleLoading = state.IsScheduleLoading;
                });

            _appStore
                .DistinctUntilChanged(state => new { state.Selected })
                .Subscribe(state =>
                {
                    this.CurrentSchedule = state.Selected?.Schedule;
                    this.CurrentTask = state.Selected?.Task;
                });

            _appStore
                .DistinctUntilChanged(state => new { state.UpcomingTasks })
                .Subscribe(state =>
                {
                    this.UpcomingTasks = state.UpcomingTasks;
                });

            _appStore
                .DistinctUntilChanged(state => new { state.Schedule, state.TaskFilter })
                .Subscribe(state =>
                {
                    this.Filter = state.TaskFilter;
                });
        }

        public Task NavigateTo(object navigationParameter)
        {
            return Task.CompletedTask;
        }

        public string Fullname
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                RaisePropertyChanged(() => Fullname);
            }
        }
        private string _fullname;


        public ObservableCollection<DayTask> UpcomingTasks
        {
            get
            {
                return _upcomingTasks;
            }
            set
            {
                _upcomingTasks = value;
                RaisePropertyChanged(() => UpcomingTasks);
                RaisePropertyChanged(() => TaskListHeight);
            }
        }
        private ObservableCollection<DayTask> _upcomingTasks;

        public int TaskListHeight
        {
            get
            {
                if (UpcomingTasks == null)
                    return 100;

                return (UpcomingTasks.Count * 100) + 50;
            }
            set
            {
                return;
            }
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

        
        public DaySchedule CurrentSchedule
        {
            get
            {
                return _currentSchedule;
            }
            set
            {
                _currentSchedule = value;
                RaisePropertyChanged(() => CurrentSchedule);
                RaisePropertyChanged(() => HasCurrentScheduleTasks);
            }
        }
        private DaySchedule _currentSchedule;

        public bool HasCurrentScheduleTasks
        {
            get
            {
                var result = CurrentSchedule?.Tasks?.Any() ?? false;
                return result;
            }
            set
            {
                return;
            }
        }

        public TaskFilter Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                RaisePropertyChanged(() => Filter);
            }
        }
        private TaskFilter _filter;

        public bool IsScheduleLoading
        {
            get
            {
                return _isScheduleLoading;
            }
            set
            {
                _isScheduleLoading = value;
                RaisePropertyChanged(() => IsScheduleLoading);
            }
        }
        private bool _isScheduleLoading;

        public Command ToggleFilterCommand => new Command(() => _appStore.Dispatch(new TaskFilterAction(this.Filter == TaskFilter.All ? TaskFilter.Upcoming : TaskFilter.All)));

        public Command EmergencyCommand => new Command(() => _appStore.Dispatch(new NavigateAction(NavigationState.Emergency)));

        public Command NavigateToTaskCommand => new Command(() => _appStore.Dispatch(new NavigateAction(NavigationState.Task)));

        public Command RefreshCommand => new Command(() => {
            _appStore.Dispatch(new LoadScheduleAction(DateTime.Today, DateTime.Today.AddDays(3)));
        });
    }
}
