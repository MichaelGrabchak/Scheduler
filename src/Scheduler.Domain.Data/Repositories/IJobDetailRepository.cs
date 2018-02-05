using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Domain.Data.Repositories
{
    public interface IJobDetailRepository : IRepository<JobDetail, int>
    {
        JobDetail GetJobDetail(string name, string group);
    }
}
