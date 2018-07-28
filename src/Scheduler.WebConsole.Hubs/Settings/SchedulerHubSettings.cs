using Microsoft.AspNet.SignalR;

using Scheduler.Core.Configurations;
using Scheduler.Core.Context;
using Scheduler.Engine;
using Scheduler.Engine.Extensions;

namespace Scheduler.WebConsole.Hubs.Settings
{
    public class SchedulerHubSettings : SchedulerSettings
    {
        public SchedulerHubSettings(IApplicationContext context, IEngineConfiguration configuration)
        {
            InstanceId = context.InstanceId.ToString();
            InstanceName = "<unregistered>";
            StartEngineImmediately = configuration.IsImmediateEngineStartEnabled;
            EnableJobsDirectoryTracking = configuration.IsJobsDirectoryTrackingEnabled;
            JobsDirectory = configuration.JobsDirectory;

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