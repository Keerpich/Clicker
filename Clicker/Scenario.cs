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
        private CancellationTokenSource cancellationTokenSource;
        public bool IsExecuting { get; private set; } = false;
        public List<Action> Actions { get; } = new List<Action>();
        public String Name { get; set; }

        public Scenario(String name)
        {
            this.Name = name;
        }

        public void Execute()
        {
            IsExecuting = true;

            cancellationTokenSource = new CancellationTokenSource();

            Task actionExecutor = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (Action action in Actions)
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
            IsExecuting = false;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        public void AddAction(Action action)
        {
            Actions.Add(action);
        }
    }
}
