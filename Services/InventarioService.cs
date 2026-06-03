using AppTaller.EF;
using AppTaller.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class InventarioService
    {
        private readonly EF.efAppDbContext _context;
        public InventarioService(EF.efAppDbContext context){
            _context = context;
        }

        public void CrearInventario(Inventario inventario) {
            if (inventario.id == 0)
                inventario.id = _context.SiguienteId("Inventario");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Inventario @opcion = 1, @id = {0}, @idProducto = {1}, @idAlmacen = {2}, @stockActual = {3}",
                inventario.id, inventario.idProducto, inventario.idAlmacen, inventario.stockActual);
        }

        public void ModificarInventario(Inventario inventario){
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Inventario @opcion = 2, @id = {0}, @idProducto = {1}, @idAlmacen = {2}, @stockActual = {3}",
                inventario.id, inventario.idProducto, inventario.idAlmacen, inventario.stockActual);
        }

        public Inventario BuscarInventario(int id) {
            return _context.Inventario.FromSqlRaw("EXEC sp_Inventario @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public List<Inventario> ObtenerInventarios()
        {
            return _context.Inventario
                .FromSqlRaw("EXEC sp_Inventario @opcion = 4")
                .ToList();
        }
        public bool ExisteInventarioProductoAlmacen(int idProducto, int idAlmacen)
        {
            return _context.Inventario.Any(i => i.idProducto == idProducto && i.idAlmacen == idAlmacen);
        }
        public int ObtenerSigienteIdInventario()
        {
            return _context.SiguienteId("Inventario");
        }

        public bool ValidarStock(int idProducto, int cantidad)
        {
            var producto = _context.Inventario.Find(idProducto);
            if (producto == null) return false;

            return producto.stockActual >= cantidad;
        }
    }
}
