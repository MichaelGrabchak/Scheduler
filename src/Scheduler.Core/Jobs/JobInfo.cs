using System;
using System.Text;

namespace Scheduler.Core.Jobs
{
    public class JobInfo
    {
        public string Group { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        public string Schedule { get; set; }

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
    }
}
