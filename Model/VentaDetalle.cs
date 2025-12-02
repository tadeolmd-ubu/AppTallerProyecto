using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class VentaDetalle
    {
        public int id { get; set; }

        public int cantidad {  get; set; }

        public decimal importe {  get; set; }
        public decimal iva {  get; set; }
        public int idVenta { get; set; }
        public int idInventario { get; set; }
        public decimal precioUnitario { get; set; }

        public VentaDetalle() {
        }
    }
}
