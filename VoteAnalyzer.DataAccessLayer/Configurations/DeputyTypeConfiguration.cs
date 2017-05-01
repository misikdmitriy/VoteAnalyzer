using System.Data.Entity.ModelConfiguration;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Configurations
{
    public class DeputyTypeConfiguration : EntityTypeConfiguration<Deputy>
    {
        public DeputyTypeConfiguration()
        {
            this.HasKey(d => d.Id);
            this.Property(d => d.Name).IsRequired();
        }
    }
}
