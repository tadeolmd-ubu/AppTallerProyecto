using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Cliente
    {
        
        public int id { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public bool estatus { get; set; }
        public int idDireccion { get; set; }

        public Cliente() { 
        }


    }
}
