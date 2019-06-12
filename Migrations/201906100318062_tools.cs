namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tools : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            AddForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente", "IdCliente");
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            DropForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente");
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta", cascadeDelete: true);
            AddForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente", "IdCliente", cascadeDelete: true);
        }
    }
}
