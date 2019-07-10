using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.Models
{
    public partial class Proveedor
    {
        public Proveedor()
        {
            this.Productos = new HashSet<Especificacion_producto>();
        }

        [Key]
        public int IdProveedor { get; set; }
        public string Empresa { get; set; }
        public string Lugar_Importacion { get; set; }

        public virtual ICollection<Especificacion_producto> Productos { get; set; }
    }
}
