using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    public partial class Proveedor_producto
    {
        public Proveedor_producto()
        {
            this.Especificacion_Productos = new HashSet<Especificacion_producto>();
        }

        [Key]
        public int Idproveedor_producto { get; set; }
        public int Cantidad_Recibida { get; set; }
       
        public virtual DetalleProveedor DetalleProveedor { get; set; }

        public virtual ICollection<Especificacion_producto> Especificacion_Productos { get; set; }
    }
}
