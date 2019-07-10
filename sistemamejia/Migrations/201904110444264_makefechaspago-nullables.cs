namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makefechaspagonullables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cliente", "Fecha_Pago_1", c => c.Int());
            AlterColumn("dbo.Cliente", "Fecha_Pago_2", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cliente", "Fecha_Pago_2", c => c.Int(nullable: false));
            AlterColumn("dbo.Cliente", "Fecha_Pago_1", c => c.Int(nullable: false));
        }
    }
}
