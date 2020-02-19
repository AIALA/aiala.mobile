using System.Reactive.Linq;
using System.Threading.Tasks;
using aiala.mobile.Models;
using Redux;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using aiala.mobile.Actions;
using aiala.mobile.Activities;
using System.Collections.Generic;
using aiala.mobile.Activities.Generic;
using aiala.mobile.Activities.Emergencies;

namespace aiala.mobile.ViewModels
{
    public class EmergencyViewModel : ViewModelBase, INavigatableViewModel, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;
        private EmergencyInformation _emergencyInformation;
        private Guid _emergencyId;
        private EmergencyState _emergencyState;

        public EmergencyViewModel(IStore<ApplicationState> appStore)
        {
            _appStore = appStore;

            _appStore
               .DistinctUntilChanged(state => new { state.Selected, state.EmergencyContacts })
               .Subscribe(state =>
               {
                   this.CurrentTask = state.Selected?.Task;

                   if (this.CurrentTask?.EmergencyContacts?.Any() ?? false)
                   {
                       this.Config1Contact = this.CurrentTask?.EmergencyContacts?.Skip(0).FirstOrDefault();
                       this.Config2Contact = this.CurrentTask?.EmergencyContacts?.Skip(1).FirstOrDefault();
                   }
                   else
                   {
                       this.Config1Contact = state.EmergencyContacts?.Skip(0).FirstOrDefault();
                       this.Config2Contact = state.EmergencyContacts?.Skip(1).FirstOrDefault();
                   }
               });

            _appStore
                .DistinctUntilChanged(state => new { state.EmergencyInformation })
                .Subscribe(state =>
                {
                    this.EmergencyInformation = state.EmergencyInformation;
                });

            this.EmergencyState = EmergencyState.Bad;
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


        public EmergencyContact Config1Contact
        {
            get
            {
                return _config1Contact;
            }
            set
            {
                _config1Contact = value;
                RaisePropertyChanged(() => Config1Contact);
            }
        }
        private EmergencyContact _config1Contact;

        public EmergencyContact Config2Contact
        {
            get
            {
                return _config2Contact;
            }
            set
            {
                _config2Contact = value;
                RaisePropertyChanged(() => Config2Contact);
            }
        }
        private EmergencyContact _config2Contact;

        public EmergencyState EmergencyState
        {
            get
            {
                return _emergencyState;
            }
            set
            {
                _emergencyState = value;
                RaisePropertyChanged(() => EmergencyState);
            }
        }

        public EmergencyInformation EmergencyInformation
        {
            get
            {
                return _emergencyInformation;
            }
            set
            {
                _emergencyInformation = value;
                RaisePropertyChanged(() => EmergencyInformation);
            }
        }

        public Command<EmergencyState> EmergencyStateCommand => new Command<EmergencyState>((emergencyState) =>
        {
            this.EmergencyState = emergencyState;
            _appStore.Dispatch(new ReportActivityAction(ActivityPriority.High, new EmergencyMoodActivity(emergencyState, this._emergencyId, DateTimeOffset.UtcNow), true));
        });

        public Command EmergencyStopCommand => new Command(() =>
        {
            _appStore.Dispatch(new ReportActivityAction(ActivityPriority.High, new EndEmergencyActivity(_emergencyId, DateTimeOffset.UtcNow), true));
            _appStore.Dispatch(new NavigateAction(NavigationState.Home));
        });

        public Command<string> EmergencyTextLinkCommand => new Command<string>((link) =>
        {
            Device.OpenUri(new Uri(link));

            var activity = new GenericActivity(GenericActivityType.EmergencyHelpTextLink, DateTimeOffset.UtcNow)
            {
                ActivityData = new Dictionary<string, string>
                {
                    ["Link"] = link
                },
                ActiveTaskId = this.CurrentTask?.Id,
                ActiveStepId = this.CurrentTask?.Steps.CurrentStep()?.Id
            };

            _appStore.Dispatch(new ReportActivityAction(ActivityPriority.Low, activity));
        });

        public Task NavigateTo(object navigationParameter)
        {
            this._emergencyId = Guid.NewGuid();
            _appStore.Dispatch(new ReportActivityAction(ActivityPriority.High, new StartEmergencyActivity(_emergencyId, DateTimeOffset.UtcNow), true));
            return Task.CompletedTask;
        }
    }
}
