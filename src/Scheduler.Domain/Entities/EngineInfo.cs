using System.Globalization;

using Scheduler.Core.Engine;
using System;

namespace Scheduler.Domain.Entities
{
    public class EngineInfo
    {
        public string RunningSince { get; set; }
        public string Version { get; set; }
        public string Engine { get; set; }
        public string State { get; set; }

        public EngineInfo(EngineDetails details)
        {
            if (details != null)
            {
                RunningSince = (details.StartDate != DateTimeOffset.MinValue) ? details.StartDate.ToString(CultureInfo.InvariantCulture) : null;
                Version = details.Version;
                Engine = details.Name;
                State = details.State;
            }
        }
    }
}
