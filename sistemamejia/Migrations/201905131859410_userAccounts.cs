namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userAccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}
