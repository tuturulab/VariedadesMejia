namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Email = c.String(),
                        Domicilio = c.String(),
                        Tipo_Pago = c.String(),
                        Compania_IdCompania = c.Int(),
                    })
                .PrimaryKey(t => t.IdCliente)
                .ForeignKey("dbo.Compania", t => t.Compania_IdCompania)
                .Index(t => t.Compania_IdCompania);
            
            CreateTable(
                "dbo.Compania",
                c => new
                    {
                        IdCompania = c.Int(nullable: false, identity: true),
                        Trabajo = c.String(),
                        Fecha_Pago = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdCompania);
            
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        IdPedido = c.Int(nullable: false, identity: true),
                        Fecha_Pedido = c.DateTime(nullable: false),
                        Cantidad_Pedido = c.Int(nullable: false),
                        Fecha_Entrega = c.DateTime(),
                        Cliente_IdCliente = c.Int(),
                    })
                .PrimaryKey(t => t.IdPedido)
                .ForeignKey("dbo.Cliente", t => t.Cliente_IdCliente, cascadeDelete: true)
                .Index(t => t.Cliente_IdCliente);
            
            CreateTable(
                "dbo.Especificacion_pedido",
                c => new
                    {
                        IdEspecificacion_Pedido = c.Int(nullable: false, identity: true),
                        Tipo_Producto = c.String(),
                        Marca = c.String(),
                        Modelo = c.String(),
                        Descripcion = c.String(),
                        Cantidad = c.String(),
                        Pedido_IdPedido = c.Int(),
                        Proveedor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.IdEspecificacion_Pedido)
                .ForeignKey("dbo.Pedido", t => t.Pedido_IdPedido)
                .ForeignKey("dbo.Proveedor", t => t.Proveedor_Id)
                .Index(t => t.Pedido_IdPedido)
                .Index(t => t.Proveedor_Id);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Empresa = c.String(),
                        Precio_Costo = c.Double(nullable: false),
                        Lugar_Importacion = c.String(),
                        Fecha_Llegada = c.DateTime(),
                        Garantia_Original = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Proveedor_producto",
                c => new
                    {
                        Idproveedor_producto = c.Int(nullable: false, identity: true),
                        Cantidad_Recibida = c.Int(nullable: false),
                        Numero_Seguimiento = c.String(),
                        Producto_IdProducto = c.Int(),
                        Proveedor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Idproveedor_producto)
                .ForeignKey("dbo.Producto", t => t.Producto_IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Proveedor", t => t.Proveedor_Id)
                .Index(t => t.Producto_IdProducto)
                .Index(t => t.Proveedor_Id);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Precio_Venta = c.Double(nullable: false),
                        Marca = c.String(),
                        Tipo_Producto = c.String(),
                        Modelo = c.String(),
                        Cantidad_Disponible = c.Int(nullable: false),
                        Credito_Disponible = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProducto);
            
            CreateTable(
                "dbo.Especificacion_producto",
                c => new
                    {
                        IdEspecificaciones_Producto = c.Int(nullable: false, identity: true),
                        Garantia = c.String(),
                        IMEI = c.String(),
                        Venta_IdVenta = c.Int(),
                        Producto_IdProducto = c.Int(),
                    })
                .PrimaryKey(t => t.IdEspecificaciones_Producto)
                .ForeignKey("dbo.Venta", t => t.Venta_IdVenta)
                .ForeignKey("dbo.Producto", t => t.Producto_IdProducto, cascadeDelete: true)
                .Index(t => t.Venta_IdVenta)
                .Index(t => t.Producto_IdProducto);
            
            CreateTable(
                "dbo.Venta",
                c => new
                    {
                        IdVenta = c.Int(nullable: false, identity: true),
                        Fecha_Venta = c.DateTime(nullable: false),
                        Orden_Pagare = c.String(),
                        Tipo_Venta = c.String(),
                        Cantidad = c.Int(nullable: false),
                        Cliente_IdCliente = c.Int(),
                    })
                .PrimaryKey(t => t.IdVenta)
                .ForeignKey("dbo.Cliente", t => t.Cliente_IdCliente, cascadeDelete: true)
                .Index(t => t.Cliente_IdCliente);
            
            CreateTable(
                "dbo.Pago",
                c => new
                    {
                        IdPago = c.Int(nullable: false, identity: true),
                        Monto = c.Double(nullable: false),
                        Plazo = c.DateTime(),
                        Fecha_Pago = c.DateTime(nullable: false),
                        Venta_IdVenta = c.Int(),
                    })
                .PrimaryKey(t => t.IdPago)
                .ForeignKey("dbo.Venta", t => t.Venta_IdVenta)
                .Index(t => t.Venta_IdVenta);
            
            CreateTable(
                "dbo.Telefono",
                c => new
                    {
                        IdTelefono = c.Int(nullable: false, identity: true),
                        Numero = c.String(),
                        Tipo_Numero = c.String(),
                        Empresa = c.String(),
                        Cliente_IdCliente = c.Int(),
                    })
                .PrimaryKey(t => t.IdTelefono)
                .ForeignKey("dbo.Cliente", t => t.Cliente_IdCliente, cascadeDelete: true)
                .Index(t => t.Cliente_IdCliente);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Venta", "Cliente_IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.Telefono", "Cliente_IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.Pedido", "Cliente_IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.Proveedor_producto", "Proveedor_Id", "dbo.Proveedor");
            DropForeignKey("dbo.Proveedor_producto", "Producto_IdProducto", "dbo.Producto");
            DropForeignKey("dbo.Especificacion_producto", "Producto_IdProducto", "dbo.Producto");
            DropForeignKey("dbo.Pago", "Venta_IdVenta", "dbo.Venta");
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            DropForeignKey("dbo.Especificacion_pedido", "Proveedor_Id", "dbo.Proveedor");
            DropForeignKey("dbo.Especificacion_pedido", "Pedido_IdPedido", "dbo.Pedido");
            DropForeignKey("dbo.Cliente", "Compania_IdCompania", "dbo.Compania");
            DropIndex("dbo.Telefono", new[] { "Cliente_IdCliente" });
            DropIndex("dbo.Pago", new[] { "Venta_IdVenta" });
            DropIndex("dbo.Venta", new[] { "Cliente_IdCliente" });
            DropIndex("dbo.Especificacion_producto", new[] { "Producto_IdProducto" });
            DropIndex("dbo.Especificacion_producto", new[] { "Venta_IdVenta" });
            DropIndex("dbo.Proveedor_producto", new[] { "Proveedor_Id" });
            DropIndex("dbo.Proveedor_producto", new[] { "Producto_IdProducto" });
            DropIndex("dbo.Especificacion_pedido", new[] { "Proveedor_Id" });
            DropIndex("dbo.Especificacion_pedido", new[] { "Pedido_IdPedido" });
            DropIndex("dbo.Pedido", new[] { "Cliente_IdCliente" });
            DropIndex("dbo.Cliente", new[] { "Compania_IdCompania" });
            DropTable("dbo.Telefono");
            DropTable("dbo.Pago");
            DropTable("dbo.Venta");
            DropTable("dbo.Especificacion_producto");
            DropTable("dbo.Producto");
            DropTable("dbo.Proveedor_producto");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Especificacion_pedido");
            DropTable("dbo.Pedido");
            DropTable("dbo.Compania");
            DropTable("dbo.Cliente");
        }
    }
}
