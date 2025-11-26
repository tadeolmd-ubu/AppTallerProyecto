using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class PresupuestoDetalle
    {
        public int id {  get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal importe { get; set; }
        public decimal iva {  get; set; }
        public int idInventario { get; set; }  
        public int idPresupuesto { get; set; }
        public PresupuestoDetalle(){
        }

    }
}
