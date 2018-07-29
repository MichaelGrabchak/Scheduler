using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration
{
    public class JobDetailConfiguration : EntityTypeConfiguration<JobDetail>
    {
        public JobDetailConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(x => x.JobName).HasColumnName("JobName").IsRequired();
            Property(x => x.JobGroup).HasColumnName("JobGroup").IsRequired();
            Property(x => x.JobDescription).HasColumnName("JobDescription").IsOptional();
            Property(x => x.JobSchedule).HasColumnName("JobSchedule").IsOptional();
            Property(x => x.JobLastRunTime).HasColumnName("JobLastRunTime").IsOptional();
            Property(x => x.JobNextRunTime).HasColumnName("JobNextRunTime").IsOptional();
            Property(x => x.StatusId).HasColumnName("StatusId").IsRequired();
            Property(x => x.InstanceId).HasColumnName("InstanceId").IsRequired();

            HasRequired(x => x.Status).WithMany().HasForeignKey(x => x.StatusId).WillCascadeOnDelete(false);
            HasRequired(x => x.Instance).WithMany().HasForeignKey(x => x.InstanceId).WillCascadeOnDelete(false);

            ToTable("JobDetail");
        }
    }
}
