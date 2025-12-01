using AppTaller.EF;
using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Logics
{
    internal class ClienteLogic
    {
        private readonly efAppDbContext _context;
        private readonly DireccionService _direccionService;
        private readonly ClienteService _clienteService;

        public ClienteLogic( efAppDbContext context,DireccionService direccionService,ClienteService clienteService){
            _context = context;
            _direccionService = direccionService;
            _clienteService = clienteService;
        }

        public void GuardarClienteYDireccion(Cliente cliente, Direccion direccion) {
            using (var transaction = _context.Database.BeginTransaction()){
                try{
                    _direccionService.CrearOActualizarDireccion(direccion);
                    _context.SaveChanges();

                    cliente.idDireccion = direccion.id;
                    _clienteService.CrearOActualizarCliente(cliente);

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception){
                    transaction.Rollback();
                    throw;
                }
            }
        }

    }
}
