using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("Pedido")]
    public partial class Pedido
    {
        public Pedido()
        {
            this.Especificaciones_pedido = new HashSet<Especificacion_pedido>();
        }

        [Key]
        public int IdPedido { get; set; }
        public DateTime? Fecha_Pedido { get; set; }

        public DateTime? Fecha_Entrega { get; set; }

        public virtual Cliente cliente { get; set; }
        public virtual ICollection<Especificacion_pedido> Especificaciones_pedido { get; set; }

        [NotMapped]
        public string NombreCliente { get { if (this.cliente != null) return this.cliente.Nombre; else return "Eliminado";  } }
        
        [NotMapped]
        public int NumeroProductos { get { return this.Especificaciones_pedido.Count; }   }
    }
}
