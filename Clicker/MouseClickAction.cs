﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicker
{
    class MouseClickAction : Action
    {
        public override void Execute()
        {
            InputSimulatorWrapper.ClickAtLocation(x, y);
        }

        public int x;
        public int y;
    }
}
