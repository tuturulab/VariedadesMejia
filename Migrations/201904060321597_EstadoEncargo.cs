namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstadoEncargo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetalleProveedor", "Estado", c => c.String());
            DropColumn("dbo.DetalleProveedor", "EstadoEncargo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetalleProveedor", "EstadoEncargo", c => c.String());
            DropColumn("dbo.DetalleProveedor", "Estado");
        }
    }
}
