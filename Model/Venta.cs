using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Venta
    {
        public int id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public bool estatus { get; set; }
        public int idEmpleado { get; set; }
        public int idCliente { get; set; }
        public int folio { get; set; }

        public Venta() {
        }
    }
}
