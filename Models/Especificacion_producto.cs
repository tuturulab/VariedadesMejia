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
        public DateTime? Garantia { get; set; }
        public string IMEI { get; set; }

        //Color, detalle
        public string Descripcion { get; set; }

        public virtual Venta Venta { get; set; }
        public virtual Producto Producto { get; set; }
        
        [Required]
        public virtual Proveedor_producto Proveedor_Producto { get; set; }
    }
}
