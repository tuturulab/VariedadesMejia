using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("Especificacion_producto")]
    public partial class Especificacion_producto
    {
        [Key]
        public int IdEspecificaciones_Producto { get; set; }
        public string Garantia { get; set; }
        public string IMEI { get; set; }

        public virtual Venta Venta { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
