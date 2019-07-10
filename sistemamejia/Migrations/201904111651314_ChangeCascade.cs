namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            AddColumn("dbo.Especificacion_producto", "Vendido", c => c.String());
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            DropColumn("dbo.Especificacion_producto", "Vendido");
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta", cascadeDelete: true);
        }
    }
}
