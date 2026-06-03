using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class catEmpresa
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string rfc { get; set; }
        public string regimen { get; set; }
        public int? idDireccion { get; set; }

        public catEmpresa()
        {
        }
    }
}
