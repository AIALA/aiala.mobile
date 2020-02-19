using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using aiala.mobile.BackgroundServices;
using aiala.mobile.iOS.Services;
using PanCardView.iOS;
using UIKit;
using Xamarin.Forms;

namespace aiala.mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private TaskRunnerService taskRunnerService;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // initialize plugins
            ImageCircleRenderer.Init();

            DependencyService.Register<SFAuthenticationSessionBrowser>();

            MessagingCenter.Subscribe<StartTaskRunnerMessage>(this, nameof(StartTaskRunnerMessage), async message => {
                if(taskRunnerService == null)
                    taskRunnerService = new TaskRunnerService();

                if(!taskRunnerService.IsStarted)
                    await taskRunnerService.Start();
            });

            MessagingCenter.Subscribe<StopTaskRunnerMessage>(this, nameof(StopTaskRunnerMessage), message => {
                if (taskRunnerService != null)
                    taskRunnerService.Stop();
            });

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
