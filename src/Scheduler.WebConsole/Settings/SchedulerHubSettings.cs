using Microsoft.AspNet.SignalR;

using Scheduler.Domain.Data.Services;
using Scheduler.Engine;
using Scheduler.Engine.Extensions;
using Scheduler.Infrastructure.Hubs;

namespace Scheduler.WebConsole.Settings
{
    public class SchedulerHubSettings : SchedulerSettings
    {
        public SchedulerHubSettings(ISchedulerInstanceService schedulerInstanceService)
        {
            var instanceSettings = schedulerInstanceService.GetSettings();

            InstanceId = instanceSettings.InstanceId;
            InstanceName = instanceSettings.InstanceName;
            StartEngineImmediately = instanceSettings.IsImmediateEngineStartEnabled;
            EnableJobsDirectoryTracking = instanceSettings.IsJobsDirectoryTrackingEnabled;
            JobsDirectory = instanceSettings.JobsDirectory;

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SchedulerHub>();

            EngineStarted = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };
            EnginePaused = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };
            EngineTerminated = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };

            JobScheduled = (sender, e) => { hubContext.Clients.All.jobScheduled(e.Job.ToJobDetails()); };
            JobUnscheduled = (sender, e) => { hubContext.Clients.All.jobUnscheduled(e.Job.ToJobDetails()); };
            JobPaused = (sender, e) => { hubContext.Clients.All.jobUpdate(e.Job.ToJobDetails()); };
            JobResumed = (sender, e) => { hubContext.Clients.All.jobUpdate(e.Job.ToJobDetails()); };

            BeforeJobExecution = (sender, e) => { hubContext.Clients.All.jobUpdate(e.Job.ToJobDetails()); };
            JobExecutionSucceeded = (sender, e) => { hubContext.Clients.All.jobUpdate(e.Job.ToJobDetails()); };
            JobExecutionFailed = (sender, e) => { hubContext.Clients.All.jobUpdate(e.Job.ToJobDetails()); };
            JobExecutionSkipped = (sender, e) => { hubContext.Clients.All.jobUpdate(e.Job.ToJobDetails()); };
        }
    }
}