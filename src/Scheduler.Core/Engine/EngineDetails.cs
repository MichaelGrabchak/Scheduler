using System;

namespace Scheduler.Core.Engine
{
    public class EngineDetails
    {
        public string Name { get; set; }
        public string State { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public string Version { get; set; }
    }
}
