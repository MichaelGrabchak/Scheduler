using System;
using System.Collections.Generic;

using Scheduler.Core.Jobs;
using Scheduler.Core.Extensions;

using Quartz;

namespace Scheduler.Engine.Quartz
{
    public class JobMetadata
    {
        public Type Type { get; private set; }

        public string Name { get; private set; }
        public string Group { get; private set; }

        public string Schedule { get; private set; }

        public string Description { get; private set; }

        private JobMetadata()
        {

        }

        public static JobMetadata ExtractData(BaseJob job)
        {
            if (!IsScheduleValid(job.Schedule))
            {
                throw new ArgumentNullException(nameof(job.Schedule));
            }

            return new JobMetadata
            {
                Type = job.GetType(),

                Name = job.GetName(),
                Group = job.GetGroup(),

                Schedule = job.Schedule,

                Description = job.GetDescription()
            };
        }

        public static IEnumerable<JobMetadata> ExtractData(IEnumerable<BaseJob> jobs)
        {
            var extractedData = new List<JobMetadata>();

            foreach(var job in jobs)
            {
                extractedData.Add(ExtractData(job));
            }

            return extractedData;
        }

        private static bool IsScheduleValid(string schedule)
        {
            return CronExpression.IsValidExpression(schedule);
        }
    }
}
