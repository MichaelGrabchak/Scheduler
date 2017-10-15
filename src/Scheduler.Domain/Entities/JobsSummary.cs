using System.Linq;
using System.Collections.Generic;

using Scheduler.Core.Jobs;

namespace Scheduler.Domain.Entities
{
    public class JobsSummary
    {
        public int TotalCount => Jobs?.Count ?? 0;
        public int TotalRunning => Jobs?.Count(_ => _.State == JobState.Normal.ToString()) ?? 0;
        public int TotalPaused => Jobs?.Count(_ => _.State == JobState.Paused.ToString()) ?? 0;

        public IList<JobDetails> Jobs { get; set; }

        private JobsSummary()
        {
            Jobs = new List<JobDetails>();
        }

        public JobsSummary(IEnumerable<JobDetails> jobsData)
            : this()
        {
            if (jobsData != null)
            {
                Jobs = jobsData.ToList();
            }
        }
    }
}