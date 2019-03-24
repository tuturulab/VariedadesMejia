namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class client : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Pedido", new[] { "Cliente_IdCliente" });
            CreateIndex("dbo.Pedido", "cliente_IdCliente");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pedido", new[] { "cliente_IdCliente" });
            CreateIndex("dbo.Pedido", "Cliente_IdCliente");
        }
    }
}
