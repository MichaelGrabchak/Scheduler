using Scheduler.Domain.Entities;

namespace Scheduler.Domain.Services
{
    public interface ISchedulerManagerService
    {
        void Start();
        void Pause();
        void Shutdown();
        void PauseJob(string jobName, string jobGroup);
        void ResumeJob(string jobName, string jobGroup);
        void TriggerJob(string jobName, string jobGroup);
        JobsSummary GetJobsSummary();
        EngineDetails GetEngineDetails();
    }
}
