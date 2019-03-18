namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class diaspago : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "Fecha_Pago_1", c => c.Int(nullable: false));
            AddColumn("dbo.Cliente", "Fecha_Pago_2", c => c.Int(nullable: false));
            DropColumn("dbo.Cliente", "Fecha_Pago");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "Fecha_Pago", c => c.DateTime());
            DropColumn("dbo.Cliente", "Fecha_Pago_2");
            DropColumn("dbo.Cliente", "Fecha_Pago_1");
        }
    }
}
