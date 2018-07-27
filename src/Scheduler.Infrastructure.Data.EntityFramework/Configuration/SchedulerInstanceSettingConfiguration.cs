using System.Data.Entity.ModelConfiguration;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration
{
    public class SchedulerInstanceSettingConfiguration : EntityTypeConfiguration<SchedulerInstanceSetting>
    {
        public SchedulerInstanceSettingConfiguration()
        {
            HasKey(x => new { x.InstanceId, x.Key });

            Property(x => x.InstanceId).HasColumnName("INSTANCEID").IsRequired();
            Property(x => x.Key).HasColumnName("KEY").IsRequired();
            Property(x => x.Value).HasColumnName("VALUE").IsOptional();

            HasRequired(x => x.Instance).WithMany().HasForeignKey(x => x.InstanceId);

            ToTable("SCHEDULERINSTANCESETTING");
        }
    }
}
