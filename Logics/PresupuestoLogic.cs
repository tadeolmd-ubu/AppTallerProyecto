using AppTaller.EF;
using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Logics
{
    internal class PresupuestoLogic
    {
        private readonly EF.efAppDbContext _context;
        public PresupuestoLogic(EF.efAppDbContext context){
            _context = context;
        }
        public void CrearPresupuestoConDetalles(
            Presupuesto presupuesto,
            List<PresupuestoDetalle> detalles)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    _context.Presupuesto.Add(presupuesto);
                    _context.SaveChanges();

                    foreach (var det in detalles){
                        det.idPresupuesto = presupuesto.id;
                        _context.PresupuestoDetalle.Add(det);
                    }

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch{
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
