using System.Globalization;
using Scheduler.Engine.Enums;
using Scheduler.Engine.Jobs;

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

        #region Property Specified

        private bool _isDescSpecified;
        public bool DescriptionSpecified
        {
            get
            {
                return (_isDescSpecified || !string.IsNullOrEmpty(Description));
            }
            set
            {
                _isDescSpecified = value;
            }
        }

        private bool _isScheduleSpecified;
        public bool ScheduleSpecified
        {
            get
            {
                return (_isScheduleSpecified || !string.IsNullOrEmpty(Schedule));
            }
            set
            {
                _isScheduleSpecified = value;
            }
        }

        private bool _isStateSpecified;
        public bool StateSpecified
        {
            get
            {
                return (_isStateSpecified || !string.IsNullOrEmpty(State));
            }
            set
            {
                _isStateSpecified = value;
            }
        }

        private bool _isNextRunSpecified;
        public bool NextFireTimeSpecified
        {
            get
            {
                return (_isNextRunSpecified || !string.IsNullOrEmpty(NextFireTime));
            }
            set
            {
                _isNextRunSpecified = value;
            }
        }

        private bool _isLastRunSpecified;
        public bool PrevFireTimeSpecified
        {
            get
            {
                return (_isLastRunSpecified || !string.IsNullOrEmpty(PreviousFireTime));
            }
            set
            {
                _isLastRunSpecified = value;
            }
        }

        #endregion

        public static JobDetails Transform(JobInfo jobInfo)
        {
            return new JobDetails
            {
                Name = jobInfo.Name,
                Group = jobInfo.Group,
                Description = jobInfo.Description,
                DescriptionSpecified = jobInfo.DescriptionSpecified,
                Schedule = jobInfo.Schedule,
                ScheduleSpecified = jobInfo.ScheduleSpecified,
                State = jobInfo.State,
                StateSpecified = jobInfo.StateSpecified,
                ActionState = jobInfo.ActionState,
                PreviousFireTime = jobInfo.PrevFireTimeUtc?.LocalDateTime.ToString(CultureInfo.InvariantCulture),
                PrevFireTimeSpecified = jobInfo.PrevFireTimeSpecified,
                NextFireTime = (jobInfo.State != JobState.Paused.ToString()) ? jobInfo.NextFireTimeUtc?.LocalDateTime.ToString(CultureInfo.InvariantCulture) : string.Empty, // if the job is on hold - we don't want to show next execution time
                NextFireTimeSpecified = jobInfo.NextFireTimeSpecified
            };
        }
    }
}
