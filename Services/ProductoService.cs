using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class ProductoService
    {
        private readonly EF.efAppDbContext _context;

        public ProductoService(EF.efAppDbContext context){
            _context = context;
        }

        public void AgregarProducto(Producto producto) {
            _context.Producto.Add(producto);
            _context.SaveChanges();
        }
        public void ModificarProducoto(Producto producto) {
            var existente = _context.Producto.Find(producto.id);
            if (existente == null)
                return;
            else{
                _context.Entry(existente).CurrentValues.SetValues(producto);
                _context.SaveChanges();
            }
        }
        public void CrearOActualizarProducto(Producto producto){
            var existe = _context.Producto.Find(producto.id);
            if (existe == null){
                AgregarProducto(producto);
            }
            else{
                ModificarProducoto(producto);
            }
        }
        public void EliminarProducto(int id){
            var producto = _context.Producto.Find(id);
            if (producto == null)
                return;
            _context.Producto.Remove(producto);
            _context.SaveChanges();
        }
        public Producto BuscarProductoIndividual(int id){
            return _context.Producto.Find(id);
        }
        public List<Producto> ObtenerProductos(){
            return _context.Producto.ToList();
        }
    }
}
