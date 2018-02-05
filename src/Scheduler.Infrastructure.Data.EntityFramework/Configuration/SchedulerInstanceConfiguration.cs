using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration
{
    public class SchedulerInstanceConfiguration : EntityTypeConfiguration<SchedulerInstance>
    {
        public SchedulerInstanceConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(x => x.InstanceName).HasColumnName("INSTANCENAME").IsRequired();

            ToTable("SCHEDULERINSTANCE");
        }
    }
}
