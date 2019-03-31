using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Variedades.Models
{
    public partial class Especificacion_pedido
    {
        [Key]
        public int IdEspecificacion_Pedido { get; set; }
        public string Tipo_Producto { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Descripcion { get; set; }
        public string Cantidad { get; set; }

        [NotMapped]
        public string Nombre { get
        {
            string nombre = "";
            if (this.Marca != string.Empty)
                nombre = nombre + this.Marca;

            if (this.Modelo != string.Empty)
                nombre = nombre + " " + this.Modelo;

            return nombre;
        }}

        public virtual Pedido Pedido { get; set; }
    }
}
