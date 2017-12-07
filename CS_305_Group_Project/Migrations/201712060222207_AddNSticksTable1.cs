namespace CS_305_Group_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNSticksTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NSticks", "ComparableDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.NSticks", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NSticks", "Location");
            DropColumn("dbo.NSticks", "ComparableDate");
        }
    }
}
