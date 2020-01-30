using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Ageha.Util
{
    public class JsonWrapper
    {
        public static JObject ReadJSON(string path)
        {
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return (JObject)JToken.ReadFrom(reader);
            }
        }

        public static T JsonChoose<T>(JObject choices) => choices.Value<T>(Math.Min(new Random().Next() * choices.Count + 1, choices.Count - 1));

        public static T JsonChoose<T>(string path)
        {
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return JsonChoose<T>((JObject)JToken.ReadFrom(reader));
            }
        }
    }
}