using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.Models
{
    //Clase para representar el databinding de la ventana MultiUsesVenta, donde se jalara los productos
    public class ProductoEnVenta
    {
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; }

        public int Cantidad { get; set; }

        public double PrecioProducto { get; set; }

        public double Subtotal { get; set; }

        public List<Especificacion_producto> ListaEspecificacionesProductos = new List<Especificacion_producto>();
    }
}
