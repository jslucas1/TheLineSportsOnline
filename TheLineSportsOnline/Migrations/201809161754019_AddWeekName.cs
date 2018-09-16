namespace TheLineSportsOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWeekName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Weeks", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Weeks", "Name");
        }
    }
}
