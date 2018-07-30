using Microsoft.AspNet.SignalR;

using Scheduler.Domain.Entities;
using Scheduler.Domain.Services;

namespace Scheduler.WebConsole.Hubs
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

        public EngineDetails GetEngineInfo()
        {
            return _scheduler.GetEngineDetails();
        }
    }
}