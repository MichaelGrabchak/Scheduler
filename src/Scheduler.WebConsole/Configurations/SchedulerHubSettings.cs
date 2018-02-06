using Scheduler.Domain.Data.Services;
using Scheduler.Domain.Entities;
using Scheduler.Infrastructure.Hubs;

using Microsoft.AspNet.SignalR;
using Scheduler.Engine;

namespace Scheduler.WebConsole.Configurations
{
    public class SchedulerHubSettings : SchedulerSettings
    {
        public SchedulerHubSettings(ISchedulerInstanceService schedulerInstanceService)
            : base()
        {
            var instanceSettings = schedulerInstanceService.GetSettings();

            InstanceId = instanceSettings.InstanceId;
            InstanceName = instanceSettings.InstanceName;
            StartEngineImmediately = instanceSettings.IsImmediateEngineStartEnabled;
            EnableJobsDirectoryTracking = instanceSettings.IsJobsDirectoryTrackingEnabled;

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SchedulerHub>();

            EngineStarted = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };
            EnginePaused = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };
            EngineTerminated = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };

            JobScheduled = (sender, e) => { hubContext.Clients.All.jobScheduled(JobDetails.Transform(e.Job)); };
            JobUnscheduled = (sender, e) => { hubContext.Clients.All.jobUnscheduled(JobDetails.Transform(e.Job)); };
            JobPaused = (sender, e) => { hubContext.Clients.All.jobUpdate(JobDetails.Transform(e.Job)); };
            JobResumed = (sender, e) => { hubContext.Clients.All.jobUpdate(JobDetails.Transform(e.Job)); };

            BeforeJobExecution = (sender, e) => { hubContext.Clients.All.jobUpdate(JobDetails.Transform(e.Job)); };
            JobExecutionSucceeded = (sender, e) => { hubContext.Clients.All.jobUpdate(JobDetails.Transform(e.Job)); };
            JobExecutionFailed = (sender, e) => { hubContext.Clients.All.jobUpdate(JobDetails.Transform(e.Job)); };
            JobExecutionSkipped = (sender, e) => { hubContext.Clients.All.jobUpdate(JobDetails.Transform(e.Job)); };
        }
    }
}