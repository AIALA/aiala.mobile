using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace aiala.mobile.Storage
{
    [Table("GalleryItemTag")]
    public class GalleryItemTag
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(GalleryItem))]
        public Guid GalleryItemId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public GalleryItem GalleryItem { get; set; }

        public string Name { get; set; }

        public double Confidence { get; set; }
    }
}
