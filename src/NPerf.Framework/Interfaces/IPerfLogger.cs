namespace NPerf.Framework.Interfaces
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
