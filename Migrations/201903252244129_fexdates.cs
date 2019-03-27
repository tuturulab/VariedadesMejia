namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fexdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "Garantia", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto", "Garantia");
        }
    }
}
