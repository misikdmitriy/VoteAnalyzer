using System.Data.Entity.ModelConfiguration;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Configurations
{
    public class SessionTypeConfiguration : EntityTypeConfiguration<Session>
    {
        public SessionTypeConfiguration()
        {
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired();
            this.Property(s => s.DateTime).IsRequired();
        }
    }
}
