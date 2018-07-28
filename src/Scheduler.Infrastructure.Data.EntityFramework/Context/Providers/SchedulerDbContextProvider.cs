using System.Data.Entity;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;

namespace Scheduler.Infrastructure.Data.EntityFramework.Context.Providers
{
    public class SchedulerDbContextProvider : IDbContextProvider
    {
        public DbContext DbContext { get; }

        public SchedulerDbContextProvider(IDataWarehouseContext dwhContext)
        {
            DbContext = new SchedulerDbContext(dwhContext.ConnectionString);
        }
    }
}
