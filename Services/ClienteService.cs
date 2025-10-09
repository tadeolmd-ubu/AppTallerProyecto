using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class ClienteService
    {
        //Crear usuarios
        public void CrearCliente(Cliente cliente) {

            using (var context = new EF.efAppDbContext()) {
                context.Cliente.Add(cliente);
                context.SaveChanges();
            }
        }

        //modificar los usuarios
        public void ModificarCliente(Cliente cliente) {

            using (var context = new EF.efAppDbContext()) {
                context.Cliente.Update(cliente);
                context.SaveChanges();
            }
        }
        //el metodo es pa que un solo boton de guardar haga las dos cosas
        public void CrearOActualizarCliente(Cliente cliente) {

            using (var context = new EF.efAppDbContext()) {

                var existe = context.Cliente.FirstOrDefault(c => c.id == c.id);

                if (existe == null) {
                    CrearCliente(cliente);
                }
                else {
                    ModificarCliente(cliente);
                }
                context.SaveChanges();
            }
        }

        //Eliminar
        public void EliminarCliente(int id) {

            using (var context = new EF.efAppDbContext()) {
                var cliente = context.Cliente.FirstOrDefault(c => c.id == id);
                if (cliente != null){
                    context.Cliente.Remove(cliente);
                    context.SaveChanges();
                }
            }
        }
        //Busqueda individual
        public void BuscarClienteIndividual(Cliente cliente) {

            using (var context = new EF.efAppDbContext()) {
                context.Cliente.FirstOrDefault(c => c.id == cliente.id);
                context.SaveChanges();
            }
        }
        //Busqueda general
        public List<Cliente> ObtenerClientes(int id ) {

            using (var context = new EF.efAppDbContext()) {
                return context.Cliente.ToList();
            }
        
        }
    }
}
