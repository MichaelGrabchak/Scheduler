using System;

namespace Scheduler.Core.Jobs
{
    public class JobInfo
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }

        public string State { get; set; }

        public DateTimeOffset? NextFireTimeUtc { get; set; }
        public DateTimeOffset? PrevFireTimeUtc { get; set; }
    }
}
