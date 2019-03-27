using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    public partial class Pago
    {   
        [Key]
        public int IdPago { get; set; }

        public double Monto { get; set; }
        
        public DateTime? Fecha_Pago { get; set; }

        public virtual Venta Venta { get; set; } 
    }
}
