using System.Data.Entity.ModelConfiguration;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Configurations
{
    public class ParsedFileTypeConfiguration : EntityTypeConfiguration<ParsedFile>
    {
        public ParsedFileTypeConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Path).IsRequired();
        }
    }
}
