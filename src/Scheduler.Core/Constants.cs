namespace Scheduler.Core
{
    public class Constants
    {
        public class System
        {
            public const string DefaultSchedulerLoggerName = "SchedulerLogger";
        }

        public class Scheduler
        {
            public const string DefaultJobsPath = @"E:\Development\Sources\Scheduler\src\Jobs";

            public class Frequency
            {
                public class Cron
                {
                    public const string EveryMinute = "0 0/1 * 1/1 * ? *";
                }
            }
        }
    }
}
