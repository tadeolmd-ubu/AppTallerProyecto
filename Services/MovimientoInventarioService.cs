using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class MovimientoInventarioService
    {
        private readonly EF.efAppDbContext _context;

        public MovimientoInventarioService(EF.efAppDbContext context ) {
            _context = context;
        }
        public void CrearMovimientoInventario(MovimientoInventario movimientoInventario) {
        _context.MovimientoInventario.Add(movimientoInventario);
        }
        public int ObtenerSiguienteId(){
            return _context.MovimientoInventario.Any()
                ? _context.MovimientoInventario.Max(m => m.id) + 1
                : 1;
        }

    }
}
