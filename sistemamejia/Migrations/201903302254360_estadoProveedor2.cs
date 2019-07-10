namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estadoProveedor2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetalleProveedor", "Numero_Seguimiento", c => c.String());
            DropColumn("dbo.Proveedor_producto", "Numero_Seguimiento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proveedor_producto", "Numero_Seguimiento", c => c.String());
            DropColumn("dbo.DetalleProveedor", "Numero_Seguimiento");
        }
    }
}
