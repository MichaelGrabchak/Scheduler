using System.Data.Entity;

namespace Scheduler.Domain.Data.EntityFramework.ContextProviders
{
    public interface IDbContextProvider
    {
        DbContext DbContext { get; }
    }
}
