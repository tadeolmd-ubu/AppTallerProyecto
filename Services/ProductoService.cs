using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class ProductoService
    {
        private readonly EF.efAppDbContext _context;

        public ProductoService(EF.efAppDbContext context){
            _context = context;
        }

        public void AgregarProducto(Producto producto) {
            if (producto.id == 0)
                producto.id = _context.SiguienteId("Producto");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Producto @opcion = 1, @id = {0}, @nombre = {1}, @precio = {2}, @estatus = {3}, @idMarca = {4}, @idTipoProducto = {5}",
                producto.id, producto.nombre, producto.precio, producto.estatus, producto.idMarca, producto.idTipoProducto);
        }
        public void ModificarProducoto(Producto producto) {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Producto @opcion = 2, @id = {0}, @nombre = {1}, @precio = {2}, @estatus = {3}, @idMarca = {4}, @idTipoProducto = {5}",
                producto.id, producto.nombre, producto.precio, producto.estatus, producto.idMarca, producto.idTipoProducto);
        }
        public void CrearOActualizarProducto(Producto producto){
            var existe = _context.Producto.Find(producto.id);
            if (existe == null)
                AgregarProducto(producto);
            else
                ModificarProducoto(producto);
        }
        public void EliminarProducto(int id){
            _context.Database.ExecuteSqlRaw("EXEC sp_Producto @opcion = 3, @id = {0}", id);
        }
        public Producto BuscarProductoIndividual(int id){
            return _context.Producto.FromSqlRaw("EXEC sp_Producto @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public List<Producto> ObtenerProductos(){
            return _context.Producto.FromSqlRaw("EXEC sp_Producto @opcion = 4").ToList();
        }
        public int ObtenerSigienteIdProducto()
        {
            return _context.SiguienteId("Producto");
        }
    }
}
