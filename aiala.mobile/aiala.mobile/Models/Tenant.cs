using System;
using System.Text;

namespace aiala.mobile.Models
{
    public class Tenant
    {
        /// <summary>
        /// Tenant identifier
        /// </summary>
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }

        public bool Enabled { get; set; }
    }
}
