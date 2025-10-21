using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _context.catEmpresa.Add(empresa);
        }

        public void ModificarEmpresa(catEmpresa empresa){
            var existente = _context.catEmpresa.Find(empresa.id);
            if (existente == null)
                return;
            _context.Entry(existente).CurrentValues.SetValues(empresa);
        }

        public void CrearOActualizarDireccion(catEmpresa empresa) {
            var existe = _context.catEmpresa.Find(empresa.id);
            if (existe == null)
            {
                CrearEmpresa(empresa);
            }
            else
            {
                ModificarEmpresa(empresa);
            }
        }

        public void EliminarEmpresa(int id){
            var empresa = _context.catEmpresa.Find(id);
            if (empresa == null)
                return;
            _context.catEmpresa.Remove(empresa);
            _context.SaveChanges();
        }

        public catEmpresa BuscarEmpresaIndividual(int id){
            return _context.catEmpresa.Find(id);
        }

        public List<catEmpresa> ObtenerEmpresas(){
            return _context.catEmpresa.ToList();
        }

    }
}
