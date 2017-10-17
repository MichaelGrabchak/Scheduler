using System.Globalization;

using Scheduler.Core.Jobs;

namespace Scheduler.Domain.Entities
{
    public class JobDetails
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }
        public string State { get; set; }
        public string ActionState { get; set; }

        public string PreviousFireTime { get; set; }
        public string NextFireTime { get; set; }

        public static JobDetails Transform(JobInfo jobInfo)
        {
            return new JobDetails
            {
                Name = jobInfo.Name,
                Group = jobInfo.Group,
                Description = jobInfo.Description,
                Schedule = jobInfo.Schedule,
                State = jobInfo.State,
                ActionState = jobInfo.ActionState,
                PreviousFireTime = jobInfo.PrevFireTimeUtc?.LocalDateTime.ToString(CultureInfo.InvariantCulture),
                NextFireTime = (jobInfo.State != JobState.Paused.ToString()) ? jobInfo.NextFireTimeUtc?.LocalDateTime.ToString(CultureInfo.InvariantCulture) : string.Empty // if the job is on hold - we don't want to show next execution time
            };
        }
    }
}
