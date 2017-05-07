namespace VoteAnalyzer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParsedFileEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParsedFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Path = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ParsedFiles");
        }
    }
}
