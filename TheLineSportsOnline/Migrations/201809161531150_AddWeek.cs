namespace TheLineSportsOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWeek : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Weeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Games", "WeekId", c => c.Int(nullable: true));
            CreateIndex("dbo.Games", "WeekId");
            AddForeignKey("dbo.Games", "WeekId", "dbo.Weeks", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "WeekId", "dbo.Weeks");
            DropIndex("dbo.Games", new[] { "WeekId" });
            DropColumn("dbo.Games", "WeekId");
            DropTable("dbo.Weeks");
        }
    }
}
