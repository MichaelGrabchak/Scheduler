using System.Data.Entity;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration.Context
{
    public class SchedulerDbContext : DbContext
    {
        public SchedulerDbContext(string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<SchedulerDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new JobStatusConfiguration());
            modelBuilder.Configurations.Add(new JobDetailConfiguration());
            modelBuilder.Configurations.Add(new SchedulerInstanceConfiguration());
            modelBuilder.Configurations.Add(new SchedulerInstanceSettingConfiguration());
        }
    }
}
