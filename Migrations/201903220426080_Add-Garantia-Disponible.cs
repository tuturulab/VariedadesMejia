namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGarantiaDisponible : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "Garantia_Disponible", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto", "Garantia_Disponible");
        }
    }
}
