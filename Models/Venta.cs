using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("Venta")]
    public partial class Venta
    {
        public Venta()
        {
            this.Especificaciones_producto = new HashSet<Especificacion_producto>();
            this.Pagos = new HashSet<Pago>();
        }

        [Key]
        public int IdVenta { get; set; }
        public DateTime? Fecha_Venta { get; set; }
        public string Orden_Pagare { get; set; }
        public string Tipo_Venta { get; set; }

        //Almacena el total de dinero que tendrá que pagar por la venta realizada.
        public double MontoVenta { get; set; }

        //Almacenar si esta venta ya se completó
        public string VentaCompletada { get; set; }
        
        public virtual Cliente Cliente { get; set; }
        
        [NotMapped]
        public String NombreCliente { get { if (this.Cliente != null) return Cliente.Cedula; else return "No tiene cliente"; } }

        [NotMapped]
        public int CantidadProductos { get { return Especificaciones_producto.Count; } }


        public virtual ICollection<Pago> Pagos { get; set;  }
        public virtual ICollection<Especificacion_producto> Especificaciones_producto{ get; set; }
    }
}
