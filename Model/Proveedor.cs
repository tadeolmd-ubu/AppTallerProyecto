using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Proveedor
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public bool estatus { get; set; }
        public int idEmpresa { get; set; }          
        public int idDireccion { get; set; }
        public Proveedor(){
        }
    }
}
