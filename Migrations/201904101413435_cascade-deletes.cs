namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadedeletes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Producto_importado", "DetalleProveedor_IdDetalleProveedor", "dbo.DetalleProveedor");
            DropForeignKey("dbo.Especificacion_producto", "Proveedor_IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            AddForeignKey("dbo.Producto_importado", "DetalleProveedor_IdDetalleProveedor", "dbo.DetalleProveedor", "IdDetalleProveedor", cascadeDelete: true);
            AddForeignKey("dbo.Especificacion_producto", "Proveedor_IdProveedor", "dbo.Proveedor", "IdProveedor", cascadeDelete: true);
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            DropForeignKey("dbo.Especificacion_producto", "Proveedor_IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.Producto_importado", "DetalleProveedor_IdDetalleProveedor", "dbo.DetalleProveedor");
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta");
            AddForeignKey("dbo.Especificacion_producto", "Proveedor_IdProveedor", "dbo.Proveedor", "IdProveedor");
            AddForeignKey("dbo.Producto_importado", "DetalleProveedor_IdDetalleProveedor", "dbo.DetalleProveedor", "IdDetalleProveedor");
        }
    }
}
