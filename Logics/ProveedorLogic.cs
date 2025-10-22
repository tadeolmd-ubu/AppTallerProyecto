using AppTaller.EF;
using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Logics
{
    internal class ProveedorLogic
    {
        private readonly efAppDbContext _context;
        private readonly DireccionService _direccionService;
        private readonly ProveedorService _proveedorService;

        public ProveedorLogic(efAppDbContext context,DireccionService direccionService, ProveedorService proveedorService){
            _context = context;
            _direccionService = direccionService;
            _proveedorService = proveedorService;
        }
        public void GuardarProveedorYDireccion(Proveedor proveedor, Direccion direccion) {
            using (var transaction = _context.Database.BeginTransaction()){
                try{
                    _direccionService.CrearOActualizarDireccion(direccion);
                    _context.SaveChanges();

                    proveedor.idDireccion = direccion.id;
                    _proveedorService.CrearOModificarProveedor(proveedor);

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception){
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

}

