using Scheduler.Core.Extensions;
using System;
using System.Collections.Generic;

namespace Scheduler.Core.Jobs.Metadata
{
    public abstract class JobMetadata
    {
        public Type Type { get; set; }

        public string Name { get; set; }
        public string Group { get; set; }

        public string Schedule { get; set; }

        public string Description { get; set; }

        public JobMetadata ExtractData(BaseJob job)
        {
            if (!IsScheduleExpressionValid(job.Schedule))
            {
                throw new ArgumentNullException(nameof(job.Schedule));
            }

            Type = job.GetType();
            Name = job.GetName();
            Group = job.GetGroup();
            Schedule = job.Schedule;
            Description = job.GetDescription();

            return this;
        }

        public IEnumerable<JobMetadata> ExtractData(IEnumerable<BaseJob> jobs)
        {
            var extractedData = new List<JobMetadata>();

            foreach (var job in jobs)
            {
                extractedData.Add(ExtractData(job));
            }

            return extractedData;
        }

        protected abstract bool IsScheduleExpressionValid(string schedule);
    }
}
