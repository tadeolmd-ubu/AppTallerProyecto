using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class PresupuestoService
    {
       private readonly EF.efAppDbContext _context;
        public PresupuestoService(EF.efAppDbContext context){
            _context = context;
        }

        public void CrearPresupuesto(Presupuesto presupuesto) {
            _context.Presupuesto.Add(presupuesto);
            _context.SaveChanges();
        }

        public void ModificarPresupuesto(Presupuesto presupuesto) {
        
            var existe = _context.Presupuesto.Find(presupuesto.id);

            if (existe == null)
                return;
            _context.Entry(existe).CurrentValues.SetValues(presupuesto);
            _context.SaveChanges();

        }
        public void CrearOModificarPresupuesto(Presupuesto presupuesto){
            var existe = _context.Presupuesto.Find(presupuesto.id);
            if (existe == null){
                CrearPresupuesto(presupuesto);
            }
            else{
                ModificarPresupuesto(presupuesto);
            }
        }
        public Presupuesto BuscarPresupuesto(int id) {
            return _context.Presupuesto.Find(id);
        }
        public List<Presupuesto> ObtenerPresupuestos() {
            return _context.Presupuesto.ToList();
        }
        public void EliminarPresupuesto(int id) {

            var presupuesto = _context.Presupuesto.Find(id);
            if (presupuesto == null)
                return;
            _context.Presupuesto.Remove(presupuesto);
            _context.SaveChanges();
        }
    }
}
