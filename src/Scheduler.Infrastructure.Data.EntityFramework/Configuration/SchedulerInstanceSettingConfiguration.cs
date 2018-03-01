using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration
{
    public class SchedulerInstanceSettingConfiguration : EntityTypeConfiguration<SchedulerInstanceSetting>
    {
        public SchedulerInstanceSettingConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(x => x.InstanceId).HasColumnName("INSTANCEID").IsRequired();
            Property(x => x.IsImmediateEngineStartEnabled).HasColumnName("ISIMMEDIATEENGINESTARTENABLED").IsRequired();
            Property(x => x.IsJobsDirectoryTrackingEnabled).HasColumnName("ISJOBSDIRECTORYTRACKINGENABLED").IsRequired();
            Property(x => x.JobsDirectory).HasColumnName("JOBSDIRECTORYPATH").IsOptional();

            HasRequired(x => x.Instance).WithMany().HasForeignKey(x => x.InstanceId);

            ToTable("SCHEDULERINSTANCESETTING");
        }
    }
}
