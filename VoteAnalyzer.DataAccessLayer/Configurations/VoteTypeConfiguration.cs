using System.Data.Entity.ModelConfiguration;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Configurations
{
    public class VoteTypeConfiguration : EntityTypeConfiguration<Vote>
    {
        public VoteTypeConfiguration()
        {
            this.HasKey(v => v.Id);
            this.Property(v => v.DeputyId).IsRequired();
            this.Property(v => v.VottingSessionId).IsRequired();
            this.Property(v => v.KnownVoteId).IsRequired();
        }
    }
}
