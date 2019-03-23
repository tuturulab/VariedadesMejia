namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadedeleteproducts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Proveedor_producto", "Idproveedor_producto", "dbo.DetalleProveedor");
            DropForeignKey("dbo.Especificacion_producto", "IdEspecificaciones_Producto", "dbo.Proveedor_producto");
            AddForeignKey("dbo.Proveedor_producto", "Idproveedor_producto", "dbo.DetalleProveedor", "IdDetalleProveedor", cascadeDelete: true);
            AddForeignKey("dbo.Especificacion_producto", "IdEspecificaciones_Producto", "dbo.Proveedor_producto", "Idproveedor_producto", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Especificacion_producto", "IdEspecificaciones_Producto", "dbo.Proveedor_producto");
            DropForeignKey("dbo.Proveedor_producto", "Idproveedor_producto", "dbo.DetalleProveedor");
            AddForeignKey("dbo.Especificacion_producto", "IdEspecificaciones_Producto", "dbo.Proveedor_producto", "Idproveedor_producto");
            AddForeignKey("dbo.Proveedor_producto", "Idproveedor_producto", "dbo.DetalleProveedor", "IdDetalleProveedor");
        }
    }
}
