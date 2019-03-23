namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImeiBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "Imei_Disponible", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto", "Imei_Disponible");
        }
    }
}
