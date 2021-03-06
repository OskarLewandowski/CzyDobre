namespace CzyDobre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNickName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NickName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 64));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 64));
            DropColumn("dbo.AspNetUsers", "NickName");
        }
    }
}
