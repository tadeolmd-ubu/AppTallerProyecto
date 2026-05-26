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

        //guardar los presupuestos
        public void CrearPresupuestoConDetalles(
            Presupuesto presupuesto,
            List<PresupuestoDetalle> detalles){
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

       //jalar los detalles de cada presupuesto
        public List<PresupuestoDetalle> ObtenerDetallesPorPresupuesto(int idPresupuesto){
            return _context.PresupuestoDetalle
                           .Where(pd => pd.idPresupuesto == idPresupuesto)
                           .ToList();
        }

        //Actualizacion de un presupuesto existente
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
                        nextId = _context.SiguienteId("PresupuestoDetalle");
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
                catch{
                    transaction.Rollback();
                    throw;
                }
            }
        }

        //Elimina los presupuestos y todos los detalles asociados
        public void EliminarPresupuestoConDetalles(int idPresupuesto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    var detalles = _context.PresupuestoDetalle
                                           .Where(d => d.idPresupuesto == idPresupuesto)
                                           .ToList();

                    if (detalles.Any()){
                        _context.PresupuestoDetalle.RemoveRange(detalles);
                        _context.SaveChanges();
                    }
                    var presupuesto = _context.Presupuesto.Find(idPresupuesto);
                    if (presupuesto == null)
                        throw new InvalidOperationException($"Presupuesto con id {idPresupuesto} no encontrado.");

                    _context.Presupuesto.Remove(presupuesto);
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