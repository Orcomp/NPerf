namespace NPerf.Experiment
{
    using System;

    internal class StartArgumentsException : Exception
    {
        public StartArgumentsException(string message)
            : base(message)
        {
        }
    }
}
