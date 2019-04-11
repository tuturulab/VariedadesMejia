using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            this.Ventas = new HashSet<Venta>();
            this.Pedidos = new HashSet<Pedido>();
            this.Telefonos = new HashSet<Telefono>();
        }
        
        [Key]
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Domicilio { get; set; }
        public string Tipo_Pago { get; set; }
        public string Compania { get; set; }
        public string Cedula { get; set; }
        
        public int? Fecha_Pago_1 { get; set;  }
        public int? Fecha_Pago_2 { get; set; }

        public virtual ICollection<Venta> Ventas { get; set; }
        
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Telefono> Telefonos { get; set; }
    }
}
