namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportacionProductos3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto_importado", "Cantidad", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto_importado", "Cantidad");
        }
    }
}
