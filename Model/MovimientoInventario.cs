using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class MovimientoInventario
    {
        public int id { get; set; }
        public int idInventario { get; set; }
        public int idTipoMovimiento { get; set; }
        public int idReferenciaMovimiento { get; set; }
        public int cantidad { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fechaMovimiento { get; set; }

        public MovimientoInventario(){
        }
    }
}
