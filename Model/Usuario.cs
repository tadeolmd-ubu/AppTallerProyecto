using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Usuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string contrasena { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public  bool estatus { get; set; }
        public int idRol {  get; set; }


        public Usuario() {
        
        }
    }
}
