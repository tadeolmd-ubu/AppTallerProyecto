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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string ciudad { get; set; }
        public string colonia { get; set; }
        public int codigoPostal { get; set; }
        public string calle { get; set; }
        public int numeroCasa { get; set; }

        public Direccion() {
        }

    }
}
