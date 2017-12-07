namespace CS_305_Group_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAspNetUsersTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ReportDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "DateSetter", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateSetter");
            DropColumn("dbo.AspNetUsers", "ReportDate");
        }
    }
}
