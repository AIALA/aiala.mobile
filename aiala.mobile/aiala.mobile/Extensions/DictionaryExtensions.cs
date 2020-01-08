using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aiala.mobile.Extensions
{
    public static class DictionaryExtensions
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        public static async Task<T> Dequeue<T>(this IDictionary<string, object> properties, string key)
        {
            if (!properties.TryGetValue(key, out var value) || !(value is Queue<string> queue) || queue.Count == 0)
            {
                return default(T);
            }

            // deserialize
            var stringEntry = queue.Dequeue();
            var entry = JsonConvert.DeserializeObject<T>(stringEntry, _jsonSerializerSettings);

            // save queue
            properties[key] = value;
            await App.Current.SavePropertiesAsync();

            return entry;
        }

        public static List<T> PeekAll<T>(this IDictionary<string, object> properties, string key)
        {
            if (!properties.TryGetValue(key, out var value) || !(value is Queue<string> queue))
            {
                return new List<T>();
            }

            return queue
                .Select(s => JsonConvert.DeserializeObject<T>(s, _jsonSerializerSettings))
                .ToList();
        }

        public static async Task Enqueue(this IDictionary<string, object> properties, object item, string key)
        {
            if (item == null)
            {
                return;
            }

            if (!properties.TryGetValue(key, out var value) || !(value is Queue<string> queue))
            {
                queue = new Queue<string>();
                properties[key] = value;
            }

            // serialize with type information
            var stringItem = JsonConvert.SerializeObject(item, typeof(object), _jsonSerializerSettings);

            // enqueue
            queue.Enqueue(stringItem);

            // save queue
            properties[key] = queue;
            await App.Current.SavePropertiesAsync();
        }

        public static int GetQueueItemsCount(this IDictionary<string, object> properties, params string[] queueNames)
        {
            var count = 0;

            foreach (var name in queueNames)
            {
                if (properties.TryGetValue(name, out var value) && value is Queue<string> queue)
                {
                    count += queue.Count;
                }
            }

            return count;
        }
    }
}
