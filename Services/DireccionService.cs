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
        //insertar direcciones

        public void GuardarDireccion(Direccion direccion) {
            using (var context = new EF.efAppDbContext()) {
                context.Direccion.Add(direccion);
                context.SaveChanges();
            }
        }
        public void ModificarDireccion(Direccion direccion) {
            using (var context = new EF.efAppDbContext()) {
                context.Direccion.Update(direccion);
                context.SaveChanges();
            }

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
            using (var context = new EF.efAppDbContext()) {
                var direccion = context.Direccion.FirstOrDefault(c => c.id == id);
                if (direccion != null) {
                    context.Direccion.Remove(direccion);
                    context.SaveChanges();
                }
            }
        }

        public Direccion BuscarDireccionIndividual(int id) {
            using (var context = new EF.efAppDbContext()) {
                return context.Direccion.FirstOrDefault(d => d.id == id);
            }
        }

        public List<Direccion> ObtenerDirecciones(){
            using (var context = new EF.efAppDbContext()) {
            return context.Direccion.ToList();
            }
        }

    }
}
