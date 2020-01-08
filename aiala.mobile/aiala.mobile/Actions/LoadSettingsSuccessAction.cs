using aiala.mobile.Models;
using Redux;
using System.Collections.Generic;

namespace aiala.mobile.Actions
{
    public class LoadSettingsSuccessAction : IAction
    {
        public LoadSettingsSuccessAction(List<EmergencyContact> emergencyContacts, List<NavigationLocation> locations, EmergencyInformation emergencyInformation)
        {
            EmergencyContacts = emergencyContacts ?? new List<EmergencyContact>();
            Locations = locations ?? new List<NavigationLocation>();
            EmergencyInformation = emergencyInformation;
        }

        public List<EmergencyContact> EmergencyContacts { get; }

        public List<NavigationLocation> Locations { get; }

        public EmergencyInformation EmergencyInformation { get; }
    }
}
