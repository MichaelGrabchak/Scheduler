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
                JobNextRunTime = NextFireTimeUtc.HasValue ? NextFireTimeUtc.Value.UtcDateTime : (DateTime?)null,
                JobLastRunTime = PrevFireTimeUtc.HasValue ? PrevFireTimeUtc.Value.UtcDateTime : (DateTime?)null
            };
        }

        public JobInfo SetValues(JobInfo newValues)
        {
            if(newValues != null)
            {
                if(Id <= 0)
                {
                    Id = newValues.Id;
                }

                if (string.IsNullOrEmpty(Name))
                {
                    Name = newValues.Name;
                }

                if (string.IsNullOrEmpty(Group))
                {
                    Group = newValues.Group;
                }

                if (string.IsNullOrEmpty(Description))
                {
                    Description = newValues.Description;
                }

                if (string.IsNullOrEmpty(Schedule))
                {
                    Schedule = newValues.Schedule;
                }

                if (string.IsNullOrEmpty(ScheduleExpression))
                {
                    ScheduleExpression = newValues.ScheduleExpression;
                }

                if (string.IsNullOrEmpty(ActionState))
                {
                    ActionState = newValues.ActionState;
                }

                if(string.IsNullOrEmpty(State))
                {
                    State = newValues.State;
                }

                if (!NextFireTimeUtc.HasValue)
                {
                    NextFireTimeUtc = newValues.NextFireTimeUtc;
                }

                if (!PrevFireTimeUtc.HasValue)
                {
                    PrevFireTimeUtc = newValues.PrevFireTimeUtc;
                }

                if(string.IsNullOrEmpty(LoggerKey))
                {
                    LoggerKey = newValues.LoggerKey;
                }
            }

            return this;
        }

        public static JobInfo Transform(JobMetadata metadata)
        {
            return new JobInfo
            {
                Name = metadata.Name,
                Group = metadata.Group,
                Description = metadata.Description,
                ScheduleExpression = metadata.Schedule
            };
        }

        public static JobDetail Transform(JobInfo jobInfo)
        {
            return new JobDetail
            {
                Id = jobInfo.Id,
                JobName = jobInfo.Name,
                JobGroup = jobInfo.Group,
                JobDescription = jobInfo.Description,
                JobSchedule = jobInfo.ScheduleExpression,
                JobNextRunTime = jobInfo.NextFireTimeUtc.HasValue ? jobInfo.NextFireTimeUtc.Value.UtcDateTime : (DateTime?)null,
                JobLastRunTime = jobInfo.PrevFireTimeUtc.HasValue ? jobInfo.PrevFireTimeUtc.Value.UtcDateTime : (DateTime?)null
            };
        }
    }
}
