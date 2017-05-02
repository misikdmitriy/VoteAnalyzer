using System.Data.Entity;

using VoteAnalyzer.DataAccessLayer.Configurations;
using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Migrations;

namespace VoteAnalyzer.DataAccessLayer.DbContexts
{
    public class MainDbContext : DbContext
    {
        static MainDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MainDbContext, Configuration>());
        }

        public IDbSet<Deputy> Deputies { get; set; }
        public IDbSet<Session> Sessions { get; set; }
        public IDbSet<Vote> Votes { get; set; }
        public IDbSet<KnownVote> KnownVotes { get; set; }
        public IDbSet<VottingSession> VottingSessions { get; set; }

        public MainDbContext()
            : this("MainDbContext")
        {
        }

        public MainDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DeputyTypeConfiguration());
            modelBuilder.Configurations.Add(new SessionTypeConfiguration());
            modelBuilder.Configurations.Add(new VoteTypeConfiguration());
            modelBuilder.Configurations.Add(new VottingSessionTypeConfiguration());
            modelBuilder.Configurations.Add(new KnownVoteTypeConfiguration());
        }
    }
}
