using Microsoft.AspNet.SignalR;

using Scheduler.Core.Engine;
using Scheduler.Domain.Entities;
using Scheduler.Infrastructure.Hubs;

namespace Scheduler.Infrastructure.Configuration
{
    public class SchedulerHubSettings : SchedulerSettings
    {
        public SchedulerHubSettings()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SchedulerHub>();

            EngineStarted = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };
            EnginePaused = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };
            EngineTerminated = (sender, e) => { hubContext.Clients.All.changeState(e.State.ToString()); };

            JobScheduled = (sender, e) => { hubContext.Clients.All.jobScheduled(JobDetails.Transform(e.Job)); };
            JobUnscheduled = (sender, e) => { hubContext.Clients.All.jobUnscheduled(JobDetails.Transform(e.Job)); };
            JobPaused = (sender, e) => { hubContext.Clients.All.changeJobState(JobDetails.Transform(e.Job)); };
            JobResumed = (sender, e) => { hubContext.Clients.All.changeJobState(JobDetails.Transform(e.Job)); };
            JobTriggered = (sender, e) =>
            {
                /*hubContext.Clients.All.changeJobState(e.JobName, e.JobGroup, JobState.Triggered.ToString());*/
            };
        }
    }
}