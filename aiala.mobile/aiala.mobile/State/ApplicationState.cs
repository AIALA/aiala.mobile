using Autofac;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using aiala.mobile.Models;
using xappido.Mobile.Auth.Actions;
using xappido.Mobile.Auth.Effects;
using xappido.Mobile.Auth.Services;
using xappido.Mobile.Core.Redux;
using xappido.Mobile.State;
using xappido.Mobile.State.Actions;
using xappido.Mobile.State.Effects;
using System.Collections.ObjectModel;
using aiala.mobile.Extensions;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Activities;

namespace aiala.mobile
{

    public class ApplicationState : AppState
    {
        public ApplicationState()
        {
            TaskFilter = TaskFilter.Upcoming;
            NavigationState = NavigationState.Home;

            PendingUpdates = App.Current.Properties.GetQueueItemsCount(ActivityQueues.All);
            IsBackgroundProcessorRunning = false;

            Selected = new ApplicationStateSelected();

            EmergencyInformation = new EmergencyInformation { EmergencyTextBad = string.Empty, EmergencyTextImproving = string.Empty };
        }

        public bool IsMasterPresented { get; set; }

        public User User { get; set; }

        public TaskFilter TaskFilter { get; set; }

        public NavigationState NavigationState { get; set; }

        public List<DaySchedule> Schedule { get; set; }

        public ApplicationStateSelected Selected { get; set; }

        public ObservableCollection<DayTask> UpcomingTasks { get; set; }

        public int PendingUpdates { get; set; }

        public bool IsBackgroundProcessorRunning { get; set; }

        public List<EmergencyContact> EmergencyContacts { get; set; }

        public EmergencyInformation EmergencyInformation { get; set; }

        public List<NavigationLocation> Locations { get; set; }

        public bool IsScheduleLoading { get; set; }

        public bool IsPictureUploading { get; set; }

        public bool IsGalleryLoading { get; set; }

        public string LatestPictureUploadReference { get; set; }

        public Picture LatestPictureUpload { get; set; }
    }

    public class ApplicationStateSelected
    {
        public override bool Equals(object obj)
        {
            // TODO optimize
            return false;
        }

        public DaySchedule Schedule { get; set; }

        public DayTask Task { get; set; }

        public Step Step { get; set; }
    }
}
