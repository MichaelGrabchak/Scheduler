using System.Text;

namespace Scheduler.Domain.Entities
{
    public class EngineInfo
    {
        public string RunningSince { get; set; }
        public string Version { get; set; }
        public string Engine { get; set; }
        public string State { get; set; }
        public string InstanceId { get; set; }
        public string InstanceName { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Engine))
            {
                sb.Append($"Engine: {Engine}");
            }

            if (!string.IsNullOrEmpty(State))
            {
                sb.Append($" State: {State}");
            }

            if (!string.IsNullOrEmpty(RunningSince))
            {
                sb.Append($" Start Date: {RunningSince}");
            }

            if (!string.IsNullOrEmpty(Version))
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
