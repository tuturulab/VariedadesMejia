using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("Producto")]
    public partial class Producto
    {
        public Producto()
        {
            this.Especificaciones_producto = new HashSet<Especificacion_producto>();
        }
        
        [Key]
        public int IdProducto { get; set; }
        public double Precio_Venta { get; set; }
        public string Marca { get; set; }
        public string Tipo_Producto { get; set; }
        public string Modelo { get; set; }
        public int Credito_Disponible { get; set; }

        public virtual ICollection<Especificacion_producto> Especificaciones_producto { get; set; }
        
    }
}
