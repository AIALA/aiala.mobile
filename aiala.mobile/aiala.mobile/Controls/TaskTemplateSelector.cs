using aiala.mobile.Models;
using aiala.mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace aiala.mobile.Controls
{
    class TaskTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        public TaskTemplateSelector()
        {
            // Retain instances!
            this.currentDataTemplate = new DataTemplate(typeof(CurrentTaskViewCell));
            this.upcomingDataTemplate = new DataTemplate(typeof(UpcomingTaskViewCell));
            this.pastDataTemplate = new DataTemplate(typeof(PastTaskViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(item is DayTask dayTask)
            {
                DataTemplate template = null;

                DayTask currentTask = null;
                if(container.BindingContext is ICurrentTaskViewModel currentTaskViewModel)
                {
                    currentTask = currentTaskViewModel.CurrentTask;
                }

                if (dayTask == currentTask)
                {
                    template = this.currentDataTemplate;
                }
                else if(dayTask.IsPastTask())
                {
                    template = this.pastDataTemplate;
                }
                else
                {
                    template = this.upcomingDataTemplate;
                }

                return template;
            }

            return null;
        }

        private readonly DataTemplate currentDataTemplate;
        private readonly DataTemplate upcomingDataTemplate;
        private readonly DataTemplate pastDataTemplate;
    }
}
