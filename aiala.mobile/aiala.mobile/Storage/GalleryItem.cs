using System;
using SQLite;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using aiala.mobile.Models;
using System.Linq;

namespace aiala.mobile.Storage
{
    [Table("GalleryItem")]
    public class GalleryItem
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public double DescriptionConfidence { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<GalleryItemTag> Tags { get; set; }

        public DateTime TakenAt { get; set; }

        public byte[] Image { get; set; }
    }
}
