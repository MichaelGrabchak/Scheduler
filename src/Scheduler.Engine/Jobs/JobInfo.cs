using System;
using System.Text;

using Scheduler.Domain.Data.Dto;

namespace Scheduler.Engine.Jobs
{
    public class JobInfo
    {
        public int Id { get; set; }

        public string Group { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        public string Schedule { get; set; }
        public string ScheduleExpression { get; set; }

        public string ActionState { get; set; }
        public string State { get; set; }

        public DateTimeOffset? NextFireTimeUtc { get; set; }
        public DateTimeOffset? PrevFireTimeUtc { get; set; }

        public string LoggerKey { get; set; }

        #region Property Specified

        private bool _isDescSpecified;
        public bool DescriptionSpecified
        {
            get => (_isDescSpecified || !string.IsNullOrEmpty(Description));
            set => _isDescSpecified = value;
        }

        private bool _isScheduleSpecified;
        public bool ScheduleSpecified
        {
            get => (_isScheduleSpecified || !string.IsNullOrEmpty(Schedule));
            set => _isScheduleSpecified = value;
        }

        private bool _isStateSpecified;
        public bool StateSpecified
        {
            get => (_isStateSpecified || !string.IsNullOrEmpty(State));
            set => _isStateSpecified = value;
        }

        private bool _isNextRunSpecified;
        public bool NextFireTimeSpecified
        {
            get => (_isNextRunSpecified || NextFireTimeUtc.HasValue);
            set => _isNextRunSpecified = value;
        }

        private bool _isLastRunSpecified;
        public bool PrevFireTimeSpecified
        {
            get => (_isLastRunSpecified || PrevFireTimeUtc.HasValue);
            set => _isLastRunSpecified = value;
        }

        #endregion

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            if(!string.IsNullOrEmpty(Group))
            {
                stringBuilder.Append($"Group: {Group}");
            }

            if(!string.IsNullOrEmpty(Name))
            {
                stringBuilder.Append($" Name: {Name}");
            }

            if(!string.IsNullOrEmpty(Description))
            {
                stringBuilder.Append($" Description: {Description}");
            }

            if(!string.IsNullOrEmpty(Schedule))
            {
                stringBuilder.Append($" Schedule: {Schedule}");
            }

            if(!string.IsNullOrEmpty(ActionState))
            {
                stringBuilder.Append($" Action state: {ActionState}");
            }

            if(!string.IsNullOrEmpty(State))
            {
                stringBuilder.Append($" State: {State}");
            }

            if(NextFireTimeUtc.HasValue)
            {
                stringBuilder.Append($" Next Fire Time (UTC): {NextFireTimeUtc}");
            }

            if(PrevFireTimeUtc.HasValue)
            {
                stringBuilder.Append($" Previous Fire Time (UTC): {PrevFireTimeUtc}");
            }

            if(!string.IsNullOrEmpty(LoggerKey))
            {
                stringBuilder.Append($" Logger Name: {LoggerKey}");
            }

            return stringBuilder.ToString().Trim();
        }

        public JobDetail ToJobDetail()
        {
            return new JobDetail {
                Id = Id,
                JobName = Name,
                JobGroup = Group,
                JobDescription = Description,
                JobSchedule = ScheduleExpression,
                JobNextRunTime = NextFireTimeUtc?.UtcDateTime,
                JobLastRunTime = PrevFireTimeUtc?.UtcDateTime
            };
        }

        public static JobInfo Create(string group, string name,
            int? id = null,
            string desc = null, bool isDescSpecified = false,
            string schedule = null, bool isScheduleSpecified = false,
            string scheduleExp = null,
            string actionState = null,
            string state = null, bool isStateSpecified = false,
            DateTimeOffset? nextFire = null, bool isNextFireTimeSpecified = false,
            DateTimeOffset? prevFire = null, bool isPrevFireTimeSpecified = false,
            string logger = null)
        {
            var jobInfo = new JobInfo
            {
                Id = id ?? 0,
                Group = group,
                Name = name,
                Description = desc, DescriptionSpecified = isDescSpecified,
                Schedule = schedule, ScheduleSpecified = isScheduleSpecified,
                ScheduleExpression = scheduleExp,
                ActionState = actionState,
                State = state, StateSpecified = isStateSpecified,
                NextFireTimeUtc = nextFire, NextFireTimeSpecified = isNextFireTimeSpecified,
                PrevFireTimeUtc = prevFire, PrevFireTimeSpecified = isPrevFireTimeSpecified,
                LoggerKey = logger
            };

            return jobInfo;
        }
    }
}
