using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Variedades.Models
{
    public partial class DetalleProveedor
    {
        public DetalleProveedor()
        {
            this.Proveedor_Productos = new HashSet<Proveedor_producto>();
        }

        [Key]
        public int IdDetalleProveedor { get; set; }

        public double Precio_Costo { get; set; }
        public DateTime? Fecha_Llegada { get; set; }
        public DateTime? Garantia_Original { get; set; }

        
        public virtual Pedido Pedido { get; set; }
        
        public virtual Proveedor Proveedor { get; set; }

        public virtual ICollection<Proveedor_producto> Proveedor_Productos { get; set; }
    }
}
