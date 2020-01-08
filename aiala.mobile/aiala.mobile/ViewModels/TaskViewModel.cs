using System.Reactive.Linq;
using System.Threading.Tasks;
using aiala.mobile.Models;
using Redux;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.ViewModels;
using xappido.Mobile.State;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using aiala.mobile.Actions;
using aiala.mobile.Activities;
using aiala.mobile.Activities.Steps;
using aiala.mobile.Activities.Tasks;

namespace aiala.mobile.ViewModels
{
    public class TaskViewModel : ViewModelBase, INavigatableViewModel, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;
        protected readonly IStore<AuthState> _authStore;
        private readonly IStore<SystemAppState> _systemStore;
        private DayStepFeedback _dayStepFeedback;

        public TaskViewModel(IStore<ApplicationState> appStore, IStore<AuthState> authStore, IStore<SystemAppState> systemStore)
        {
            _appStore = appStore;
            _authStore = authStore;
            _systemStore = systemStore;

            _dayStepFeedback = new DayStepFeedback();

            _appStore
                .DistinctUntilChanged(state => new { state.UpcomingTasks })
                .Subscribe(state =>
                {
                    this.UpcomingTask = state.UpcomingTasks
                        .Where(q => !q.IsCurrentTask() && q.State == DayTaskState.Undone)
                        .OrderBy(o => o.Start).FirstOrDefault();
                });

            _appStore
                .DistinctUntilChanged(state => new { state.Selected })
                .Subscribe(state =>
                {
                    this.CurrentTask = state.Selected?.Task;
                    this.HasCurrentTask = this.CurrentTask != null;

                    var steps = this.CurrentTask?.Steps?.OrderBy(o => o.Order);

                    if (steps != null && steps.Any())
                    {
                        this.Steps = new ObservableCollection<Step>(steps)
                        {
                            _dayStepFeedback
                        };
                    }
                    else
                    {
                        this.Steps = new ObservableCollection<Step>();
                    }

                    Step currentStep = this.Steps.CurrentStep();

                    if (currentStep == null)
                        currentStep = _dayStepFeedback;

                    this.CurrentStep = currentStep;
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
                RaisePropertyChanged(() => StepIndexText);
            }
        }
        private DayTask _currentTask;

        public ObservableCollection<Step> Steps
        {
            get
            {
                return _steps;
            }
            set
            {
                _steps = value;
                //_currentStep = null;

                RaisePropertyChanged(() => Steps);
                RaisePropertyChanged(() => StepIndexText);
                UpdateStepIndex();
            }
        }
        private ObservableCollection<Step> _steps;

        public bool HasCurrentTask
        {
            get
            {
                return _hasCurrentTask;
            }
            set
            {
                _hasCurrentTask = value;
                RaisePropertyChanged(() => HasCurrentTask);
            }
        }
        private bool _hasCurrentTask;

        public DayTask UpcomingTask
        {
            get
            {
                return _upcomingTask;
            }
            set
            {
                _upcomingTask = value;
                RaisePropertyChanged(() => UpcomingTask);
            }
        }
        private DayTask _upcomingTask;

        public int StepIndex
        {
            get
            {
                return _stepIndex;
            }
            set
            {
                _stepIndex = value;
                RaisePropertyChanged(() => StepIndex);
                RaisePropertyChanged(() => IsLastStep);
                RaisePropertyChanged(() => IsFirstStep);
            }
        }
        private int _stepIndex;

        public bool IsLastStep
        {
            get
            {
                return StepIndex >= this.Steps.Count - 1;
            }
        }

        public bool IsFirstStep
        {
            get
            {
                return StepIndex == 0;
            }
        }

        public string StepIndexText
        {
            get
            {
                return $"{StepIndex + 1} / {this.Steps.Count}";
            }
        }

        public Step CurrentStep
        {
            get
            {
                return _currentStep;
            }
            set
            {
                _currentStep = value;

                RaisePropertyChanged(() => CurrentStep);
                RaisePropertyChanged(() => StepIndexText);
                UpdateStepIndex();
            }
        }

        private Step _currentStep;

        public Task NavigateTo(object navigationParameter)
        {
            return Task.CompletedTask;
        }

        private void UpdateStepIndex()
        {
            StepIndex = Steps?.IndexOf(CurrentStep) ?? -1;
        }

        public Command NavigateCommand => new Command<bool>((navigateForward) =>
        {
            var currentIndex = StepIndex;
            if (Steps == null
                || CurrentStep == null
                || currentIndex == -1
                || (navigateForward && currentIndex >= Steps.Count - 1)
                || (!navigateForward && currentIndex <= 0))
            {
                return;
            }

            var previous = Steps[currentIndex];
            if (navigateForward)
            {
                var current = Steps[currentIndex + 1];

                // finish previous step
                if (previous is DayStep dayStep)
                {
                    _appStore.Dispatch(new TaskStepUpdateAction(this.CurrentTask.Id, dayStep.Id, DayStepState.Done));
                    _appStore.Dispatch(new ReportActivityAction(ActivityPriority.High, new StepStateActivity(DayStepState.Done, dayStep.Id, DateTimeOffset.UtcNow)));

                    Console.WriteLine($"set step as done | {dayStep.Text}");
                }

                // finish task
                if (current is DayStepFeedback feedbackStep)
                {
                    _appStore.Dispatch(new TaskStateUpdateAction(this.CurrentTask.Id, DayTaskState.Done, DayTaskFeedback.None));
                    RaisePropertyChanged(() => CurrentTask);

                    Console.WriteLine($"set task as done |  {this.CurrentTask.Name}");
                }
            }
            else
            {
                var current = Steps[currentIndex - 1];

                // open previous step
                if (previous is DayStep dayStep)
                {
                    _appStore.Dispatch(new TaskStepUpdateAction(this.CurrentTask.Id, dayStep.Id, DayStepState.Undone));
                    _appStore.Dispatch(new ReportActivityAction(ActivityPriority.High, new StepStateActivity(DayStepState.Undone, dayStep.Id, DateTimeOffset.UtcNow)));

                    Console.WriteLine($"set step as undone | {dayStep.Text}");
                }

                // unfinish task
                else if (previous is DayStepFeedback feedbackStep)
                {
                    _appStore.Dispatch(new TaskStateUpdateAction(this.CurrentTask.Id, DayTaskState.Undone, DayTaskFeedback.None));
                    RaisePropertyChanged(() => CurrentTask);

                    Console.WriteLine($"set task as undone | {this.CurrentTask.Name}");
                }

                // open current step
                if (current is DayStep currentDayStep)
                {
                    if (currentDayStep.State == DayStepState.Undone)
                        return;

                    _appStore.Dispatch(new TaskStepUpdateAction(this.CurrentTask.Id, currentDayStep.Id, DayStepState.Undone));
                    _appStore.Dispatch(new ReportActivityAction(ActivityPriority.High, new StepStateActivity(DayStepState.Undone, currentDayStep.Id, DateTimeOffset.UtcNow)));

                    Console.WriteLine($"set step as undone | {currentDayStep.Text}");
                }
            }
        });
        
        public Command FeedbackCommand => new Command<DayTaskFeedback>((feedback) =>
        {
            if (this.CurrentTask.State == DayTaskState.Done)
            {
                if (this.CurrentTask != null)
                {
                    var currentTaskId = this.CurrentTask.Id;
                    var currentTaskName = this.CurrentTask.Name;

                    // set feedback to done task
                    _appStore.Dispatch(new TaskStateUpdateAction(currentTaskId, DayTaskState.Done, feedback));
                    _appStore.Dispatch(new ReportActivityAction(ActivityPriority.High, new TaskFeedbackActivity(feedback, currentTaskId, DateTimeOffset.UtcNow)));

                    Console.WriteLine($"set feedback to {feedback} for done task | {currentTaskName}");
                }
            }
            else
            {
                Console.WriteLine($"set feedback not applied due undone state for task | {this.CurrentTask.Name}");
            }
        });

        public Command EmergencyCommand => new Command(() => _appStore.Dispatch(new NavigateAction(NavigationState.Emergency)));
    }
}
