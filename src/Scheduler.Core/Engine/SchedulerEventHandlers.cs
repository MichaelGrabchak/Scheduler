using System;

using Scheduler.Core.Jobs;

namespace Scheduler.Core.Engine
{
    public class SchedulerEventArgs : EventArgs
    { }

    public class EngineOperationEventArgs : SchedulerEventArgs
    {
        public EngineState State { get; set; }
    }

    public class JobOperationEventArgs : SchedulerEventArgs
    {
        public JobInfo Job { get; set; }
    }

    public delegate void SchedulerEventHandler(object sender, SchedulerEventArgs e);
    public delegate void EngineOperationEventHandler(object sender, EngineOperationEventArgs e);
    public delegate void JobOperationEventHandler(object sender, JobOperationEventArgs e);
}
