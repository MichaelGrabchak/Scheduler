using ConsoleApi = System.Console;

using Scheduler.Domain.Data.Services;
using Scheduler.Engine;

namespace Scheduler.Console.Configurations
{
    public class SchedulerConsoleSettings : SchedulerSettings
    {
        public SchedulerConsoleSettings(ISchedulerInstanceService schedulerInstanceService)
            : base()
        {
            var instanceSettings = schedulerInstanceService.GetSettings();

            InstanceId = instanceSettings.InstanceId;
            InstanceName = instanceSettings.InstanceName;
            StartEngineImmediately = instanceSettings.IsImmediateEngineStartEnabled;
            EnableJobsDirectoryTracking = instanceSettings.IsJobsDirectoryTrackingEnabled;

            EngineStarted = (sender, e) => { ConsoleApi.WriteLine("The engine has been started..."); };
            EnginePaused = (sender, e) => { ConsoleApi.WriteLine("The engine has been paused..."); };
            EngineTerminated = (sender, e) => { ConsoleApi.WriteLine("The engine has been terminated..."); };

            JobScheduled = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been scheduled"); };
            JobUnscheduled = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been unscheduled"); };
            JobPaused = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been put on hold"); };
            JobResumed = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been resumed"); };
            JobTriggered = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been triggered manually"); };

            JobExecutionSucceeded = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has finished with status: SUCCESS"); };
            JobExecutionFailed = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has finished with status: FAILED"); };
            JobExecutionSkipped = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) execution has been skipped"); };

            BeforeJobExecution = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) is going to be executed"); };
            AfterJobExecution = (sender, e) => { ConsoleApi.WriteLine($"The job (Name:{e.Job.Name}, Group:{e.Job.Group}) has been executed"); };
        }
    }
}