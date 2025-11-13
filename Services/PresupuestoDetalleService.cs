using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class PresupuestoDetalleService
    {
        private readonly EF.efAppDbContext _context;
        public PresupuestoDetalleService(EF.efAppDbContext context){
            _context = context;
        }
        public void CrearPresupuestoDetalle(PresupuestoDetalle presupuestoDetalle){
            _context.PresupuestoDetalle.Add(presupuestoDetalle);   
        }
        public void ModificarPresupuestoDetalle(PresupuestoDetalle presupuestoDetalle){

            var existe = _context.PresupuestoDetalle.Find(presupuestoDetalle.id);

            if (existe == null)
                return;
            _context.Entry(existe).CurrentValues.SetValues(presupuestoDetalle);
            

        }
        public void CrearOModificarPresupuestoDetalle(PresupuestoDetalle presupuestoDetalle){
            var existe = _context.PresupuestoDetalle.Find(presupuestoDetalle.id);
            if (existe == null){
                CrearPresupuestoDetalle(presupuestoDetalle);
            }
            else{
                ModificarPresupuestoDetalle(presupuestoDetalle);
            }
        }
        public PresupuestoDetalle BuscarPresupuestoDetalle(int id){
            return _context.PresupuestoDetalle.Find(id);
        }
        public List<PresupuestoDetalle> ObtenerPresupuestosDetalle(){
            return _context.PresupuestoDetalle.ToList();
        }
        public void EliminarPresupuestoDetalle(int id){

            var presupuestoDetalle = _context.PresupuestoDetalle.Find(id);
            if (presupuestoDetalle == null)
                return;
            _context.PresupuestoDetalle.Remove(presupuestoDetalle);
            _context.SaveChanges();
        }

        public int ObtenerSigienteIdPresupuestoDetalle()
        {
            var ultimo = _context.PresupuestoDetalle
                                 .OrderByDescending(x => x.id)
                                 .Select(x => x.id)
                                 .FirstOrDefault();

            return ultimo + 1;
        }
    }
}
