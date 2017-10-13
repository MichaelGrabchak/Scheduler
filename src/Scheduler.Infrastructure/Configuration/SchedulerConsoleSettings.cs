using System;

using Scheduler.Core.Engine;

namespace Scheduler.Infrastructure.Configuration
{
    public class SchedulerConsoleSettings : SchedulerSettings
    {
        public SchedulerConsoleSettings()
        {
            EngineStarted = (sender, e) => { Console.WriteLine("The engine has been started..."); };
            EnginePaused = (sender, e) => { Console.WriteLine("The engine has been paused..."); };
            EngineTerminated = (sender, e) => { Console.WriteLine("The engine has been terminated..."); };

            JobScheduled = (sender, e) => { Console.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been scheduled"); };
            JobUnscheduled = (sender, e) => { Console.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been unscheduled"); };
            JobPaused = (sender, e) => { Console.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been put on hold"); };
            JobResumed = (sender, e) => { Console.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been resumed"); };
            JobTriggered = (sender, e) => { Console.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been triggered manually"); };
        }
    }
}