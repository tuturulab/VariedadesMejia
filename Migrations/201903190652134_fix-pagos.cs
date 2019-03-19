namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixpagos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Venta", "Plazo", c => c.String());
            DropColumn("dbo.Venta", "Cantidad");
            DropColumn("dbo.Pago", "Plazo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pago", "Plazo", c => c.DateTime());
            AddColumn("dbo.Venta", "Cantidad", c => c.Int(nullable: false));
            DropColumn("dbo.Venta", "Plazo");
        }
    }
}
