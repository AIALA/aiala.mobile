using System;
using System.Collections.Generic;
using System.Text;

namespace aiala.mobile.BackgroundServices
{
    public static class ActivityQueues
    {
        public const string High = "HighQueue";

        public const string Low = "LowQueue";

        public static readonly string[] All = new[] { High, Low };
    }
}
