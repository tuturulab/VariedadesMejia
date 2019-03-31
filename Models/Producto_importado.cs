using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.Models
{
    public partial class Producto_importado
    {
        [Key]
        public int IdProducto_importado { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Cantidad { get; set; }

        public string NombreProveedor { get; set; }

        public double Precio { get; set; }
        public virtual DetalleProveedor DetalleProveedor { get; set; }
    }
}
