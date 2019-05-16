namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Nombre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Nombre");
        }
    }
}
