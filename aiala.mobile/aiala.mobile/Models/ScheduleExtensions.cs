using System;
using System.Linq;
using System.Collections.Generic;
using aiala.mobile.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace aiala.mobile.Models
{
    public static class ScheduleExtensions
    {
        public static DaySchedule GetCurrentSchedule(this IEnumerable<DaySchedule> schedules, DateTime? dueDate = null)
        {
            if (dueDate == null)
                dueDate = DateTimeHelper.Today;

            var emptySchedule = new DaySchedule() { Tasks = new List<DayTask>() };

            if (schedules == null || !schedules.Any())
                return emptySchedule;

            var current = schedules
                .FirstOrDefault(q => q.Date.Date.Equals(dueDate.Value.Date));

            if (current == null)
                return emptySchedule;

            return current;
        }

        public static DayTask GetCurrentTask(this DaySchedule schedule, DateTime? dueDate = null)
        {
            if (dueDate == null)
                dueDate = DateTimeHelper.Now;

            if (schedule == null || !schedule.Tasks.Any())
                return null;

            var currents = schedule.Tasks
                .Where(q => q.IsCurrentTask(dueDate))
                .OrderBy(o => o.Start)
                .ToList();

            var current = currents.FirstOrDefault();

            if (current == null)
                return null;

            return current;
        }

        public static ObservableCollection<DayTask> GetUpcomingTasks(this DaySchedule schedule, DateTime? dueTime = null, bool withoutDone = true)
        {
            TimeSpan currentTime = DateTimeHelper.Now.TimeOfDay;

            if (dueTime.HasValue)
                currentTime = dueTime.Value.TimeOfDay;

            if (schedule.Tasks == null || !schedule.Tasks.Any())
                return new ObservableCollection<DayTask>(new List<DayTask>());

            var upcomingTasks = schedule.Tasks
                .Where(q => q.End >= currentTime)
                .Where(q => withoutDone == false || q.State != DayTaskState.Done);

            return new ObservableCollection<DayTask>(upcomingTasks);
        }

        public static ObservableCollection<DayTask> GetFilteredTasks(this DaySchedule schedule, TaskFilter filter, DateTime? dueTime = null)
        {
            if (filter == TaskFilter.All)
            {
                var tasks = schedule?.Tasks ?? new List<DayTask>();
                return new ObservableCollection<DayTask>(tasks);
            }

            if (filter == TaskFilter.Upcoming)
            {
                var result = new ObservableCollection<DayTask>();

                var currentTask = GetCurrentTask(schedule, dueTime);
                if (currentTask != null)
                {
                    result.Add(currentTask);
                    var upcoming = schedule.Tasks.SkipWhile(t => t != currentTask)
                        .Skip(1)
                        .FirstOrDefault();
                    if (upcoming != null)
                    {
                        result.Add(upcoming);
                    }
                }
                else
                {
                    var upcomingTasks = GetUpcomingTasks(schedule, dueTime, withoutDone: false)
                        .Where(q => q != currentTask)
                        .Take(2);

                    foreach (var upcomingTask in upcomingTasks)
                    {
                        result.Add(upcomingTask);
                    }
                }
                

                return result;
            }

            return new ObservableCollection<DayTask>();
        }
    }
}
