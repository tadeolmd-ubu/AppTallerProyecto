using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class MovimientoInventarioService
    {
        private readonly EF.efAppDbContext _context;

        public MovimientoInventarioService(EF.efAppDbContext context ) {
            _context = context;
        }
        public void CrearMovimientoInventario(MovimientoInventario movimientoInventario) {
            if (movimientoInventario.id == 0)
                movimientoInventario.id = _context.SiguienteId("MovimientoInventario");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_MovimientoInventario @opcion = 1, @id = {0}, @idInventario = {1}, @idTipoMovimiento = {2}, @idReferenciaMovimiento = {3}, @cantidad = {4}, @idUsuario = {5}",
                movimientoInventario.id, movimientoInventario.idInventario, movimientoInventario.idTipoMovimiento,
                movimientoInventario.idReferenciaMovimiento, movimientoInventario.cantidad, (int?)null);
        }
        public int ObtenerSiguienteId(){
            return _context.SiguienteId("MovimientoInventario");
        }

    }
}
