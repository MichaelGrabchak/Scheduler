using Scheduler.Domain.Data.Dto;

namespace Scheduler.Domain.Data.Services
{
    public interface IJobDetailService
    {
        JobDetail GetJobDetail(string jobName, string jobGroup);
        void UpdateJobDetail(JobDetail jobDetail);
    }
}
