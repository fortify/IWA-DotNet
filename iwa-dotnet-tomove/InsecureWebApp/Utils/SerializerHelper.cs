using Newtonsoft.Json;
namespace MicroFocus.InsecureWebApp.Utils
{
    public static class SerializerHelper
    {
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static object Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<object>(json,
                // Include TypeNameHandling.All to allow for a more smooth casting and check
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
    }
}
