using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace aiala.mobile.Activities.Steps
{
    public class StepActivityBase : ActivityBase
    {
        public StepActivityBase(Guid stepId, DateTimeOffset timestamp) : base(timestamp)
        {
            StepId = stepId;
            ActiveStepId = stepId;
        }

        public Guid StepId { get; }
    }
}
