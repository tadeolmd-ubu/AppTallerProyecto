using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class ReferenciaService
    {
        EF.efAppDbContext _context;
        public ReferenciaService(EF.efAppDbContext context){
            _context = context;
        }
        public List<ReferenciaMovimiento> ObtenerReferencias() {
        return _context.ReferenciaMovimiento.FromSqlRaw("EXEC sp_ReferenciaMovimiento @opcion = 4").ToList();
        }

        public ReferenciaMovimiento BuscarReferencia(int id) {
            return _context.ReferenciaMovimiento.FromSqlRaw("EXEC sp_ReferenciaMovimiento @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
    }
}
