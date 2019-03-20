using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.Models
{
    [Table("Proveedor")]
    public partial class Proveedor
    {
        public Proveedor()
        {
            this.DetalleProveedor = new HashSet<DetalleProveedor>();
        }

        [Key]
        public int IdProveedor { get; set; }
        public string Empresa { get; set; }
        public string Lugar_Importacion { get; set; }

        public virtual ICollection<DetalleProveedor> DetalleProveedor { get; set; }
    }
}
