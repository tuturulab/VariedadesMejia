using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    [Table("Proveedor_producto")]
    public partial class Proveedor_producto
    {
        [Key]
        public int Idproveedor_producto { get; set; }
        public int Cantidad_Recibida { get; set; }
        public string Numero_Seguimiento { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
