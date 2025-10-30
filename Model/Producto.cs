using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Producto
    {
        public int id {  get; set; }
        public string nombre { get; set; }
        public decimal precio { get; set; }
        public bool estatus { get; set; }
        public int idMarca { get; set; }
        public int idTipoProducto { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fechaCreacion { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fechaActualizacion { get; set; }

        public Producto() {
        }
    }
}
