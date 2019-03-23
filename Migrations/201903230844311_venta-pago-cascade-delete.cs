namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ventapagocascadedelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pago", "Venta_IdVenta", "dbo.Venta");
            AddForeignKey("dbo.Pago", "Venta_IdVenta", "dbo.Venta", "IdVenta", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pago", "Venta_IdVenta", "dbo.Venta");
            AddForeignKey("dbo.Pago", "Venta_IdVenta", "dbo.Venta", "IdVenta");
        }
    }
}
