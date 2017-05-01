using System.Data.Entity;

namespace VoteAnalyzer.DataAccessLayer.DbContexts
{
    public class MainDbContext : DbContext
    {
        static MainDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MainDbContext, MigrateDbConfiguration>());
        }

        public MainDbContext()
        {
        }

        public MainDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
