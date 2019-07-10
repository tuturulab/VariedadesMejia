namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estadoProveedor3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto_importado", "NombreProveedor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto_importado", "NombreProveedor");
        }
    }
}
