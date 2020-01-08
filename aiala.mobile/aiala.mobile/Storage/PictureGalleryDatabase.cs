using Xamarin.Forms.Xaml;
using System.Reactive.Linq;
using System;
using SQLite;
using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using System.Threading.Tasks;
using System.Linq;
using aiala.mobile.Models;
using System.IO;

namespace aiala.mobile.Storage
{
    public class PictureGalleryDatabase
    {
        private readonly SQLiteConnection _database;

        public PictureGalleryDatabase()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PictureGallery.db3");

            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<GalleryItemTag>();
            _database.CreateTable<GalleryItem>();
        }

        public List<Picture> GetItems(int take = 5)
        {
            var items = _database.GetAllWithChildren<GalleryItem>(recursive: true);

            var result = items.Select(To)
                .OrderByDescending(o => o.CreatedAt)
                .Take(take)
                .ToList();

            return result;
        }
        public List<Picture> GetRelatedItems(Picture picture, int take = 5)
        {
            // three most confident tags
            var tags = picture.GetMostConfidentTags(3);

            var parentId = picture.Id;

            if (!tags.Any())
                return new List<Picture>();

            var ofTag = _database.GetAllWithChildren<GalleryItemTag>(q => tags.Contains(q.Name), recursive: true);

            var result = ofTag
                .Where(q => q.GalleryItem != null)
                .Select(s => s.GalleryItem)

                .Where(q => q.Id != parentId)
                .Distinct(new GalleryItemEqualitiyComparer())
                
                // order by best average confidence of three most confidente tags
                .OrderByDescending(o => o.Tags?.Where(q => tags.Contains(q.Name)).Average(avg => avg.Confidence) ?? 0.0)
                .OrderByDescending(o => o.TakenAt)
                
                .Take(take)
                .Select(To)
                .ToList();

            return result;
        }

        public Picture GetPicture(Guid pictureId)
        {
            var galleryItem = GetGalleryItem(pictureId);
            return To(galleryItem);
        }

        private GalleryItem GetGalleryItem(Guid pictureId)
        {
            var galleryItem = _database.GetWithChildren<GalleryItem>(pictureId);
            return galleryItem;
        }

        public void ReplaceAllItems(List<Picture> pictures)
        {
            CleanupDatabase();
            var items = pictures.Select(From).ToList();
            _database.InsertOrReplaceAllWithChildren(items);
        }

        public void UpsertItem(Picture picture)
        {
            var item = From(picture);
            _database.InsertOrReplaceWithChildren(item);
        }

        public void DeletePicture(Guid pictureId)
        {
            var galleryItem = GetGalleryItem(pictureId);
            _database.Delete(galleryItem);
        }

        public void CleanupDatabase()
        {
            _database.Execute("DELETE FROM GalleryItem");
            _database.Execute("DELETE FROM GalleryItemTag");
        }

        private Picture To(GalleryItem item)
        {
            if (item == null)
                return null;

            return new Picture
            {
                Id = item.Id,
                PictureUrl = item.PictureUrl,
                CreatedAt = item.TakenAt,
                AiMetadata = new PictureMetadata
                {
                    Description = item.Description,
                    DescriptionConfidence = item.DescriptionConfidence,
                    Tags = item.Tags?.Select(s => new PictureTag
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Confidence = s.Confidence
                    }).ToList() ?? new List<PictureTag>()
                }
            };
        }

        private GalleryItem From(Picture item)
        {
            var result = new GalleryItem
            {
                Id = item.Id,
                Description = item.AiMetadata?.Description,
                DescriptionConfidence = item.AiMetadata?.DescriptionConfidence ?? 0,
                PictureUrl = item.PictureUrl,
                TakenAt = item.CreatedAt,
                Tags = new List<GalleryItemTag>()
            };

            if (item.AiMetadata?.Tags.Any() == true)
            {
                foreach (var tag in item.AiMetadata?.Tags)
                {
                    result.Tags.Add(new GalleryItemTag
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Confidence = tag.Confidence,
                        GalleryItemId = item.Id,
                    });
                }
            }

            return result;
        }

        private class GalleryItemEqualitiyComparer : EqualityComparer<GalleryItem>
        {
            public override bool Equals(GalleryItem x, GalleryItem y)
            {
                if (x == null && y == null)
                    return true;

                if ((x == null && y != null) || (x != null && y == null))
                    return false;

                return x.Id.Equals(y.Id);
            }

            public override int GetHashCode(GalleryItem obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
