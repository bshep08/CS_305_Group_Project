namespace CS_305_Group_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAspNetUsersTable1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Role", c => c.String());
        }
    }
}
