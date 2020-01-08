using System;
using System.Collections.Generic;
using System.Linq;

namespace aiala.mobile.Models
{
    public static class DayTaskExtensions
    {
        public static bool IsOverdue(this DayTask dayTask, DateTime? dueTime = null)
        {
            TimeSpan currentTime = DateTimeHelper.Now.TimeOfDay;

            if (dueTime.HasValue)
                currentTime = dueTime.Value.TimeOfDay;

            if (dayTask == null)
                return false;

            var isAfter = currentTime.After(dayTask.End);
            var isUndone = dayTask.State == DayTaskState.Undone;

            var overdue = isAfter && isUndone;
            return overdue;
        }

        public static bool IsOffsetExpired(this DayTask dayTask, DateTime? dueTime = null)
        {
            var isOverdue = dayTask.IsOverdue(dueTime);

            if (!isOverdue)
                return false;
            
            TimeSpan currentTime = DateTimeHelper.Now.TimeOfDay;

            if (dueTime.HasValue)
                currentTime = dueTime.Value.TimeOfDay;

            var offsetEnd = dayTask.End.Add(dayTask.ExpirationOffset);

            var isAfter = currentTime.After(offsetEnd);
            var isUndone = dayTask.State == DayTaskState.Undone;

            var overdue = isAfter && isUndone;
            return overdue;
        }

        public static bool IsCurrentTask(this DayTask dayTask, DateTime? dueTime = null)
        {
            var isPast = dayTask.IsPastTask(dueTime);

            if (isPast)
                return false;

            TimeSpan currentTime = DateTimeHelper.Now.TimeOfDay;

            if (dueTime.HasValue)
                currentTime = dueTime.Value.TimeOfDay;

            if (dayTask == null)
                return false;

            var offsetEnd = dayTask.End.Add(dayTask.ExpirationOffset);

            var hasStarted = dayTask.Start.Before(currentTime);
            var isBetween = currentTime.Between(dayTask.Start, offsetEnd);
            var isUndone = dayTask.State == DayTaskState.Undone;
            var hasNoFeedback = dayTask.Feedback == DayTaskFeedback.None;

            // task is current when
            // - task is undone or has no feedback
            // - task is on time incl. offset or has started so far
            var isCurrent = (isUndone || hasNoFeedback) && (isBetween || hasStarted);
            return isCurrent;
        }

        public static bool IsPastTask(this DayTask dayTask, DateTime? dueTime = null)
        {
            TimeSpan currentTime = DateTimeHelper.Now.TimeOfDay;

            if (dueTime.HasValue)
                currentTime = dueTime.Value.TimeOfDay;

            if (dayTask == null)
                return false;

            var offsetEnd = dayTask.End.Add(dayTask.ExpirationOffset);

            var isBefore = offsetEnd.Before(currentTime);
            var isDone = dayTask.State == DayTaskState.Done;
            var hasFeedback = dayTask.Feedback != DayTaskFeedback.None;

            var isPast = isDone && hasFeedback && isBefore;
            return isPast;
        }

        public static DayStep CurrentStep(this IEnumerable<Step> steps)
        {
            if (steps == null || !steps.Any())
                return null;

            var currentStep = steps
                .OfType<DayStep>()
                .OrderBy(o => o.Order)
                .FirstOrDefault(q => q.State == DayStepState.Undone);

            return currentStep;
        }
    }
}
