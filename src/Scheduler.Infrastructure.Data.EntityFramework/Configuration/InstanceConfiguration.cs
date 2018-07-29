using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Infrastructure.Data.EntityFramework.Configuration
{
    public class InstanceConfiguration : EntityTypeConfiguration<Instance>
    {
        public InstanceConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();

            ToTable("Instance");
        }
    }
}
