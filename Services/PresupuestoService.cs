using AppTaller.Model;
using Microsoft.EntityFrameworkCore;
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
            if (presupuesto.id == 0)
                presupuesto.id = _context.SiguienteId("Presupuesto");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Presupuesto @opcion = 1, @id = {0}, @total = {1}, @estatus = {2}, @nota = {3}, @idCliente = {4}, @idUsuario = {5}",
                presupuesto.id, presupuesto.total, presupuesto.estatus, presupuesto.nota ?? (object)DBNull.Value,
                presupuesto.idCliente, presupuesto.idUsuario);
        }
        public void ModificarPresupuesto(Presupuesto presupuesto) {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Presupuesto @opcion = 2, @id = {0}, @total = {1}, @estatus = {2}, @nota = {3}, @idCliente = {4}, @idUsuario = {5}",
                presupuesto.id, presupuesto.total, presupuesto.estatus, presupuesto.nota ?? (object)DBNull.Value,
                presupuesto.idCliente, presupuesto.idUsuario);
        }
        public void CrearOModificarPresupuesto(Presupuesto presupuesto){
            var existe = _context.Presupuesto.Find(presupuesto.id);
            if (existe == null)
                CrearPresupuesto(presupuesto);
            else
                ModificarPresupuesto(presupuesto);
        }
        public Presupuesto BuscarPresupuesto(int id) {
            return _context.Presupuesto.FromSqlRaw("EXEC sp_Presupuesto @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public List<Presupuesto> ObtenerPresupuestos()
        {
            return _context.Presupuesto
                     .FromSqlRaw("EXEC sp_Presupuesto @opcion = 4")
                     .ToList();
        }
        public void EliminarPresupuesto(int id) {
            _context.Database.ExecuteSqlRaw("EXEC sp_Presupuesto @opcion = 3, @id = {0}", id);
        }
        public int ObtenerSigienteIdPresupuesto(){
            return _context.SiguienteId("Presupuesto");
        }
        public bool ExistePresupuesto(int idPresupuesto){
            if (idPresupuesto <= 0)
                return false;
            return _context.Presupuesto.Any(i => i.id == idPresupuesto);
        }
    }
}
