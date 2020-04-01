using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicker
{
    class SleepAction : Action
    {
        public override void Execute()
        {
            InputSimulatorWrapper.Sleep(TimeToSleepInMs);
        }

        public int TimeToSleepInMs { get; set; } = 1000;
    }
}
