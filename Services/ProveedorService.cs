using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class ProveedorService
    {
        private readonly EF.efAppDbContext _context;

        public ProveedorService(EF.efAppDbContext context)
        {
            _context = context;
        }
        public void  CrearProveedor(Proveedor proveedor ) {
            if (proveedor.id == 0)
                proveedor.id = _context.SiguienteId("Proveedor");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Proveedor @opcion = 1, @id = {0}, @nombre = {1}, @telefono = {2}, @estatus = {3}, @idEmpresa = {4}, @idDireccion = {5}",
                proveedor.id, proveedor.nombre, proveedor.telefono, proveedor.estatus, proveedor.idEmpresa, proveedor.idDireccion);
        }
        public void ModificarProveedor(Proveedor proveedor){
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Proveedor @opcion = 2, @id = {0}, @nombre = {1}, @telefono = {2}, @estatus = {3}, @idEmpresa = {4}, @idDireccion = {5}",
                proveedor.id, proveedor.nombre, proveedor.telefono, proveedor.estatus, proveedor.idEmpresa, proveedor.idDireccion);
        }
        public void CrearOModificarProveedor(Proveedor proveedor) {
            var existe = _context.Proveedor.Find(proveedor.id);
            if (existe == null)
                CrearProveedor(proveedor);
            else
                ModificarProveedor(proveedor);
        }
        public void EliminarProveedor(int id){
            _context.Database.ExecuteSqlRaw("EXEC sp_Proveedor @opcion = 3, @id = {0}", id);
        }

        public Proveedor BuscarProveedorIndividual(int id){
            return _context.Proveedor.FromSqlRaw("EXEC sp_Proveedor @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public List<Proveedor> ObtenerProveedores(){
            return _context.Proveedor.FromSqlRaw("EXEC sp_Proveedor @opcion = 4").ToList();
        }

        public int ObtenerSigienteIdProveedor()
        {
            return _context.SiguienteId("Proveedor");
        }

    }
}
