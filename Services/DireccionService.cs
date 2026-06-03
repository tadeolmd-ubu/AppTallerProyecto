using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTaller.Model;
using AppTaller.Views;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
namespace AppTaller.Services
{
    internal class DireccionService
    {
        private readonly EF.efAppDbContext _context;

        public DireccionService(EF.efAppDbContext context) {
            _context = context;
        }
        public void GuardarDireccion(Direccion direccion) {
            if (direccion.id == 0)
                direccion.id = _context.SiguienteId("Direccion");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Direccion @opcion = 1, @id = {0}, @ciudad = {1}, @colonia = {2}, @codigoPostal = {3}, @calle = {4}, @numeroCasa = {5}",
                direccion.id, direccion.ciudad, direccion.colonia, direccion.codigoPostal, direccion.calle, direccion.numeroCasa);
        }
        public void ModificarDireccion(Direccion direccion) {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Direccion @opcion = 2, @id = {0}, @ciudad = {1}, @colonia = {2}, @codigoPostal = {3}, @calle = {4}, @numeroCasa = {5}",
                direccion.id, direccion.ciudad, direccion.colonia, direccion.codigoPostal, direccion.calle, direccion.numeroCasa);
        }
        public void CrearOActualizarDireccion(Direccion direccion) {
            var existe = _context.Direccion.Find(direccion.id);
            if (existe == null)
                GuardarDireccion(direccion);
            else
                ModificarDireccion(direccion);
        }

        public void EliminarDireccion(int id) {
            _context.Database.ExecuteSqlRaw("EXEC sp_Direccion @opcion = 3, @id = {0}", id);
        }

        public Direccion BuscarDireccionIndividual(int id) {
            return _context.Direccion.FromSqlRaw("EXEC sp_Direccion @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }

        public List<Direccion> ObtenerDirecciones(){
            return _context.Direccion.FromSqlRaw("EXEC sp_Direccion @opcion = 4").ToList();
        }
        public int ObtenerSigienteIdDireccion()
        {
            return _context.SiguienteId("Direccion");
        }
    }
}
