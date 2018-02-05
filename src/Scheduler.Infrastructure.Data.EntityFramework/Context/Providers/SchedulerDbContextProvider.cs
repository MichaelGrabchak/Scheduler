using System.Data.Entity;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration.Context.Providers
{
    public class SchedulerDbContextProvider : IDbContextProvider
    {
        public DbContext DbContext { get; private set; }

        public SchedulerDbContextProvider(ISchedulerContext context)
        {
            DbContext = new SchedulerDbContext(context.ConnectionString);
        }
    }
}
