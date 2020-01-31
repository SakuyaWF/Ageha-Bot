using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Ageha.Util
{
    public class JsonWrapper
    {
        /// <summary>
        /// Read a JSON from a file
        /// </summary>
        /// <param name="path">The path where the file is</param>
        /// <returns>A JObject containing the JSON of the file</returns>
        public static JObject ReadJSON(string path)
        {
            // Opens the file
            using (StreamReader file = File.OpenText(path))
            // Reads the json from the file
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                // Deserialize the JSON into the JObject
                return (JObject)JToken.ReadFrom(reader);
            }
        }

        /// <summary>
        /// Given a JObject, this method will choose randomily a token
        /// </summary>
        /// <typeparam name="T">The type of the returned object</typeparam>
        /// <param name="choices">The JObject containing the token choices</param>
        /// <returns>A random token from the JObject</returns>
        public static T JsonChoose<T>(JObject choices) => choices.Value<T>(new Random().Next(1, choices.Count + 1).ToString());

        /// <summary>
        /// Given a JObject, this method will choose randomily a token
        /// </summary>
        /// <typeparam name="T">The type of the returned object</typeparam>
        /// <param name="path">The path of the JSON</param>
        /// <returns>A random token from the JObject</returns>
        public static T JsonChoose<T>(string path)
        {
            // Opens the file
            using (StreamReader file = File.OpenText(path))
            // Reads the json from the file
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                // Deserialize the JSON into the JObject
                return JsonChoose<T>((JObject)JToken.ReadFrom(reader));
            }
        }
    }
}