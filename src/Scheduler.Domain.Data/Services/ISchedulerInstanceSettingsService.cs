using Scheduler.Domain.Data.Dto;

namespace Scheduler.Domain.Data.Services
{
    public interface ISchedulerInstanceService
    {
        InstanceSettings GetSettings();
    }
}
