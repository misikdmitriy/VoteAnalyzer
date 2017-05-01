using System.Data.Entity.ModelConfiguration;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Configurations
{
    public class VottingSessionTypeConfiguration : EntityTypeConfiguration<VottingSession>
    {
        public VottingSessionTypeConfiguration()
        {
            this.HasKey(v => v.Id);
            this.Property(v => v.Subject).IsRequired();
            this.Property(v => v.Number).IsOptional();
            this.Property(v => v.SessionId).IsRequired();
        }
    }
}
