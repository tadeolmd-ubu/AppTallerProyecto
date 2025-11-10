using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class ReferenciaService
    {
        EF.efAppDbContext _context;
        public ReferenciaService(EF.efAppDbContext context){
            _context = context;
        }
        public List<ReferenciaMovimiento> ObtenerReferencias() {
        return _context.ReferenciaMovimiento.ToList();
        }

        public ReferenciaMovimiento BuscarReferencia(int id) {
            return _context.ReferenciaMovimiento.Find(id);
        }
    }
}
