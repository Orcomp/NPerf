namespace NPerf.Core.Communication
{
    using System.Threading;

    internal static class NamedSemaphore
    {
        private static readonly object sync = new object();

        public static Semaphore OpenOrCreate(string name, int initialCount, int maximumCount)
        {
            Semaphore sem;
            lock (sync)
            {
                if (!TryOpenExistingSemaphore(name, out sem))
                {
                    sem = new Semaphore(initialCount, maximumCount, name);
                }
            }

            return sem;
        }

        private static bool TryOpenExistingSemaphore(string name, out Semaphore semaphore)
        {
            try
            {
                semaphore = Semaphore.OpenExisting(name);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                semaphore = null;
            }

            return semaphore != null;
        }

    }
}
