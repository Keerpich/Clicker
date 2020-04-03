using Newtonsoft.Json;
using System.IO;

namespace Clicker
{
    class JSONSerializer
    {
        public static void SerializeScenario(Scenario scenario, string fullPath)
        {
            using (StreamWriter file = File.CreateText(fullPath))
            {
                string json = JsonConvert.SerializeObject(scenario, Formatting.Indented, new ActionsJsonConverter());
                file.Write(json);
            }
        }

        public static Scenario DeserializeScenario(string fullPath)
        {
            using (StreamReader file = File.OpenText(fullPath))
            {
                string content = file.ReadToEnd();
                return JsonConvert.DeserializeObject<Scenario>(content, new ActionsJsonConverter());
            }
        }

    }
}
