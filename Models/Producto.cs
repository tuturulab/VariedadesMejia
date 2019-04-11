using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Variedades.Models
{
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

        //Garantia numero de meses
        public int? Garantia { get; set; }

        //Proceso almacenado para saber si un producto tiene acceso a crédito a la hora de venta, o si este tiene imei, puesto que hay tablets que tienen o no imei
        public int Credito_Disponible { get; set; }
        public int Imei_Disponible { get; set; }
        public int Garantia_Disponible { get; set; }

        //Variable para mostrar un si o un no en caso de que tenga acceso a credito
        [NotMapped]
        public string Crediticio { get { if (Credito_Disponible == 1) return "Sí"; else return "No"; } }

        //Variable para guardar el numero de especificaciones a mostrae en la base de datos
        [NotMapped]
        public int NumeroDeEspecificacionesDisponibles { get { return Especificaciones_producto.Where(x => x.Vendido.Equals("No") ).Count(); }  }
        
        [NotMapped]
        public int NumeroDeEspecificacionesTotales { get; set; }
        
        public virtual ICollection<Especificacion_producto> Especificaciones_producto { get; set; }
        
    }
}
