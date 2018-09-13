namespace TheLineSportsOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserLockedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Locked");
        }
    }
}
