using System.Data.Entity.ModelConfiguration;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Configurations
{
    public class KnownVoteTypeConfiguration : EntityTypeConfiguration<KnownVote>
    {
        public KnownVoteTypeConfiguration()
        {
            this.HasKey(v => v.Id);
            this.Property(v => v.Vote).IsRequired();
        }
    }
}
