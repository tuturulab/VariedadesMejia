namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        Compania = c.String(),
                        Fecha_Pago_1 = c.Int(nullable: false),
                        Fecha_Pago_2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        IdPedido = c.Int(nullable: false, identity: true),
                        Fecha_Pedido = c.DateTime(nullable: false),
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
                        IdEspecificacion_Pedido = c.Int(nullable: false),
                        Tipo_Producto = c.String(),
                        Marca = c.String(),
                        Modelo = c.String(),
                        Descripcion = c.String(),
                        Cantidad = c.String(),
                        Pedido_IdPedido = c.Int(),
                    })
                .PrimaryKey(t => t.IdEspecificacion_Pedido)
                .ForeignKey("dbo.DetalleProveedor", t => t.IdEspecificacion_Pedido)
                .ForeignKey("dbo.Pedido", t => t.Pedido_IdPedido)
                .Index(t => t.IdEspecificacion_Pedido)
                .Index(t => t.Pedido_IdPedido);
            
            CreateTable(
                "dbo.DetalleProveedor",
                c => new
                    {
                        IdDetalleProveedor = c.Int(nullable: false, identity: true),
                        Precio_Costo = c.Double(nullable: false),
                        Fecha_Llegada = c.DateTime(),
                        Garantia_Original = c.DateTime(),
                        Proveedor_IdProveedor = c.Int(),
                    })
                .PrimaryKey(t => t.IdDetalleProveedor)
                .ForeignKey("dbo.Proveedor", t => t.Proveedor_IdProveedor)
                .Index(t => t.Proveedor_IdProveedor);
            
            CreateTable(
                "dbo.Proveedor_producto",
                c => new
                    {
                        Idproveedor_producto = c.Int(nullable: false),
                        Cantidad_Recibida = c.Int(nullable: false),
                        Numero_Seguimiento = c.String(),
                        Especificacion_Producto_IdEspecificaciones_Producto = c.Int(),
                    })
                .PrimaryKey(t => t.Idproveedor_producto)
                .ForeignKey("dbo.DetalleProveedor", t => t.Idproveedor_producto)
                .ForeignKey("dbo.Especificacion_producto", t => t.Especificacion_Producto_IdEspecificaciones_Producto)
                .Index(t => t.Idproveedor_producto)
                .Index(t => t.Especificacion_Producto_IdEspecificaciones_Producto);
            
            CreateTable(
                "dbo.Especificacion_producto",
                c => new
                    {
                        IdEspecificaciones_Producto = c.Int(nullable: false, identity: true),
                        Garantia = c.DateTime(),
                        IMEI = c.String(),
                        Descripcion = c.String(),
                        Producto_IdProducto = c.Int(),
                        Proveedor_IdProveedor = c.Int(),
                        Venta_IdVenta = c.Int(),
                    })
                .PrimaryKey(t => t.IdEspecificaciones_Producto)
                .ForeignKey("dbo.Producto", t => t.Producto_IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Proveedor", t => t.Proveedor_IdProveedor)
                .ForeignKey("dbo.Venta", t => t.Venta_IdVenta)
                .Index(t => t.Producto_IdProducto)
                .Index(t => t.Proveedor_IdProveedor)
                .Index(t => t.Venta_IdVenta);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Precio_Venta = c.Double(nullable: false),
                        Marca = c.String(),
                        Tipo_Producto = c.String(),
                        Modelo = c.String(),
                        Credito_Disponible = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProducto);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        IdProveedor = c.Int(nullable: false, identity: true),
                        Empresa = c.String(),
                        Lugar_Importacion = c.String(),
                    })
                .PrimaryKey(t => t.IdProveedor);
            
            CreateTable(
                "dbo.Venta",
                c => new
                    {
                        IdVenta = c.Int(nullable: false, identity: true),
                        Fecha_Venta = c.DateTime(nullable: false),
                        Orden_Pagare = c.String(),
                        Tipo_Venta = c.String(),
                        Plazo = c.String(),
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
            DropForeignKey("dbo.Especificacion_pedido", "Pedido_IdPedido", "dbo.Pedido");
            DropForeignKey("dbo.Especificacion_pedido", "IdEspecificacion_Pedido", "dbo.DetalleProveedor");
            DropForeignKey("dbo.Proveedor_producto", "Especificacion_Producto_IdEspecificaciones_Producto", "dbo.Especificacion_producto");
            DropForeignKey("dbo.Pago", "Venta_IdVenta", "dbo.Venta");
            DropForeignKey("dbo.Especificacion_producto", "Venta_IdVenta", "dbo.Venta");
            DropForeignKey("dbo.Especificacion_producto", "Proveedor_IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.DetalleProveedor", "Proveedor_IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.Especificacion_producto", "Producto_IdProducto", "dbo.Producto");
            DropForeignKey("dbo.Proveedor_producto", "Idproveedor_producto", "dbo.DetalleProveedor");
            DropIndex("dbo.Telefono", new[] { "Cliente_IdCliente" });
            DropIndex("dbo.Pago", new[] { "Venta_IdVenta" });
            DropIndex("dbo.Venta", new[] { "Cliente_IdCliente" });
            DropIndex("dbo.Especificacion_producto", new[] { "Venta_IdVenta" });
            DropIndex("dbo.Especificacion_producto", new[] { "Proveedor_IdProveedor" });
            DropIndex("dbo.Especificacion_producto", new[] { "Producto_IdProducto" });
            DropIndex("dbo.Proveedor_producto", new[] { "Especificacion_Producto_IdEspecificaciones_Producto" });
            DropIndex("dbo.Proveedor_producto", new[] { "Idproveedor_producto" });
            DropIndex("dbo.DetalleProveedor", new[] { "Proveedor_IdProveedor" });
            DropIndex("dbo.Especificacion_pedido", new[] { "Pedido_IdPedido" });
            DropIndex("dbo.Especificacion_pedido", new[] { "IdEspecificacion_Pedido" });
            DropIndex("dbo.Pedido", new[] { "Cliente_IdCliente" });
            DropTable("dbo.Telefono");
            DropTable("dbo.Pago");
            DropTable("dbo.Venta");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Producto");
            DropTable("dbo.Especificacion_producto");
            DropTable("dbo.Proveedor_producto");
            DropTable("dbo.DetalleProveedor");
            DropTable("dbo.Especificacion_pedido");
            DropTable("dbo.Pedido");
            DropTable("dbo.Cliente");
        }
    }
}
