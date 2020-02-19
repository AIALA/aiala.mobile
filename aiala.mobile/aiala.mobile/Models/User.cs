using System.Collections.Generic;

namespace aiala.mobile.Models
{
    public class User
    {
        public User()
        {
            this.Tenants = new List<Tenant>();
        }
        /// <summary>
        /// Benutzer-ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Vorname
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Telefon
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// E-Mail Adresse
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Profile picture
        /// </summary>
        public string PictureUrl { get; set; }

        public List<Tenant> Tenants { get; set; }
    }
}
