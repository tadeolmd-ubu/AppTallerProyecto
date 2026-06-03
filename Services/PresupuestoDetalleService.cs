using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class PresupuestoDetalleService
    {
        private readonly EF.efAppDbContext _context;
        public PresupuestoDetalleService(EF.efAppDbContext context){
            _context = context;
        }
        public void CrearPresupuestoDetalle(PresupuestoDetalle presupuestoDetalle){
            if (presupuestoDetalle.id == 0)
                presupuestoDetalle.id = _context.SiguienteId("PresupuestoDetalle");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_PresupuestoDetalle @opcion = 1, @id = {0}, @cantidad = {1}, @precioUnitario = {2}, @importe = {3}, @iva = {4}, @idInventario = {5}, @idPresupuesto = {6}",
                presupuestoDetalle.id, presupuestoDetalle.cantidad, presupuestoDetalle.precioUnitario,
                presupuestoDetalle.importe, presupuestoDetalle.iva, presupuestoDetalle.idInventario, presupuestoDetalle.idPresupuesto);
        }
        public void ModificarPresupuestoDetalle(PresupuestoDetalle presupuestoDetalle){
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_PresupuestoDetalle @opcion = 2, @id = {0}, @cantidad = {1}, @precioUnitario = {2}, @importe = {3}, @iva = {4}, @idInventario = {5}, @idPresupuesto = {6}",
                presupuestoDetalle.id, presupuestoDetalle.cantidad, presupuestoDetalle.precioUnitario,
                presupuestoDetalle.importe, presupuestoDetalle.iva, presupuestoDetalle.idInventario, presupuestoDetalle.idPresupuesto);
        }
        public void CrearOModificarPresupuestoDetalle(PresupuestoDetalle presupuestoDetalle){
            var existe = _context.PresupuestoDetalle.Find(presupuestoDetalle.id);
            if (existe == null)
                CrearPresupuestoDetalle(presupuestoDetalle);
            else
                ModificarPresupuestoDetalle(presupuestoDetalle);
        }
        public PresupuestoDetalle BuscarPresupuestoDetalle(int id){
            return _context.PresupuestoDetalle.FromSqlRaw("EXEC sp_PresupuestoDetalle @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public List<PresupuestoDetalle> ObtenerPresupuestosDetalle(){
            return _context.PresupuestoDetalle.FromSqlRaw("EXEC sp_PresupuestoDetalle @opcion = 4").ToList();
        }
        public void EliminarPresupuestoDetalle(int id){
            _context.Database.ExecuteSqlRaw("EXEC sp_PresupuestoDetalle @opcion = 3, @id = {0}", id);
        }

        public int ObtenerSigienteIdPresupuestoDetalle()
        {
            return _context.SiguienteId("PresupuestoDetalle");
        }
    }
}
