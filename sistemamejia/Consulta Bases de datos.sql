SELECT C.Nombre,V.IdVenta, P.Fecha_Pago
FROM Cliente as C, Venta as V, Pago as P
WHERE C.IdCliente = 1 AND C.IdCliente = V.Cliente_IdCliente AND V.IdVenta = P.Venta_IdVenta

SELECT DISTINCT C.Nombre, V.MontoVenta Precio,P.Marca, P.Modelo
FROM Cliente as C, Venta,Producto as P
INNER JOIN Venta as V ON V.IdVenta = P.IdProducto
WHERE C.IdCliente = V.Cliente_IdCliente

SELECT C.Nombre, Pe.Fecha_Pedido, Ep.Cantidad, Ep.Marca,Ep.Modelo,Ep.Descripcion
FROM Cliente as C, Pedido as Pe,Especificacion_pedido as Ep
WHERE C.IdCliente = Pe.Cliente_IdCliente AND Pe.IdPedido = Ep.Pedido_IdPedido