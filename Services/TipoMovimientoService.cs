using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class TipoMovimientoService
    {
        private readonly EF.efAppDbContext _context;

        public TipoMovimientoService(EF.efAppDbContext context) {
        _context = context;
        }

        public List<TipoMovimiento> ObtenerTiposMovimientos() {
        return _context.TipoMovimiento.FromSqlRaw("EXEC sp_TipoMovimiento @opcion = 4").ToList();
        }
    }
}
