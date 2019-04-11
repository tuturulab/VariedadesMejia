using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    public partial class Especificacion_producto
    {
        [Key]
        public int IdEspecificaciones_Producto { get; set; }

        //Borrar
        public DateTime? Garantia { get; set; }
        public string IMEI { get; set; }

        //Color, detalle
        public string Descripcion { get; set; }

        //Si o no
        public String Vendido { get; set; }

        public double PrecioCosto { get; set; }
        public DateTime? Garantia_Original { get; set; }

        //Variable para mostrar el nombre del producto al que fue asignado
        [NotMapped]
        public string Nombre { get { return Producto.Marca + " " + Producto.Modelo; } }

        [NotMapped]
        public double Precio { get { return Producto.Precio_Venta; } }

        [NotMapped]
        public string Tipo_Producto { get { return Producto.Tipo_Producto; } }

        [NotMapped]
        public string GarantiaDisponible { get { if (Producto.Garantia_Disponible == 0) return "No"; else return "Si";  } }

        
        public virtual Venta Venta { get; set; }

        public virtual Producto Producto { get; set; }

        public virtual Proveedor_producto Proveedor_Producto { get; set; }

        public virtual Proveedor Proveedor { get; set; }
    }
}
