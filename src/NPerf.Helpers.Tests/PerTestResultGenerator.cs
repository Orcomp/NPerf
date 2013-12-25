namespace NPerf.Test.Helpers
{
    using NPerf.Core.PerfTestResults;

    public class PerTestResultGenerator
    {
        public static NextResult CreatePerfResult()
        {
            return new NextResult
                       {
                           Descriptor = 0.1,
                           Duration = 10.33,
                           MemoryUsage = 100322
                       };
        }

        public static ExperimentError CreateExperimentError()
        {
            return new ExperimentError
                       {
                           ExceptionType = "extype",
                           FullMessage = "fullmessage",
                           Message = "message",
                           Source = string.Empty,
                           Descriptor = -1
                       };
        }

        public static ExperimentCompleted CreateExperimentCompleted()
        {
            return new ExperimentCompleted();
        }
    }
}
