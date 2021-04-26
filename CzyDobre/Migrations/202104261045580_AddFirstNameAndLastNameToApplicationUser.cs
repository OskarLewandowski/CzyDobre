namespace CzyDobre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstNameAndLastNameToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 64));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
