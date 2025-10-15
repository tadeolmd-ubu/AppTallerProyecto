using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTaller.Model;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
namespace AppTaller.Services
{
    internal class DireccionService
    {
        private readonly EF.efAppDbContext _context;

        public DireccionService() {
        _context = new EF.efAppDbContext();
        }

        //insertar direcciones

        public void GuardarDireccion(Direccion direccion) {
            _context.Direccion.Add(direccion);
            _context.SaveChanges();
        }
        public void ModificarDireccion(Direccion direccion) {
            _context.Direccion.Update(direccion);
            _context.SaveChanges();
        }
        public void CrearOActualizarDireccion(Direccion direccion) {
            using (var context = new EF.efAppDbContext()) {
                var existe = context.Direccion.FirstOrDefault(d => d.id == direccion.id);
                if (existe == null) {
                    GuardarDireccion(direccion);
                }
                else {
                    ModificarDireccion(direccion);
                }
                context.SaveChanges();
            }
        }

        public void EliminarDireccion(int id) {            
                var direccion = _context.Direccion.Find(id);
            if (direccion != null)
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
