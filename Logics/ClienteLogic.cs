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
        public void GuardarClienteYDireccion(Cliente cliente, Direccion direccion)
        {
            using (var context = new efAppDbContext())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var direccionService = new DireccionService(context);
                    var clienteService = new ClienteService(context);

                    
                    direccionService.CrearOActualizarDireccion(direccion);
                    context.SaveChanges();
                   
                    cliente.idDireccion = direccion.id;
                    
                    clienteService.CrearOActualizarCliente(cliente);

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
