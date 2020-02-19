using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using aiala.mobile.Configuration;
using aiala.mobile.Actions;
using aiala.mobile.Effects;
using xappido.Mobile;
using xappido.Mobile.Core.Redux;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Actions;
using xappido.Mobile.Auth.Actions;
using aiala.mobile.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using aiala.mobile.BackgroundServices;
using Redux;
using aiala.mobile.Extensions;
using aiala.mobile.Activities;
using System.Reactive.Linq;
using System;
using aiala.mobile.Activities.Steps;
using aiala.mobile.Activities.Generic;
using aiala.mobile.Activities.Tasks;
using aiala.mobile.Activities.Emergencies;
using Autofac;
using aiala.mobile.Storage;
using System.Globalization;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace aiala.mobile
{
    public partial class App : Application
    {
        private IAppContext _appContext;
        private IStore<ApplicationState> _appStore;
        private IDisposable _taskEvaluationInterval;

        public App()
        {
            InitializeComponent();

            // set date time
            //var dueDate = new System.DateTime(2019, 1, 1, 08, 50, 0);
            DateTimeHelper.Init(null);

            var appContainer = AppContainer
                .Instance();

            appContainer
                // base registrations
                .UseXappidoMobile<ApplicationContext, ApplicationNavigationPageConnector, ApplicationSettingsService>()
                .UseAuthentication<ApplicationSettingsService>("aiala.mobile.AuthStorageKey")

                // application registration
                .RegisterStore<ApplicationStore, ApplicationState>(new ApplicationStore(ApplicationReducer.Execute, new ApplicationState(), d => ReduxMiddleware.EffectsMiddleware(d, appContainer.ResolveEffects)))
                .RegisterViewModel<LoginViewModel>()

                .RegisterEffect<LoadUserInfoEffect, LoadUserInfoAction>()
                .RegisterEffect<LoadScheduleEffect, LoadScheduleAction>()
                .RegisterEffect<LoadSettingsEffect, LoadSettingsAction>()

                .RegisterEffect<ScheduleWatcherEffect, SystemIsReadyAction>()
                .RegisterEffect<LongRunningWatcherEffect, SystemIsReadyAction>()
                .RegisterEffect<ReportActivityEffect, ReportActivityAction>()
                .RegisterEffect<StopTaskRunnerEffect, LogoutSuccessAction>()
                .RegisterEffect<EvaluateTaskEffect, EvaluateTaskAction>()

                .RegisterEffect<SystemReadyEffect, SystemIsReadyAction>()
                .RegisterEffect<ResetLocalDataEffect, LogoutSuccessAction>()
                .RegisterEffect<StartApplicationEffect, LoadUserInfoSuccessAction>()
                .RegisterEffect<NavigationEffect, NavigateAction>()

                .RegisterEffect<TaskOffsetEffect, TaskOffsetAction>()
                .RegisterEffect<TaskStateUpdateEffect, TaskStateUpdateAction>()
                .RegisterEffect<TaskStepUpdateEffect, TaskStepUpdateAction>()

                .RegisterEffect<LoadGalleryEffect, LoadGalleryAction>()
                .RegisterEffect<LoadGallerySuccessEffect, LoadGallerySuccessAction>()
                .RegisterEffect<UploadPictureEffect, UploadPictureAction>()
                .RegisterEffect<UploadPictureSuccessEffect, UploadPictureSuccessAction>()
                .RegisterEffect<DeletePictureEffect, DeletePictureAction>()

                .RegisterViewModel<HomeViewModel>(asSingleInstance: true)
                .RegisterViewModel<TaskViewModel>(asSingleInstance: true)
                .RegisterViewModel<MapViewModel>(asSingleInstance: true)
                .RegisterViewModel<PictureViewModel>(asSingleInstance: true)
                .RegisterViewModel<PictureDetailViewModel>()
                .RegisterViewModel<EmergencyViewModel>()
                .RegisterViewModel<SettingsViewModel>(asSingleInstance: true)

                .RegisterViewModel<NavigationButtonBarViewModel>(asSingleInstance: true)
                .RegisterViewModel<SchedulerViewModel>(asSingleInstance: true)
                .RegisterViewModel<TaskOverdueViewModel>(asSingleInstance: true)

                .RegisterViewModel<MasterViewModel>(asSingleInstance: true)
                .RegisterViewModel<MasterDetailViewModel>(asSingleInstance: true)

                .RegisterType<AppLoggerService>()
                .RegisterType<ActivityQueueWorker>()
                .RegisterType<ActivityHandlerService>()

                .RegisterActivityHandler<StepActivityHandler>()
                .RegisterActivityHandler<GenericActivityHandler>()
                .RegisterActivityHandler<TaskActivityHandler>()
                .RegisterActivityHandler<EmergencyActivityHandler>()

                .RegisterType<PictureGalleryDatabase>(asSingleInstance: true)

                .Build();

            _appContext = AppContainer
                .Instance()
                .Resolve<IAppContext>();

            _appStore = AppContainer.Instance().Resolve<IStore<ApplicationState>>();
        }

        protected override async void OnStart()
        {
            var settingsService = AppContainer.Instance().Resolve<IApplicationSettingsService>();
            if (settingsService.Culture != null)
            {
                var culture = new CultureInfo(settingsService.Culture);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            // Handle when your app starts
            Microsoft.AppCenter.AppCenter.Start("android=f9cce2c1-ffe7-446e-a827-0ff7fe787258;ios=14c2eed6-49b1-4684-afe9-d8ab297551d8;",
                typeof(Analytics), typeof(Crashes));

            MessagingCenter.Subscribe<TaskRunnerStartedMessage>(this, nameof(TaskRunnerStartedMessage), msg => OnTaskRunnerStarted());
            MessagingCenter.Subscribe<TaskRunnerCancelledMessage>(this, nameof(TaskRunnerCancelledMessage), msg => OnTaskRunnerCancelled());

            MessagingCenter.Send(new StartTaskRunnerMessage(), nameof(StartTaskRunnerMessage));

            await _appContext.Start();

            StartTaskEvaluationInterval();
        }

        private void StartTaskEvaluationInterval()
        {
            if (_taskEvaluationInterval != null)
                return;

            _taskEvaluationInterval = Observable.Interval(TimeSpan.FromSeconds(30))
                .Subscribe(RaiseTaskEvaluation);
        }

        private void StopTaskEvaluationInterval()
        {
            if (_taskEvaluationInterval != null)
            {
                _taskEvaluationInterval.Dispose();
                _taskEvaluationInterval = null;
            }
        }

        private void RaiseTaskEvaluation(long observer)
        {
            _appStore.Dispatch(new EvaluateTaskAction());
        }

        protected override async void OnSleep()
        {
            // Handle when your app sleeps
            await _appContext.Sleep();

            var message = new StopTaskRunnerMessage();
            MessagingCenter.Send(message, nameof(StopTaskRunnerMessage));

            StopTaskEvaluationInterval();
        }

        protected override async void OnResume()
        {
            //var newDueDate = DateTimeHelper.Now.AddMinutes(5);
            //DateTimeHelper.Init(newDueDate);

            // Handle when your app resumes
            await _appContext.Resume();

            var message = new StartTaskRunnerMessage();
            MessagingCenter.Send(message, nameof(StartTaskRunnerMessage));

            StartTaskEvaluationInterval();
        }

        private void OnTaskRunnerCancelled()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                _appStore.Dispatch(new BackgroundProcessorStateAction(false));
                System.Diagnostics.Debug.WriteLine("Background processor cancelled");
            });
        }

        private void OnTaskRunnerStarted()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                _appStore.Dispatch(new BackgroundProcessorStateAction(true));
                System.Diagnostics.Debug.WriteLine("Background processor started");
            });
        }
    }
}
