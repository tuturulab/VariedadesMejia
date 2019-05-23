namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascademayorqueall : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            AddForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente", "IdCliente", cascadeDelete: true);
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            DropForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente");
            AddForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta", "IdVenta");
            AddForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente", "IdCliente");
        }
    }
}
