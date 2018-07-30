using System.Linq;

using Scheduler.Domain.Services;
using Scheduler.Domain.Entities;
using Scheduler.Engine;
using Scheduler.Engine.Extensions;

namespace Scheduler.Infrastructure.Services
{
    public class SchedulerManagerService : ISchedulerManagerService
    {
        private readonly ISchedulerEngine _scheduler;

        public SchedulerManagerService(ISchedulerEngine scheduler)
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
            _scheduler.Stop();
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
            return new JobsSummary(_scheduler.GetAllJobs().Select(x => x.ToJobDetails()));
        }

        public EngineDetails GetEngineDetails()
        {
            return _scheduler.GetEngineInfo().ToEngineDetails();
        }
    }
}