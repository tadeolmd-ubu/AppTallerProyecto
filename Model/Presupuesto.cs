using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Presupuesto
    {   
        public int id {  get; set; }
        public decimal total { get; set; }
        public bool estatus { get; set; }
        public string nota { get; set; }
        public int idCliente { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fechaCreacion { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fechaModificacion { get; set; }
        public int idUsuario { get; set; }
        public Presupuesto() {
        }

    }
}
