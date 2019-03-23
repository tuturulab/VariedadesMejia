namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpagos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Venta", "MontoVenta", c => c.Double(nullable: false));
            AddColumn("dbo.Pago", "Cancelado", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pago", "Cancelado");
            DropColumn("dbo.Venta", "MontoVenta");
        }
    }
}
