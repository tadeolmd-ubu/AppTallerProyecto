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
        private readonly Services.PresupuestoService _presupuestoService;
        private readonly Services.PresupuestoDetalleService _presupuestoDetalleService;

        public PresupuestoLogic(EF.efAppDbContext context){
            _context = context;
            _presupuestoService = new Services.PresupuestoService(_context);
            _presupuestoDetalleService = new Services.PresupuestoDetalleService(_context);
        }
        public void CrearPresupuestoConDetalles(
            Presupuesto presupuesto,
            List<PresupuestoDetalle> detalles)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    _presupuestoService.CrearPresupuesto(presupuesto);
                    _context.SaveChanges();

                    foreach (var det in detalles){
                        det.idPresupuesto = presupuesto.id;
                        _presupuestoDetalleService.CrearPresupuestoDetalle(det);
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
       
        public List<object> ObtenerDetallesPorPresupuesto(int idPresupuesto)
        {
            var query =
                from d in _context.PresupuestoDetalle
                join p in _context.Producto on d.idProducto equals p.id
                where d.idPresupuesto == idPresupuesto
                select new
                {
                    id = d.id,
                    nombre = p.nombre,
                    cantidad = d.cantidad,
                    precioUnitario = d.precioUnitario,
                    importe = d.importe,
                    iva = d.iva
                };

            return query.ToList<object>();
        }
    }
}
