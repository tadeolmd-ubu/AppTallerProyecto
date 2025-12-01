using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _context.Proveedor.Add(proveedor);
        }
        public void ModificarProveedor(Proveedor proveedor){
            var existente = _context.Proveedor.Find(proveedor.id);
            if (existente == null)
                return;
            _context.Entry(existente).CurrentValues.SetValues(proveedor);
        }
        public void CrearOModificarProveedor(Proveedor proveedor) {
            var existe = _context.Proveedor.Find(proveedor.id);
            if (existe == null){
                CrearProveedor(proveedor);
            }
            else{
                ModificarProveedor(proveedor);
            }
        }
        public void EliminarProveedor(int id){
            var proveedor = _context.Proveedor.Find(id);
            if (proveedor == null)
                return;
            _context.Proveedor.Remove(proveedor);
            _context.SaveChanges();
        }

        public Proveedor BuscarProveedorIndividual(int id){
           return _context.Proveedor.Find(id);
        }
        public List<Proveedor> ObtenerProveedores(){
            return _context.Proveedor.ToList();
        }

        public int ObtenerSigienteIdProveedor()
        {
            var ultimo = _context.Proveedor
                                 .OrderByDescending(x => x.id)
                                 .Select(x => x.id)
                                 .FirstOrDefault();

            return ultimo + 1;
        }

    }
}
