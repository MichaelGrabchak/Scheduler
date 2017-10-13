using System;
using System.Collections.Generic;

using Scheduler.Core.Jobs;

using Quartz;

namespace Scheduler.Engine.Quartz
{
    public class JobData
    {
        public Type Type { get; private set; }

        public string Name { get; private set; }
        public string Group { get; private set; }

        public string Schedule { get; private set; }

        private JobData()
        {

        }

        public static JobData ExtractData(BaseJob job)
        {
            if (!IsValidSchedule(job.Schedule))
            {
                throw new ArgumentNullException(nameof(job.Schedule));
            }

            return new JobData
            {
                Type = job.GetType(),

                Name = job.GetType().Name,
                Group = job.GetType().Namespace,

                Schedule = job.Schedule
            };
        }

        public static IEnumerable<JobData> ExtractData(IEnumerable<BaseJob> jobs)
        {
            var extractedData = new List<JobData>();

            foreach(var job in jobs)
            {
                extractedData.Add(ExtractData(job));
            }

            return extractedData;
        }

        private static bool IsValidSchedule(string schedule)
        {
            return CronExpression.IsValidExpression(schedule);
        }
    }
}
