using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Variedades.Models
{
    public partial class DetalleProveedor
    {
        public DetalleProveedor()
        {
            this.Proveedor_Productos = new HashSet<Proveedor_producto>();
            this.Producto_Importados = new HashSet<Producto_importado>();
        }

        [Key]
        public int IdDetalleProveedor { get; set; }

        public double Precio_Costo { get; set; }
        public DateTime? Fecha_Llegada { get; set; }
        
        
        public string Estado { get; set; }

        //En espera, llegó
        [NotMapped]
        public string EstadoEncargo { get { if (this.Estado == null) return "En trámite"; else return "Concluido"; } }

        public string Numero_Seguimiento { get ; set; }

        [NotMapped]
        public String Llegada { get { if (this.Fecha_Llegada == null) return "No ha llegado"; else return this.Fecha_Llegada.ToString();    }  }

        [NotMapped]
        public string Seguimiento { get { if (this.Numero_Seguimiento != null) return Numero_Seguimiento; else return "No Tiene";  } }

        [NotMapped]
        public int NumeroEncargos { get { if (this.Producto_Importados != null) return this.Producto_Importados.Count(); else return 0; }  }

        public virtual Pedido Pedido { get; set; }
        

        

        public virtual ICollection<Proveedor_producto> Proveedor_Productos { get; set; }
        public virtual ICollection<Producto_importado> Producto_Importados { get; set; }
    }
}
