using System.Collections.Generic;

namespace Scheduler.Core.Configurations
{
    public static class GlobalLoggingSettings
    {
        private static readonly IDictionary<string, string> JobLoggerNames;

        static GlobalLoggingSettings()
        {
            JobLoggerNames = new Dictionary<string, string>();
        }

        public static string GetLoggerName(string key)
        {
            return GetLogger(key);
        }

        public static string GetLoggerName(string group, string job)
        {
            return GetLogger($"{group}.{job}");
        }

        public static void SetLoggerName(string key, string loggerName)
        {
            SetLogger(key, loggerName);
        }

        public static void SetLoggerName(string group, string job, string loggerName)
        {
            SetLogger($"{group}.{job}", loggerName);
        }

        private static void SetLogger(string key, string logger)
        {
            if (JobLoggerNames.ContainsKey(key))
            {
                JobLoggerNames[key] = logger;
                return;
            }

            JobLoggerNames.Add(key, logger);
        }
        
        private static string GetLogger(string key)
        {
            return (string.IsNullOrEmpty(key) || !JobLoggerNames.ContainsKey(key))
                ? Constants.System.DefaultSchedulerLoggerName
                : JobLoggerNames[key];
        }
    }
}
