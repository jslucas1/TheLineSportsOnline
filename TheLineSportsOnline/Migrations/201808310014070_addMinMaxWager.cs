namespace TheLineSportsOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMinMaxWager : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MinWager", c => c.Long(nullable: false));
            AddColumn("dbo.AspNetUsers", "MaxWager", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MaxWager");
            DropColumn("dbo.AspNetUsers", "MinWager");
        }
    }
}
