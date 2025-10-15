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
        private readonly EF.efAppDbContext _context;

        public ClienteService() {
            _context = new EF.efAppDbContext();
        }

        //Crear usuarios
        public void CrearCliente(Cliente cliente) {

            _context.Cliente.Add(cliente);
            _context.SaveChanges();
        }

        //modificar los usuarios
        public void ModificarCliente(Cliente cliente) {

            _context.Cliente.Update(cliente);
            _context.SaveChanges();
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
            var cliente = _context.Cliente.FirstOrDefault(c => c.id == id);
            if (cliente != null) {
                _context.Cliente.Remove(cliente);
                _context.SaveChanges();
            }
        }
        //Busqueda individual
        public void BuscarClienteIndividual(Cliente cliente) {  
            var clienteC = _context.Cliente.FirstOrDefault(c => c.id == cliente.id);
                _context.SaveChanges();            
        }
        //Busqueda general
        public List<Cliente> ObtenerClientes() {
            return _context.Cliente.ToList();
        }
    }
}
