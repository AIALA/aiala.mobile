using aiala.mobile.Converters;
using aiala.mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aiala.mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchedulerView : ContentView
	{
        public static readonly BindableProperty DayScheduleProperty =
              BindableProperty.Create("DaySchedule", typeof(object), typeof(DaySchedule), null, propertyChanged: OnDaySchedulePropertyChanged);

        public static readonly BindableProperty ScheduledTasksProperty =
              BindableProperty.Create("ScheduledTasks", typeof(object), typeof(List<DayTask>), null, propertyChanged: OnTasksPropertyChanged);

        public DaySchedule DaySchedule
        {
            get { return GetValue(DayScheduleProperty) as DaySchedule; }
            set { SetValue(DayScheduleProperty, value); }
        }

        private static void OnDaySchedulePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
            {
                if (bindable is SchedulerView schedulerView)
                {
                    var tasks = (newValue as DaySchedule)?.Tasks ?? new List<DayTask>();
                    schedulerView.ApplySchedule(tasks);
                }
            }
        }

        public List<DayTask> ScheduledTasks
        {
            get { return GetValue(ScheduledTasksProperty) as List<DayTask>; }
            set { SetValue(ScheduledTasksProperty, value); }
        }

        private static void OnTasksPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SchedulerView schedulerView)
            {
                schedulerView.ApplySchedule(newValue as List<DayTask>);
            }
        }

        private static TimeSpan PastTimeSpan = new TimeSpan(1, 0, 0);
        private static TimeSpan FutureTimeSpan = new TimeSpan(2, 0, 0);
        private static TimeSpan TotalTimeSpan = new TimeSpan(3, 0, 0);

        public SchedulerView()
        {
            InitializeComponent();

            //this.SetBinding(SchedulerView.DayScheduleProperty, "CurrentSchedule");
            this.SetBinding(SchedulerView.ScheduledTasksProperty, "Tasks");
        }
        

        private void ApplySchedule(List<DayTask> tasks)
        {
            var removees = this.scheduleLayout.Children.Where(q => q != this.backgroundBox).ToList();
            foreach (var child in removees)
            {
                this.scheduleLayout.Children.Remove(child);
            }

            if (tasks == null || !tasks.Any())
                return;

            foreach(var task in tasks)
            {
                if(task.End.Before(DateTimeHelper.Now.TimeOfDay.Subtract(PastTimeSpan))
                    || task.Start.After(DateTimeHelper.Now.TimeOfDay.Add(FutureTimeSpan)))
                {
                    // too late or too early
                    continue;
                }


                var box = CreateItem(task);

                // update color
                if (task.State == DayTaskState.Done)
                {
                    box.BackgroundColor = (Color)Application.Current.Resources["taskStateDone"];
                }
                else if (task.IsOverdue())
                {
                    box.BackgroundColor = (Color)Application.Current.Resources["taskStateDelay"];
                }
                else
                {
                    box.BackgroundColor = (Color)Application.Current.Resources["taskStateUndone"];
                }

                this.scheduleLayout.Children.Add(box,
                    xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        var offset = DateTimeHelper.Now.TimeOfDay.Subtract(PastTimeSpan);
                        var start = task.Start.Subtract(offset).TotalMinutes;

                        var result = Math.Max(0, (parent.Width * (start / TotalTimeSpan.TotalMinutes)));
                        return result;
                    }),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) =>
                    {

                        var maxEnd = task.End.After(DateTimeHelper.Now.TimeOfDay.Add(FutureTimeSpan))
                        ? DateTimeHelper.Now.TimeOfDay.Add(FutureTimeSpan)
                        : task.End;

                        var minStart = task.Start.Before(DateTimeHelper.Now.TimeOfDay.Subtract(PastTimeSpan))
                        ? DateTimeHelper.Now.TimeOfDay.Subtract(PastTimeSpan)
                        : task.Start;

                        var duration = maxEnd.Subtract(minStart).TotalMinutes;
                        
                        var result = ((parent.Width) / TotalTimeSpan.TotalMinutes) * duration;
                        return result;
                    }),
                    heightConstraint: Constraint.Constant(14));
            }
        }

        private Frame CreateItem(DayTask task)
        {
            return new Frame
            {
                CornerRadius = 0,
                HeightRequest = 14,
                BorderColor = Color.WhiteSmoke,
                HasShadow = false
            };
        }
    }
}