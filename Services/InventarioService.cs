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
            _context.Inventario.Add(inventario);
            _context.SaveChanges();
        }

        public void ModificarInventario(Inventario inventario){
            var existente = _context.Inventario.Find(inventario.id);
            if (existente == null)
                return;
            _context.Entry(existente).CurrentValues.SetValues(inventario);
            _context.SaveChanges();
        }

        public Inventario BuscarInventario(int id) {
        return _context.Inventario.Find(id);
        }
        public List<Inventario> ObtenerInventarios(){
            return _context.Inventario.ToList();
        }
        public bool ExisteInventarioProductoAlmacen(int idProducto, int idAlmacen)
        {
            return _context.Inventario.Any(i => i.idProducto == idProducto && i.idAlmacen == idAlmacen);
        }
    }
}
