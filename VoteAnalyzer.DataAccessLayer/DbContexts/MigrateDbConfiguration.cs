using System.Data.Entity.Migrations;

namespace VoteAnalyzer.DataAccessLayer.DbContexts
{
    public class MigrateDbConfiguration : DbMigrationsConfiguration<MainDbContext>
    {
        public MigrateDbConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }
    }
}
