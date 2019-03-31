namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportacionProductos2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto_importado", "Precio", c => c.Double(nullable: false));
            DropColumn("dbo.Producto_importado", "Cantidad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Producto_importado", "Cantidad", c => c.Int(nullable: false));
            DropColumn("dbo.Producto_importado", "Precio");
        }
    }
}
