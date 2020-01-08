using System;
using System.Collections.Generic;

namespace aiala.mobile.Models
{
    public class DaySchedule
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public List<DayTask> Tasks { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Date}";
        }
    }
}
