namespace NPerf.Framework
{
    public enum PrtfLogLevel
    {
        Debug,
        Info,
        Error
    }

    public interface IPerfLogger
    {
        void Log(PrtfLogLevel logLevel, string logRecord);
    }
}
