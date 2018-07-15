namespace Scheduler.Core
{
    public class Constants
    {
        public class Scheduler
        {
            public class System
            {
                public const string DefaultLogger = "SchedulerLogger";
            }

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
