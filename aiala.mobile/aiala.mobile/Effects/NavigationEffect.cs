using aiala.mobile.Actions;
using aiala.mobile.Activities;
using aiala.mobile.Activities.Generic;
using aiala.mobile.ViewModels;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class NavigationEffect : Effect<NavigateAction>
    {
        private readonly IStore<ApplicationState> _appStore;
        private readonly INavigationService _navigationService;

        public NavigationEffect(IStore<ApplicationState> appStore, INavigationService navigationService, NavigateAction action) : base(action)
        {
            _appStore = appStore;
            _navigationService = navigationService;
        }

        public override Task OnExecute(NavigateAction action)
        {
            _appStore.Dispatch(new ReportActivityAction(ActivityPriority.Low, new GenericActivity(GenericActivityType.AppPageNavigation, DateTimeOffset.UtcNow)
            {
                ActivityData = new Dictionary<string, string>
                {
                    ["Page"] = action.NavigationState.ToString()
                }
            }));

            switch(action.NavigationState)
            {
                case Models.NavigationState.Home:
                    return _navigationService.NavigateToAsync<HomeViewModel>();

                case Models.NavigationState.Task:
                    return _navigationService.NavigateToAsync<TaskViewModel>();

                case Models.NavigationState.Map:
                    return _navigationService.NavigateToAsync<MapViewModel>();

                case Models.NavigationState.Picture:
                    return _navigationService.NavigateToAsync<PictureViewModel>();

                case Models.NavigationState.Emergency:
                    return _navigationService.NavigateToAsync<EmergencyViewModel>();

                case Models.NavigationState.Settings:
                    return _navigationService.NavigateToAsync<SettingsViewModel>(false);

                case Models.NavigationState.LoginSettings:
                    return _navigationService.NavigateToAsync<SettingsViewModel>(true);

                case Models.NavigationState.Login:
                    return _navigationService.NavigateToAsync<LoginViewModel>();

                default:
                    return _navigationService.NavigateToAsync<HomeViewModel>();
            }
        }
    }
}
