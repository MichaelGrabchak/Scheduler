using System;
using System.Linq;
using System.Collections.Generic;

namespace Scheduler.Domain.Entities
{
    public class JobsSummary
    {
        public int TotalCount => Jobs?.Count ?? 0;
        public int TotalRunning => Jobs?.Count(job => job.State.Equals("Normal", StringComparison.CurrentCultureIgnoreCase)) ?? 0;
        public int TotalPaused => Jobs?.Count(job => job.State.Equals("Paused", StringComparison.CurrentCultureIgnoreCase)) ?? 0;

        public IList<JobDetails> Jobs { get; set; } = new List<JobDetails>();

        public JobsSummary(IEnumerable<JobDetails> jobsData)
        {
            if (jobsData != null)
            {
                Jobs = jobsData.ToList();
            }
        }
    }
}