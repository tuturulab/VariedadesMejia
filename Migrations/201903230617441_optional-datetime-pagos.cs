namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class optionaldatetimepagos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pago", "Fecha_Pago", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pago", "Fecha_Pago", c => c.DateTime(nullable: false));
        }
    }
}
