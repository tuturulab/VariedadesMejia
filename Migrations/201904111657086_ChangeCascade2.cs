namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCascade2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente");
            AddForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente", "IdCliente");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente");
            AddForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente", "IdCliente", cascadeDelete: true);
        }
    }
}
