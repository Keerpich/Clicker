using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clicker
{
    class Scenario
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public Scenario(String name)
        {
            this.name = name;
        }

        public void Execute()
        {
            Task actionExecutor = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (Action action in actions)
                    {
                        if (cancellationTokenSource.IsCancellationRequested)
                            return;

                        action.Execute();
                    }
                }
            }, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
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
