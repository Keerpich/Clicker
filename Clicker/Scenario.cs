using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicker
{
    class Scenario
    {
        public Scenario(String name)
        {
            this.name = name;
        }

        public void Execute()
        {
            foreach (Action action in actions)
            {
                action.Execute();
            }
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }

        private readonly List<Action> actions = new List<Action>();
        private String name;

        public List<Action> Actions {get => actions;}
        public String Name { get => name; set => name = value; }
    }
}
