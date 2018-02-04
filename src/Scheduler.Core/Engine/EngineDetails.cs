using System;
using System.Text;

namespace Scheduler.Core.Engine
{
    public class EngineDetails
    {
        public string Name { get; set; }
        public string State { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public string Version { get; set; }
        public string InstanceId { get; set; }
        public string InstanceName { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if(!string.IsNullOrEmpty(Name))
            {
                sb.Append($"Name: {Name}");
            }

            if(!string.IsNullOrEmpty(State))
            {
                sb.Append($" State: {State}");
            }

            if(StartDate != null && StartDate != DateTimeOffset.MaxValue)
            {
                sb.Append($" Start Date: {StartDate}");
            }

            if(!string.IsNullOrEmpty(Version))
            {
                sb.Append($" Version: {Version}");
            }

            if(!string.IsNullOrEmpty(InstanceId))
            {
                sb.Append($" Instance ID: {InstanceId}");
            }

            if(!string.IsNullOrEmpty(InstanceName))
            {
                sb.Append($" Instance Name: {InstanceName}");
            }

            return sb.ToString().Trim();
        }
    }
}
