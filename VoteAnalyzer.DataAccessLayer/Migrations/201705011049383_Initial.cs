namespace VoteAnalyzer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deputies",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Sessions",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false),
                    DateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.VottingSessions",
                    c => new
                    {
                        Id = c.Guid(nullable: false),
                        Subject = c.String(nullable: false),
                        Number = c.Int(),
                        SessionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", c => c.SessionId)
                .Index(c => c.SessionId);

            CreateTable(
                    "dbo.Votes",
                    c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeputyId = c.Guid(nullable: false),
                        VoteAction = c.Int(nullable: false),
                        VottingSessionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deputies", c => c.DeputyId)
                .ForeignKey("dbo.VottingSessions", c => c.VottingSessionId)
                .Index(c => c.DeputyId)
                .Index(c => c.VottingSessionId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.VottingSessions", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.Votes", "DeputyId", "dbo.Deputies");
            DropForeignKey("dbo.Votes", "VottingSessionId", "dbo.VottingSessions");
            DropIndex("dbo.VottingSessions", new[] { "SessionId" });
            DropIndex("dbo.Votes", new[] { "DeputyId" });
            DropIndex("dbo.Votes", new[] { "VottingSessionId" });
            DropTable("dbo.VottingSessions");
            DropTable("dbo.Votes");
            DropTable("dbo.Sessions");
            DropTable("dbo.Deputies");
        }
    }
}
