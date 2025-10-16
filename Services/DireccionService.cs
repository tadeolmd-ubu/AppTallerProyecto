using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTaller.Model;
using AppTaller.Views;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
namespace AppTaller.Services
{
    internal class DireccionService
    {
        private readonly EF.efAppDbContext _context;

        public DireccionService(EF.efAppDbContext context) {
            _context = context;
        }

        //insertar direcciones

        public void GuardarDireccion(Direccion direccion) {
            _context.Direccion.Add(direccion);
            _context.SaveChanges();
        }
        public void ModificarDireccion(Direccion direccion) {
            var existente = _context.Direccion.Find(direccion.id);
            if (existente == null)
                return;

            _context.Entry(existente).CurrentValues.SetValues(direccion);
            _context.SaveChanges();
        }
        public void CrearOActualizarDireccion(Direccion direccion) {         
                var existe = _context.Direccion.Find(direccion.id);
                if (existe == null) {
                    GuardarDireccion(direccion);
                }
                else {
                    ModificarDireccion(direccion);
                }
                _context.SaveChanges();            
        }

        public void EliminarDireccion(int id) {            
                var direccion = _context.Direccion.Find(id);
            if (direccion == null)
                return;
            _context.Direccion.Remove(direccion);
            _context.SaveChanges(); 
                
        }

        public Direccion BuscarDireccionIndividual(int id) {
            return _context.Direccion.Find(id);
        }

        public List<Direccion> ObtenerDirecciones(){            
            return _context.Direccion.ToList();            
        }

    }
}
