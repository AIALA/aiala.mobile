using System;
using System.Collections.Generic;
using System.Text;

namespace aiala.mobile.Activities.Emergencies
{
    public class EndEmergencyActivity : EmergencyActivityBase
    {
        public EndEmergencyActivity(Guid emergencyId, DateTimeOffset timestamp) : base(emergencyId, timestamp)
        {
        }
    }
}
