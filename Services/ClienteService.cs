using AppTaller.Model;
using AppTaller.Views;
using Microsoft.EntityFrameworkCore;
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

        public ClienteService(EF.efAppDbContext context) {
            _context = context;
        }

        public void CrearCliente(Cliente cliente) {
            _context.Cliente.Add(cliente);
        }

        public void ModificarCliente(Cliente cliente) {
            var existente = _context.Cliente.Find(cliente.id);
            if (existente == null)
                return;

            _context.Entry(existente).CurrentValues.SetValues(cliente);
        }
        public void CrearOActualizarCliente(Cliente cliente) {
                var existe = _context.Cliente.Find(cliente.id);
                if (existe == null) {
                    CrearCliente(cliente);
                }
                else {
                    ModificarCliente(cliente);
                }            
        }

        public void EliminarCliente(int id) {
            var cliente = _context.Cliente.Find(id);
            if (cliente == null) 
                return;
            _context.Cliente.Remove(cliente);
            _context.SaveChanges();
            
        }
        public Cliente BuscarClienteIndividual(int id) {  
             return _context.Cliente.Find(id);
                     }
        public List<Cliente> ObtenerClientes() {
            return _context.Cliente
                .AsNoTracking()
                .ToList();
        }

        public int ObtenerSigienteIdCliente(){
            var ultimo = _context.Cliente
                                 .OrderByDescending(x => x.id)
                                 .Select(x => x.id)
                                 .FirstOrDefault();

            return ultimo + 1;
        }
    }
}
