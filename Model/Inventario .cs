using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Inventario
    {  
        public int id { get; set; }
        public int idProducto { get; set; }
        public int idAlmacen { get; set; }  
        public int stockActual { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fechaUltimaActualizacion { get; set; }

        public Inventario() {
        }
    }
}
