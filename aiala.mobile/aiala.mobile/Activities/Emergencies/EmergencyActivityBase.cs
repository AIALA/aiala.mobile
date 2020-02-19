using System;

namespace aiala.mobile.Activities.Emergencies
{
    public class EmergencyActivityBase : ActivityBase
    {
        public EmergencyActivityBase(Guid emergencyId, DateTimeOffset timestamp) : base(timestamp)
        {
            EmergencyId = emergencyId;
        }

        public Guid EmergencyId { get; }
    }
}
