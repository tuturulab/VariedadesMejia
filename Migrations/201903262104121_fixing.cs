namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "Cedula", c => c.String());
            DropColumn("dbo.Pago", "Cancelado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pago", "Cancelado", c => c.String());
            DropColumn("dbo.Cliente", "Cedula");
        }
    }
}
