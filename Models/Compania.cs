using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{ 
    [Table("Compania")]
    public partial class Compania
    {
        public Compania()
        {
            this.Clientes = new HashSet<Cliente>();
        }

        [Key]
        public int IdCompania { get; set; }
        public string Trabajo { get; set; }
        public DateTime? Fecha_Pago { get; set; }
    
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
