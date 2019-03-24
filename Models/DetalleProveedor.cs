using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("DetalleProveedor")]
    public partial class DetalleProveedor
    {
        public DetalleProveedor()
        {
            this.Especificacion_pedido = new HashSet<Especificacion_pedido>();
            this.Proveedor_Productos = new HashSet<Proveedor_producto>();
        }

        [Key]
        public int IdDetalleProveedor { get; set; }

        public double Precio_Costo { get; set; }
        public DateTime? Fecha_Llegada { get; set; }
        public DateTime? Garantia_Original { get; set; }

        public virtual ICollection<Especificacion_pedido> Especificacion_pedido { get; set; }
        
        public virtual Proveedor Proveedor { get; set; }

        public virtual ICollection<Proveedor_producto> Proveedor_Productos { get; set; }
    }
}
