using System;
using System.Collections.Generic;

namespace aiala.mobile.Models
{
    public class DayTask
    {
        public DayTask()
        {
            this.ExpirationOffset = TimeSpan.Zero;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan DefaultDuration { get; set; }
        public List<DayStep> Steps { get; set; }
        public IList<EmergencyContact> EmergencyContacts { get; set; }
        public DayTaskFeedback Feedback { get; set; }
        public DayTaskState State { get; set; }
        public string FreeFormPlace { get; set; }

        public Picture Picture { get; set; }

        public Place Place { get; set; }

        public TimeSpan ExpirationOffset { get; set; }

        public void AddDelay(TimeSpan offset)
        {
            // add offset starting from now, due this
            // calculate end to now
            // and add it to offset

            var offsetEndToNow = DateTimeHelper.Now.TimeOfDay.Subtract(this.End);
            offset = offset.Add(offsetEndToNow);

            this.ExpirationOffset = this.ExpirationOffset.Add(offset);
        }

        public override string ToString()
        {
            return $"{Name} : {Start} - {End} + offset {ExpirationOffset} | {State}";
        }

        public override bool Equals(object obj)
        {
            if(obj is DayTask dayTask)
            {
                var equals = this.State == dayTask.State
                    && this.Feedback == dayTask.Feedback;

                return equals;
            }

            return false;
        }
    }
}
