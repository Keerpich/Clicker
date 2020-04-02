using Newtonsoft.Json;
using System.IO;

namespace Clicker
{
    class JSONSerializer
    {
        public static void SerializeScenario(Scenario scenario)
        {
            using (StreamWriter file = File.CreateText(@"scenario_1.json"))
            {
                string json = JsonConvert.SerializeObject(scenario, Formatting.Indented, new ActionsJsonConverter());
                file.Write(json);
            }
        }

        public static Scenario DeserializeScenario()
        {
            Scenario result;

            using (StreamReader file = File.OpenText(@"scenario_1.json"))
            {
                string content = file.ReadToEnd();
                result = JsonConvert.DeserializeObject<Scenario>(content, new ActionsJsonConverter());
            }

            return result;
        }

    }
}
