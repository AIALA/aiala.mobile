using System;
using System.Collections.Generic;

namespace aiala.mobile.Models
{
    public class Picture
    {
        public Guid Id { get; set; }

        public string PictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public PictureMetadata AiMetadata { get; set; }
    }
}
