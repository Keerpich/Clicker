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

    }
}
