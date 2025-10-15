using AppTaller.EF;
using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Logic
{
    internal class ClienteLogic
    {
        private readonly ClienteService _clienteService;
        private readonly DireccionService _direccionService;
        private readonly EF.efAppDbContext _context;

        public ClienteLogic()
        {
            
            _context = new EF.efAppDbContext();
        }


        public void GuardarClienteCompleto(Cliente cliente, Direccion direccion) {

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // PASAMOS el contexto al servicio
                    var direccionService = new DireccionService(_context);
                    var clienteService = new ClienteService(_context);

                    if (direccion.id != 0)
                        direccionService.GuardarDireccion(direccion);
                    //else
                    //    direccionService.ModificarDireccion(direccion);

                    //cliente.idDireccion = direccion.id;

                    //if (cliente.id == 0)
                    //    clienteService.CrearCliente(cliente);
                    //else
                    //    clienteService.ModificarCliente(cliente);

                    //transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }

        }

    }
}