using System;

namespace aiala.mobile.Models
{
    public class DayStep : Step
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public DayStepState State { get; set; }

        public override string ToString()
        {
            return $"{Text} | {Order} | {State}";
        }
    }

    public class DayStepFeedback : Step
    {
        public override string ToString()
        {
            return $"Day Step Feedback";
        }
    }

    public class Step { }
}
