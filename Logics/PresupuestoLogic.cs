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

                    foreach (var det in detalles){
                        det.idPresupuesto = presupuesto.id;
                        _presupuestoDetalleService.CrearPresupuestoDetalle(det);
                    }

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

                    _context.Database.ExecuteSqlRaw(
                        "EXEC sp_Presupuesto @opcion = 2, @id = {0}, @total = {1}, @estatus = {2}, @nota = {3}, @idCliente = {4}, @idUsuario = {5}",
                        presupuesto.id, presupuesto.total, presupuesto.estatus, presupuesto.nota ?? (object)DBNull.Value,
                        presupuesto.idCliente, presupuesto.idUsuario);

                    var detallesExistentes = ObtenerDetallesPorPresupuesto(presupuesto.id);

                    var idsRecibidos = new HashSet<int>(
                        detalles.Where(d => d.id != 0).Select(d => d.id)
                    );
                    foreach (var detExist in detallesExistentes){
                        if (!idsRecibidos.Contains(detExist.id)){
                            _context.Database.ExecuteSqlRaw("EXEC sp_PresupuestoDetalle @opcion = 3, @id = {0}", detExist.id);
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
                            _presupuestoDetalleService.CrearPresupuestoDetalle(det);
                        }
                        else{
                            var detEnBd = detallesExistentes.FirstOrDefault(x => x.id == det.id);
                            if (detEnBd != null){
                                _context.Database.ExecuteSqlRaw(
                                    "EXEC sp_PresupuestoDetalle @opcion = 2, @id = {0}, @cantidad = {1}, @precioUnitario = {2}, @importe = {3}, @iva = {4}, @idInventario = {5}, @idPresupuesto = {6}",
                                    det.id, det.cantidad, det.precioUnitario, det.importe, det.iva, det.idInventario, det.idPresupuesto);
                            }
                        }
                    }
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
                    var presupuesto = _context.Presupuesto.Find(idPresupuesto);
                    if (presupuesto == null)
                        throw new InvalidOperationException($"Presupuesto con id {idPresupuesto} no encontrado.");

                    _context.Database.ExecuteSqlRaw("EXEC sp_Presupuesto @opcion = 3, @id = {0}", idPresupuesto);

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
