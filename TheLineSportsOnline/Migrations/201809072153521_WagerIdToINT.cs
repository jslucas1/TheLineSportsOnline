namespace TheLineSportsOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WagerIdToINT : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Wagers");
            AlterColumn("dbo.Wagers", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Wagers", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Wagers");
            AlterColumn("dbo.Wagers", "Id", c => c.Byte(nullable: false, identity: true));
            AddPrimaryKey("dbo.Wagers", "Id");
        }
    }
}
