using System;

namespace aiala.mobile.Activities.Emergencies
{
    public class StartEmergencyActivity : EmergencyActivityBase
    {
        public StartEmergencyActivity(Guid emergencyId, DateTimeOffset timestamp) : base(emergencyId, timestamp)
        {
        }
    }
}
