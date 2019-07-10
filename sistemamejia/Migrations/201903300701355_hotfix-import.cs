namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotfiximport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Especificacion_pedido", "DetalleProveedor_IdDetalleProveedor", "dbo.DetalleProveedor");
            DropIndex("dbo.Especificacion_pedido", new[] { "DetalleProveedor_IdDetalleProveedor" });
            AddColumn("dbo.DetalleProveedor", "Pedido_IdPedido", c => c.Int());
            CreateIndex("dbo.DetalleProveedor", "Pedido_IdPedido");
            AddForeignKey("dbo.DetalleProveedor", "Pedido_IdPedido", "dbo.Pedido", "IdPedido");
            DropColumn("dbo.Especificacion_pedido", "DetalleProveedor_IdDetalleProveedor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Especificacion_pedido", "DetalleProveedor_IdDetalleProveedor", c => c.Int());
            DropForeignKey("dbo.DetalleProveedor", "Pedido_IdPedido", "dbo.Pedido");
            DropIndex("dbo.DetalleProveedor", new[] { "Pedido_IdPedido" });
            DropColumn("dbo.DetalleProveedor", "Pedido_IdPedido");
            CreateIndex("dbo.Especificacion_pedido", "DetalleProveedor_IdDetalleProveedor");
            AddForeignKey("dbo.Especificacion_pedido", "DetalleProveedor_IdDetalleProveedor", "dbo.DetalleProveedor", "IdDetalleProveedor");
        }
    }
}
