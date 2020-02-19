using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.Droid;
using PanCardView.Droid;
using Android.Content;
using aiala.mobile.Droid.Services;
using aiala.mobile.BackgroundServices;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.Permissions;

namespace aiala.mobile.Droid
{
    [Activity(
        Label = "AIALA",
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_round_launcher",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            DependencyService.Register<ChromeCustomTabsBrowser>();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // initialize plugins
            ImageCircleRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            // Task Runner Service
            // https://robgibbens.com/backgrounding-with-xamarin-forms/
            MessagingCenter.Subscribe<StartTaskRunnerMessage>(this, nameof(StartTaskRunnerMessage), message => {
                if (TaskRunnerService.IsStarted)
                    return;

                var intent = new Intent(this, typeof(TaskRunnerService));
                StartService(intent);

                TaskRunnerService.IsStarted = true;
            });

            MessagingCenter.Subscribe<StopTaskRunnerMessage>(this, nameof(StopTaskRunnerMessage), message => {
                var intent = new Intent(this, typeof(TaskRunnerService));
                StopService(intent);

                TaskRunnerService.IsStarted = false;
            });

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}