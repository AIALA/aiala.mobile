using System;
using System.Collections.Generic;
using System.Text;

namespace aiala.mobile.Models
{
    public class PictureMetadata
    {
        public PictureMetadata()
        {
            this.Tags = new List<PictureTag>();
        }

        public Guid Id { get; set; }

        public string Description { get; set; }

        public double DescriptionConfidence { get; set; }

        public List<PictureTag> Tags { get; set; }
    }
}
