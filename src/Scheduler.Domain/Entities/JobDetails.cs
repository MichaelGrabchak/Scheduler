using System.Globalization;

using Scheduler.Core.Jobs;
using Scheduler.Domain.Entities.Enumerations;

namespace Scheduler.Domain.Entities
{
    public class JobDetails
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Schedule { get; set; }
        public JobState State { get; set; }

        public string PreviousFireTime { get; set; }
        public string NextFireTime { get; set; }

        public static JobDetails Transform(JobInfo jobInfo)
        {
            return new JobDetails
            {
                Name = jobInfo.Name,
                Group = jobInfo.Group,
                Schedule = jobInfo.Schedule,
                State = (jobInfo.State == "Normal") ? JobState.Running : JobState.Paused,
                PreviousFireTime = jobInfo.PrevFireTimeUtc?.LocalDateTime.ToString(CultureInfo.InvariantCulture),
                NextFireTime = jobInfo.NextFireTimeUtc?.LocalDateTime.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}
