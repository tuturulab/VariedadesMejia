using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("Proveedor")]
    public partial class Proveedor
    {
        public Proveedor()
        {
            this.Especificaciones_pedido = new HashSet<Especificacion_pedido>();
            this.Proveedores_producto = new HashSet<Proveedor_producto>();
        }

        [Key]
        public int Id { get; set; }
        public string Empresa { get; set; }
        public double Precio_Costo { get; set; }
        public string Lugar_Importacion { get; set; }
        public DateTime? Fecha_Llegada { get; set; }
        public DateTime? Garantia_Original { get; set; }

        public virtual ICollection<Proveedor_producto> Proveedores_producto { get; set; }
        public virtual ICollection<Especificacion_pedido> Especificaciones_pedido { get; set; }
    }
}
