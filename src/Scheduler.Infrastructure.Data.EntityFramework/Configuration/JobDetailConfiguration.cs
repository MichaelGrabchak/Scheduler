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

            Property(x => x.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(x => x.JobName).HasColumnName("JOBNAME").IsRequired();
            Property(x => x.JobGroup).HasColumnName("JOBGROUP").IsRequired();
            Property(x => x.JobDescription).HasColumnName("JOBDESCRIPTION").IsOptional();
            Property(x => x.JobSchedule).HasColumnName("JOBSCHEDULE").IsOptional();
            Property(x => x.JobLastRunTime).HasColumnName("JOBLASTRUNTIME").IsOptional();
            Property(x => x.JobNextRunTime).HasColumnName("JOBNEXTRUNTIME").IsOptional();
            Property(x => x.StatusId).HasColumnName("STATUSID").IsRequired();
            Property(x => x.InstanceId).HasColumnName("INSTANCEID").IsRequired();

            HasRequired(x => x.Status).WithMany().HasForeignKey(x => x.StatusId).WillCascadeOnDelete(false);
            HasRequired(x => x.Instance).WithMany().HasForeignKey(x => x.InstanceId).WillCascadeOnDelete(false);

            ToTable("JOBDETAIL");
        }
    }
}
