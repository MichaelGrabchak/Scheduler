using System.Data.Entity.ModelConfiguration;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration
{
    public class SettingConfiguration : EntityTypeConfiguration<Setting>
    {
        public SettingConfiguration()
        {
            HasKey(x => new { x.InstanceId, x.Key });

            Property(x => x.InstanceId).HasColumnName("InstanceId").IsRequired();
            Property(x => x.Key).HasColumnName("Key").IsRequired();
            Property(x => x.Value).HasColumnName("Value").IsOptional();

            HasRequired(x => x.Instance).WithMany().HasForeignKey(x => x.InstanceId);

            ToTable("Setting");
        }
    }
}
