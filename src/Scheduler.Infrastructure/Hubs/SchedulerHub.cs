using Scheduler.Domain.Services;
using Scheduler.Domain.Entities;

using Microsoft.AspNet.SignalR;

namespace Scheduler.Infrastructure.Hubs
{
    public class SchedulerHub : Hub
    {
        private readonly ISchedulerManagerService _scheduler;

        public SchedulerHub(ISchedulerManagerService scheduler)
        {
            _scheduler = scheduler;
        }

        public void Start()
        {
            _scheduler.Start();
        }

        public void Pause()
        {
            _scheduler.Pause();
        }

        public void Shutdown()
        {
            _scheduler.Shutdown();
        }

        public void PauseJob(string jobName, string jobGroup)
        {
            _scheduler.PauseJob(jobName, jobGroup);
        }

        public void ResumeJob(string jobName, string jobGroup)
        {
            _scheduler.ResumeJob(jobName, jobGroup);
        }

        public void TriggerJob(string jobName, string jobGroup)
        {
            _scheduler.TriggerJob(jobName, jobGroup);
        }

        public JobsSummary GetJobsSummary()
        {
            return _scheduler.GetJobsSummary();
        }

        public EngineInfo GetEngineInfo()
        {
            return _scheduler.GetEngineInfo();
        }
    }
}