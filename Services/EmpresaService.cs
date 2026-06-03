using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class EmpresaService
    {
        private readonly EF.efAppDbContext _context;

        public EmpresaService(EF.efAppDbContext context)
        {
            _context = context;
        }
        public void CrearEmpresa(catEmpresa empresa) {
            if (empresa.id == 0)
                empresa.id = _context.SiguienteId("catEmpresa");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_catEmpresa @opcion = 1, @id = {0}, @nombre = {1}, @rfc = {2}, @regimen = {3}, @idDireccion = {4}",
                empresa.id, empresa.nombre, empresa.rfc, empresa.regimen, empresa.idDireccion);
        }
        public void ModificarEmpresa(catEmpresa empresa){
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_catEmpresa @opcion = 2, @id = {0}, @nombre = {1}, @rfc = {2}, @regimen = {3}, @idDireccion = {4}",
                empresa.id, empresa.nombre, empresa.rfc, empresa.regimen, empresa.idDireccion);
        }
        public void CrearOActualizarDireccion(catEmpresa empresa) {
            var existe = _context.catEmpresa.Find(empresa.id);
            if (existe == null)
                CrearEmpresa(empresa);
            else
                ModificarEmpresa(empresa);
        }
        public void EliminarEmpresa(int id){
            _context.Database.ExecuteSqlRaw("EXEC sp_catEmpresa @opcion = 3, @id = {0}", id);
        }
        public catEmpresa BuscarEmpresaIndividual(int id){
            return _context.catEmpresa.FromSqlRaw("EXEC sp_catEmpresa @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public List<catEmpresa> ObtenerEmpresas(){
            return _context.catEmpresa.FromSqlRaw("EXEC sp_catEmpresa @opcion = 4").ToList();
        }
    }
}
