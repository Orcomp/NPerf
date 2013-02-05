namespace NPerf.Test.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NPerf.Core.Communication;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;

    public class SendReceiveLockHelper : IDisposable
    {
        private readonly SendReceiveLock locker;

        private List<Action> sendActions = new List<Action>();

        private List<Func<object>> recieveFunctions = new List<Func<object>>();
        
        public SendReceiveLockHelper(string fullName, string emptyName)
        {
            this.locker = new SendReceiveLock(fullName, emptyName);
        }

        public bool Send(TimeSpan timeout)
        {
            var tasks =
                this.sendActions.Select(sending => (Action)(() => this.locker.Send(sending)))
                    .Select(useLocker => Task.Factory.StartNew(useLocker))
                    .ToArray();

            return Task.WaitAll(tasks, timeout);
        }

        public bool Receive<T>(ConcurrentBag<T> list, TimeSpan timeout)
        {
            var tasks =
                this.recieveFunctions.Select(receiving => (Action)(() => list.Add((T)this.locker.Receive(receiving))))
                    .Select(useLocker => Task.Factory.StartNew(useLocker))
                    .ToArray();

            return Task.WaitAll(tasks, timeout);
        }

        public void AddSendAction(Action action)
        {
            this.sendActions.Add(action);
        }

        public void AddReceiveFunction(Func<object> func)
        {
            this.recieveFunctions.Add(func);
        }

        public void Dispose()
        {
            this.locker.Dispose();
        }
    }
}
