using Newtonsoft.Json;
using System.IO;

namespace Clicker
{
    class JSONSerializer
    {
        public static void SerializeScenario(Scenario scenario)
        {
            string fileName = string.Format("scenario_{0}.json", scenario.Name);

            using (StreamWriter file = File.CreateText(fileName))
            {
                string json = JsonConvert.SerializeObject(scenario, Formatting.Indented, new ActionsJsonConverter());
                file.Write(json);
            }
        }

        public static Scenario DeserializeScenario(string name)
        {
            string fileName = string.Format("scenario_{0}.json", name);

            using (StreamReader file = File.OpenText(fileName))
            {
                string content = file.ReadToEnd();
                return JsonConvert.DeserializeObject<Scenario>(content, new ActionsJsonConverter());
            }
        }

    }
}
