using AppTaller.EF;
using AppTaller.Model;
using Microsoft.EntityFrameworkCore;
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

        public PresupuestoLogic(EF.efAppDbContext context)
        {
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
                try
                {
                    _presupuestoService.CrearPresupuesto(presupuesto);
                    _context.SaveChanges();

                    foreach (var det in detalles)
                    {
                        det.idPresupuesto = presupuesto.id;
                        _presupuestoDetalleService.CrearPresupuestoDetalle(det);
                    }

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // Plan en pseudocódigo:
        // 1. Recibir el presupuesto con su id y la lista de detalles nueva.
        // 2. Abrir una transacción.
        // 3. Buscar en la BD el presupuesto existente por id; si no existe lanzar excepción.
        // 4. Actualizar los valores escalares del presupuesto existente con los nuevos valores.
        //    - Usar Entry(...).CurrentValues.SetValues(presupuesto) para mapear propiedades.
        // 5. Obtener la lista de detalles actuales asociados al presupuesto.
        // 6. Comparar detalles actuales con detalles recibidos:
        //    - Para cada detalle recibido:
        //        * Si id == 0 -> es nuevo: asignar idPresupuesto y Añadir.
        //        * Si id != 0 y existe en BD -> actualizar sus valores con SetValues.
        //        * Si id != 0 y no existe -> asignar idPresupuesto y Añadir.
        //    - Para cada detalle existente en BD que no esté en la lista recibida -> eliminarlo.
        // 7. Guardar cambios y confirmar la transacción.
        // 8. En caso de error, revertir la transacción y propagar la excepción.

        // Implementación: actualiza presupuesto y sus detalles (añadir/actualizar/eliminar)
        public void ActualizarpresupuestoConDetalle(Presupuesto presupuesto, List<PresupuestoDetalle> detalles)
        {
            if (presupuesto == null) throw new ArgumentNullException(nameof(presupuesto));
            if (detalles == null) detalles = new List<PresupuestoDetalle>();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Obtener presupuesto existente
                    var existente = _context.Presupuesto.Find(presupuesto.id);
                    if (existente == null)
                        throw new InvalidOperationException($"Presupuesto con id {presupuesto.id} no encontrado.");

                    // Actualizar propiedades escalares del presupuesto
                    _context.Entry(existente).CurrentValues.SetValues(presupuesto);

                    // Obtener detalles actuales desde la BD
                    var detallesExistentes = ObtenerDetallesPorPresupuesto(presupuesto.id);

                    // Conjunto de ids de detalles recibidos (los que deben permanecer/actualizarse)
                    var idsRecibidos = new HashSet<int>(detalles.Where(d => d != null && d.id != 0).Select(d => d.id));

                    // Eliminar detalles que existen en BD pero no están en la lista recibida
                    foreach (var detExist in detallesExistentes)
                    {
                        if (!idsRecibidos.Contains(detExist.id))
                        {
                            // Remover del contexto
                            _context.PresupuestoDetalle.Remove(detExist);
                        }
                    }

                    // Procesar detalles recibidos: nuevos o a actualizar
                    foreach (var det in detalles)
                    {
                        if (det == null) continue;

                        if (det.id == 0)
                        {
                            // Nuevo detalle
                            det.idPresupuesto = presupuesto.id;
                            _context.PresupuestoDetalle.Add(det);
                        }
                        else
                        {
                            // Intentar actualizar existente
                            var detEnBd = detallesExistentes.FirstOrDefault(d => d.id == det.id);
                            if (detEnBd != null)
                            {
                                // Actualizar propiedades del detalle existente
                                _context.Entry(detEnBd).CurrentValues.SetValues(det);
                            }
                            else
                            {
                                // No está en BD (posible inconsistencia): tratar como nuevo
                                det.idPresupuesto = presupuesto.id;
                                _context.PresupuestoDetalle.Add(det);
                            }
                        }
                    }

                    // Guardar todos los cambios
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        // En PresupuestoDetalleService
        public List<PresupuestoDetalle> ObtenerDetallesPorPresupuesto(int idPresupuesto)
        {
            return _context.PresupuestoDetalle
                           .Where(pd => pd.idPresupuesto == idPresupuesto)
                           .ToList();
        }


        public void ActualizarPresupuestoConDetalles(Presupuesto presupuesto, List<PresupuestoDetalle> detalles)
        {
            using (var transaction = _context.Database.BeginTransaction()){
                try{
                    var existente = _context.Presupuesto.Find(presupuesto.id);
                    if (existente == null)
                        throw new InvalidOperationException($"Presupuesto con id {presupuesto.id} no encontrado.");

                    _context.Entry(existente).CurrentValues.SetValues(presupuesto);

                    var detallesExistentes = ObtenerDetallesPorPresupuesto(presupuesto.id);

                    var idsRecibidos = new HashSet<int>(
                        detalles.Where(d => d.id != 0).Select(d => d.id)
                    );
                    foreach (var detExist in detallesExistentes){
                        if (!idsRecibidos.Contains(detExist.id)){
                            _context.PresupuestoDetalle.Remove(detExist);
                        }
                    }
                    int nextId = 0;
                    if (detalles.Any(d => d.id == 0)){
                        nextId = _context.PresupuestoDetalle
                                         .OrderByDescending(x => x.id)
                                         .Select(x => x.id)
                                         .FirstOrDefault() + 1;
                    }
                    foreach (var det in detalles){
                        if (det.id == 0){
                            det.id = nextId++;
                            det.idPresupuesto = presupuesto.id;
                            _context.PresupuestoDetalle.Add(det);
                        }
                        else{
                            var detEnBd = detallesExistentes.FirstOrDefault(x => x.id == det.id);

                            if (detEnBd != null){
                                _context.Entry(detEnBd).CurrentValues.SetValues(det);
                            }
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}