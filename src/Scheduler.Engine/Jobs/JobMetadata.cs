using System;
using System.Collections.Generic;

using Scheduler.Domain.Data.Services;
using Scheduler.Jobs;
using Scheduler.Jobs.Extensions;

namespace Scheduler.Engine.Jobs
{
    public abstract class JobMetadata
    {
        protected readonly IJobDetailService _jobDetailService;

        public Type Type { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }

        public string Schedule { get; set; }
        public string Description { get; set; }
        public byte State { get; set; }

        public JobMetadata(IJobDetailService jobDetailService)
        {
            _jobDetailService = jobDetailService;
        }

        public JobMetadata ExtractData(BaseJob job)
        {
            if (!IsScheduleExpressionValid(job.Schedule))
            {
                throw new ArgumentNullException(nameof(job.Schedule));
            }

            Type = job.GetType();
            Name = job.GetName();
            Group = job.GetGroup();

            var jobDetail = _jobDetailService.GetJobDetail(Name, Group);
            
            Schedule = jobDetail?.JobSchedule ?? job.Schedule;
            Description = jobDetail?.JobDescription ?? job.GetDescription();

            State = jobDetail?.StatusId ?? 1;

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
