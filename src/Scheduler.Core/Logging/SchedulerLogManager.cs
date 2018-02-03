using System.Collections.Generic;

using Scheduler.Core.Configurations;

namespace Scheduler.Core.Logging
{
    public static class SchedulerLogManager
    {
        private static IDictionary<string, ISchedulerLogger> Loggers;

        static SchedulerLogManager()
        {
            Loggers = new Dictionary<string, ISchedulerLogger>();
        }

        public static ISchedulerLogger GetSchedulerLogger()
        {
            return GetLogger(GlobalLoggingSettings.GetLoggerName(null));
        }

        public static ISchedulerLogger GetJobLogger(string loggerName)
        {
            return GetLogger(loggerName);
        }

        public static ISchedulerLogger GetJobLogger(string group, string job)
        {
            return GetLogger(GlobalLoggingSettings.GetLoggerName(group, job));
        }

        private static ISchedulerLogger GetLogger(string loggerName)
        {
            if (!Loggers.ContainsKey(loggerName))
            {
                Loggers.Add(loggerName, 
                    GlobalUnity.Resolve<ISchedulerLogger>(
                        new KeyValuePair<string, object>("loggerName", loggerName)));
            }

            return Loggers[loggerName];
        }
    }
}
