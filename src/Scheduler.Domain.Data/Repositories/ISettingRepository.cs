using System.Collections.Generic;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Domain.Data.Repositories
{
    public interface ISettingRepository : IRepository<Setting, int>
    {
        IList<Setting> GetInstanceSettings();
    }
}
