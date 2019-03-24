namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadedeletepedido : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Especificacion_pedido", "Pedido_IdPedido", "dbo.Pedido");
            AddForeignKey("dbo.Especificacion_pedido", "Pedido_IdPedido", "dbo.Pedido", "IdPedido", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Especificacion_pedido", "Pedido_IdPedido", "dbo.Pedido");
            AddForeignKey("dbo.Especificacion_pedido", "Pedido_IdPedido", "dbo.Pedido", "IdPedido");
        }
    }
}
