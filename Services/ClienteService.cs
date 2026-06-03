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
            if (cliente.id == 0)
                cliente.id = _context.SiguienteId("Cliente");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Cliente @opcion = 1, @id = {0}, @nombre = {1}, @telefono = {2}, @estatus = {3}, @idDireccion = {4}",
                cliente.id, cliente.nombre, cliente.telefono, cliente.estatus, cliente.idDireccion);
        }

        public void ModificarCliente(Cliente cliente) {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Cliente @opcion = 2, @id = {0}, @nombre = {1}, @telefono = {2}, @estatus = {3}, @idDireccion = {4}",
                cliente.id, cliente.nombre, cliente.telefono, cliente.estatus, cliente.idDireccion);
        }
        public void CrearOActualizarCliente(Cliente cliente) {
            var existe = _context.Cliente.Find(cliente.id);
            if (existe == null)
                CrearCliente(cliente);
            else
                ModificarCliente(cliente);
        }

        public void EliminarCliente(int id) {
            _context.Database.ExecuteSqlRaw("EXEC sp_Cliente @opcion = 3, @id = {0}", id);
        }
        public Cliente BuscarClienteIndividual(int id) {
            return _context.Cliente.FromSqlRaw("EXEC sp_Cliente @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public List<Cliente> ObtenerClientes() {
            return _context.Cliente.FromSqlRaw("EXEC sp_Cliente @opcion = 4").ToList();
        }

        public int ObtenerSigienteIdCliente(){
            return _context.SiguienteId("Cliente");
        }
    }
}
