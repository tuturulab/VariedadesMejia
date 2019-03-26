using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    public partial class Telefono
    {
        
        [Key]
        public int IdTelefono { get; set; }
        public string Numero { get; set; }

        //Convencional o celular
        public string Tipo_Numero { get; set; }
       
        //Claro , Movistar 
        public string Empresa { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}