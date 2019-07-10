namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Code", c => c.String());
            AddColumn("dbo.User", "ValidUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "ValidUntil");
            DropColumn("dbo.User", "Code");
        }
    }
}
