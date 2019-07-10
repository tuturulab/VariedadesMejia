namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estadoProveedor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetalleProveedor", "EstadoEncargo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetalleProveedor", "EstadoEncargo");
        }
    }
}
