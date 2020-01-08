using System.Threading.Tasks;
using aiala.mobile.Models;
using Redux;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.ViewModels;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using aiala.mobile.Actions;
using aiala.mobile.Activities;
using aiala.mobile.Resources;
using System.Collections.Generic;
using aiala.mobile.Activities.Generic;
using aiala.mobile.Configuration;

namespace aiala.mobile.ViewModels
{
    public class MapViewModel : ViewModelBase, INavigatableViewModel, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;

        public MapViewModel(IStore<ApplicationState> appStore)
        {
            _appStore = appStore;

            _appStore
               .DistinctUntilChanged(state => new { state.Selected })
               .Subscribe(state =>
                {
                    this.CurrentTask = state.Selected?.Task;

                    if(this.CurrentTask?.Place != null)
                    {
                        this.TaskLocation = NavigationLocation.Map(this.CurrentTask.Place);
                    }
                    else if (!string.IsNullOrWhiteSpace(this.CurrentTask?.FreeFormPlace))
                    {
                        this.TaskLocation = new NavigationLocation
                        {
                            Name = this.CurrentTask?.FreeFormPlace,
                            Location = null
                        };
                    }
                    else
                    {
                        this.TaskLocation = new NavigationLocation
                        {
                            Name = UiTexts.Navigate_NoTaskLocation,
                            Location = null
                        };
                    }
               });

            _appStore
               .DistinctUntilChanged(state => new { state.Locations })
               .Subscribe(state =>
               {
                   this.Config1Location = state.Locations.Skip(0).FirstOrDefault();
                   this.Config2Location = state.Locations.Skip(1).FirstOrDefault();
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

        public NavigationLocation CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;
                RaisePropertyChanged(() => CurrentLocation);
            }
        }
        private NavigationLocation _currentLocation;

        public NavigationLocation TaskLocation
        {
            get
            {
                return _taskLocation;
            }
            set
            {
                _taskLocation = value;
                RaisePropertyChanged(() => TaskLocation);
            }
        }
        private NavigationLocation _taskLocation;

        public NavigationLocation Config1Location
        {
            get
            {
                return _config1Location;
            }
            set
            {
                _config1Location = value;
                RaisePropertyChanged(() => Config1Location);
            }
        }
        private NavigationLocation _config1Location;

        public NavigationLocation Config2Location
        {
            get
            {
                return _config2Location;
            }
            set
            {
                _config2Location = value;
                RaisePropertyChanged(() => Config2Location);
            }
        }
        private NavigationLocation _config2Location;

        public Task NavigateTo(object navigationParameter)
        {
            return Task.CompletedTask;
        }

        public Command NavigateCommand => new Command<NavigationLocation>(async (location) =>
        {
            if (location == null || location.Location == null)
                return;

            var currentStep = this.CurrentTask?.Steps?.CurrentStep();

            var activity = new GenericActivity(GenericActivityType.PlaceNavigation, DateTimeOffset.UtcNow)
            {
                ActivityData = new Dictionary<string, string>
                {
                    ["Name"] = location.Name,
                    ["Latitude"] = location.Location.Latitude.ToString(),
                    ["Longitude"] = location.Location.Longitude.ToString(),
                },
                ActiveTaskId = this.CurrentTask?.Id,
                ActiveStepId = currentStep?.Id
            };
            _appStore.Dispatch(new ReportActivityAction(ActivityPriority.Low, activity));
            
            var geolocation = new Location(location.Location.Latitude, location.Location.Longitude);
            var options = new MapLaunchOptions { Name = location.Name };

            await Map.OpenAsync(geolocation, options);
        });

        public Command NavigateCurrentCommand => new Command(async () =>
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var currentStep = this.CurrentTask?.Steps?.CurrentStep();
                    var navigationLocation = new NavigationLocation { Name = "Current location", Location = new NavigationLocation.Geolocation { Latitude = location.Latitude, Longitude = location.Longitude } };

                    var activity = new GenericActivity(GenericActivityType.PlaceNavigation, DateTimeOffset.UtcNow)
                    {
                        ActivityData = new Dictionary<string, string>
                        {
                            ["Name"] = "Current location",
                            ["Latitude"] = location.Latitude.ToString(),
                            ["Longitude"] = location.Longitude.ToString(),
                        },
                        ActiveTaskId = this.CurrentTask?.Id,
                        ActiveStepId = currentStep?.Id
                    };
                    _appStore.Dispatch(new ReportActivityAction(ActivityPriority.Low, activity));
                    
                    var geolocation = new Location(location.Latitude, location.Longitude);
                    var options = new MapLaunchOptions { Name = "Current location" };

                    await Map.OpenAsync(geolocation, options);
                }
                else
                {
                    this.CurrentLocation = null;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        });

        public Command EmergencyCommand => new Command(() => _appStore.Dispatch(new NavigateAction(NavigationState.Emergency)));
    }
}
    