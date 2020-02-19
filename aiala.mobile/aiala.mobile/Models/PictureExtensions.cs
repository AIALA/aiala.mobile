using System.Collections.Generic;
using System.Linq;

namespace aiala.mobile.Models
{
    public static class PictureExtensions
    {
        public static List<string> GetMostConfidentTags(this Picture picture, int count)
        {
            if (picture == null
                || picture.AiMetadata == null
                || picture.AiMetadata.Tags == null
                || !picture.AiMetadata.Tags.Any())
            {
                return new List<string>();
            }

            var tags = picture.AiMetadata.Tags
                .OrderByDescending(o => o.Confidence)
                .Take(count)
                .Select(s => s.Name)
                .ToList();

            return tags;
        }

        public static string GetConfidentTags(this Picture picture, int count)
        {
            var tags = picture.GetMostConfidentTags(count);
            return string.Join(", ", tags);
        }

        public static string GetTags(this Picture picture)
        {
            if (picture == null
                || picture.AiMetadata == null
                || picture.AiMetadata.Tags == null
                || !picture.AiMetadata.Tags.Any())
            {
                return string.Empty;
            }

            var tags = picture.AiMetadata.Tags
                .Select(s => s.Name);

            return string.Join(", ", tags);
        }
    }
}
