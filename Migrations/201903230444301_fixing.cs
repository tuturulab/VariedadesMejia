namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Venta", "VentaCompletada", c => c.String());
            DropColumn("dbo.Venta", "Plazo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Venta", "Plazo", c => c.String());
            DropColumn("dbo.Venta", "VentaCompletada");
        }
    }
}
