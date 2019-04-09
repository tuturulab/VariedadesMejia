namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingupdateschange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DetalleProveedor", "Proveedor_IdProveedor", "dbo.Proveedor");
            DropIndex("dbo.DetalleProveedor", new[] { "Proveedor_IdProveedor" });
            AddColumn("dbo.Especificacion_producto", "PrecioCosto", c => c.Double(nullable: false));
            AddColumn("dbo.Especificacion_producto", "Garantia_Original", c => c.DateTime());
            AddColumn("dbo.Especificacion_producto", "Proveedor_IdProveedor", c => c.Int());
            CreateIndex("dbo.Especificacion_producto", "Proveedor_IdProveedor");
            AddForeignKey("dbo.Especificacion_producto", "Proveedor_IdProveedor", "dbo.Proveedor", "IdProveedor");
            DropColumn("dbo.DetalleProveedor", "Garantia_Original");
            DropColumn("dbo.DetalleProveedor", "Proveedor_IdProveedor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetalleProveedor", "Proveedor_IdProveedor", c => c.Int());
            AddColumn("dbo.DetalleProveedor", "Garantia_Original", c => c.DateTime());
            DropForeignKey("dbo.Especificacion_producto", "Proveedor_IdProveedor", "dbo.Proveedor");
            DropIndex("dbo.Especificacion_producto", new[] { "Proveedor_IdProveedor" });
            DropColumn("dbo.Especificacion_producto", "Proveedor_IdProveedor");
            DropColumn("dbo.Especificacion_producto", "Garantia_Original");
            DropColumn("dbo.Especificacion_producto", "PrecioCosto");
            CreateIndex("dbo.DetalleProveedor", "Proveedor_IdProveedor");
            AddForeignKey("dbo.DetalleProveedor", "Proveedor_IdProveedor", "dbo.Proveedor", "IdProveedor");
        }
    }
}
