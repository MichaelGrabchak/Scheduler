namespace Scheduler.Logging
{
    public interface ILoggerProvider
    {
        ILogger GetLogger();
        ILogger GetLogger(string logName);
        ILogger GetLogger(string job, string group);
    }
}
