using aiala.mobile.Actions;
using aiala.mobile.Activities;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Extensions;
using Redux;
using xappido.Mobile.State.Actions;

namespace aiala.mobile
{
    public static class ApplicationReducer
    {
        public static ApplicationState Execute(ApplicationState state, IAction action)
        {
            if (action is LoadUserInfoAction loadUser)
            {
                state.IsHttpRequestProcessing = true;
            }

            if (action is LoadUserInfoSuccessAction loadUserSuccess)
            {
                state.IsHttpRequestProcessing = false;
                state.User = loadUserSuccess.Result;
            }

            if (action is LoadUserInfoFailedAction loadUserFailed)
            {
                state.IsHttpRequestProcessing = false;
            }

            if(action is LoadScheduleAction loadScheduleAction)
            {
                state.IsHttpRequestProcessing = true;
                state.IsScheduleLoading = true;
            }

            if(action is LoadScheduleSuccessAction loadScheduleSuccessAction)
            {
                state.Schedule = loadScheduleSuccessAction.Result;

                state.IsHttpRequestProcessing = false;
                state.IsScheduleLoading = false;
            }

            if(action is LoadScheduleFailedAction loadScheduleFailedAction)
            {
                state.IsHttpRequestProcessing = false;
                state.IsScheduleLoading = false;
            }

            if(action is TaskFilterAction taskFilterAction)
            {
                state.TaskFilter = taskFilterAction.TaskFilter;
            }

            if(action is NavigateAction navigateAction)
            {
                state.NavigationState = navigateAction.NavigationState;
            }

            if(action is CurrentScheduleUpdateAction curentScheduleUpdate)
            {
                state.Selected = new ApplicationStateSelected
                {
                    Schedule = curentScheduleUpdate.CurrentSchedule,
                    Task = state.Selected.Task,
                    Step = state.Selected.Step
                };
            }

            if(action is CurrentStateUpdateAction curentStateUpdate)
            {
                state.Selected = new ApplicationStateSelected
                {
                    Schedule = state.Selected.Schedule,
                    Task = curentStateUpdate.CurrentTask,
                    Step = state.Selected.Step
                };
                state.UpcomingTasks = curentStateUpdate.UpcomingTasks;
            }

            if(action is BackgroundProcessorStateAction backgroundProcessorStateAction)
            {
                state.IsBackgroundProcessorRunning = backgroundProcessorStateAction.IsRunning;
            }

            if(action is ReportActivityAction || action is ReportActivitySuccessAction)
            {
                state.PendingUpdates = App.Current.Properties.GetQueueItemsCount(ActivityQueues.All);
            }

            if (action is LoadSettingsAction loadSettings)
            {
                state.IsHttpRequestProcessing = true;
            }

            if (action is LoadSettingsSuccessAction loadSettingsSuccess)
            {
                state.IsHttpRequestProcessing = false;
                state.EmergencyContacts = loadSettingsSuccess.EmergencyContacts;
                state.Locations = loadSettingsSuccess.Locations;
                state.EmergencyInformation = loadSettingsSuccess.EmergencyInformation;
            }

            if (action is LoadSettingsFailedAction loadSettingsFailed)
            {
                state.IsHttpRequestProcessing = false;
            }

            if(action is TaskUpdateSuccessAction taskUpdateSuccessAction)
            {
                state.Selected = new ApplicationStateSelected
                {
                    Schedule = state.Selected.Schedule,
                    Task = taskUpdateSuccessAction.Task,
                    Step = state.Selected.Step
                };
            }

            if(action is UploadPictureAction uploadPictureAction)
            {
                state.IsPictureUploading = true;
                state.LatestPictureUploadReference = "";
            }

            if(action is UploadPictureFailedAction uploadPictureFailedAction)
            {
                state.IsPictureUploading = false;
                state.LatestPictureUploadReference = "";
            }

            if(action is UploadPictureSuccessAction uploadPictureSuccessAction)
            {
                state.IsPictureUploading = false;
                state.LatestPictureUpload = uploadPictureSuccessAction.Picture;

                state.LatestPictureUploadReference = uploadPictureSuccessAction.Reference;
            }

            if(action is LoadGalleryAction loadGalleryAction)
            {
                state.IsHttpRequestProcessing = true;
                state.IsGalleryLoading = true;
            }

            if(action is LoadGallerySuccessAction)
            {
                state.IsHttpRequestProcessing = false;
                state.IsGalleryLoading = false;
            }

            if (action is LoadGalleryFailedAction)
            {
                state.IsHttpRequestProcessing = false;
                state.IsGalleryLoading = false;
            }

            return state;
        }
    }
}
