using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("DetalleProveedor")]
    public partial class DetalleProveedor
    {
        [Key]
        public int IdDetalleProveedor { get; set; }

        public double Precio_Costo { get; set; }
        public DateTime? Fecha_Llegada { get; set; }
        public DateTime? Garantia_Original { get; set; }

        public virtual Especificacion_pedido Especificacion_pedido { get; set; }
        
        public virtual Proveedor_producto Proveedor_producto { get; set; }
    }
}
