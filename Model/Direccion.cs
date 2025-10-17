using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Direccion
    {

        
        public int id { get; set; }
        public string ciudad { get; set; }
        public string colonia { get; set; }
        public string codigoPostal { get; set; }
        public string calle { get; set; }
        public string numeroCasa { get; set; }

        public Direccion() {
        }

    }
}
