using aiala.mobile.Models;
using System.Collections.Generic;

namespace aiala.mobile.Models
{
    public class TaskEqualityComparer : IEqualityComparer<DayTask>
    {
        public bool Equals(DayTask x, DayTask y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.ExpirationOffset == y.ExpirationOffset;
        }

        public int GetHashCode(DayTask obj)
        {
            return obj.ExpirationOffset.GetHashCode();
        }
    }
}
