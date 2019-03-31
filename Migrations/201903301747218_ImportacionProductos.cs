namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportacionProductos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Producto_importado",
                c => new
                    {
                        IdProducto_importado = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Cantidad = c.Int(nullable: false),
                        DetalleProveedor_IdDetalleProveedor = c.Int(),
                    })
                .PrimaryKey(t => t.IdProducto_importado)
                .ForeignKey("dbo.DetalleProveedor", t => t.DetalleProveedor_IdDetalleProveedor)
                .Index(t => t.DetalleProveedor_IdDetalleProveedor);
            
            AddColumn("dbo.Pedido", "Estado_Pedido", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Producto_importado", "DetalleProveedor_IdDetalleProveedor", "dbo.DetalleProveedor");
            DropIndex("dbo.Producto_importado", new[] { "DetalleProveedor_IdDetalleProveedor" });
            DropColumn("dbo.Pedido", "Estado_Pedido");
            DropTable("dbo.Producto_importado");
        }
    }
}
