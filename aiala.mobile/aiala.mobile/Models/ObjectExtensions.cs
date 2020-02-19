using Newtonsoft.Json;

namespace aiala.mobile.Models
{
    public static class ObjectExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }
    }
}
