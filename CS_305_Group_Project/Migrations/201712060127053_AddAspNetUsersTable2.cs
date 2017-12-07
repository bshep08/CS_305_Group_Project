namespace CS_305_Group_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAspNetUsersTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Role");
        }
    }
}
