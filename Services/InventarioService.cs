using AppTaller.EF;
using AppTaller.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class InventarioService
    {
        private readonly EF.efAppDbContext _context;
        public InventarioService(EF.efAppDbContext context){
            _context = context;
        }

        /// <summary>
        /// Registra un movimiento de entrada o ajuste absoluto de stock.
        /// </summary>
        /// <param name="idInventario">ID del inventario. Si es entrada y no existe inventario, debe enviarse un nuevo ID.</param>
        /// <param name="idProducto">ID del producto</param>
        /// <param name="idAlmacen">ID del almacén</param>
        /// <param name="nuevoValor">Para entrada: cantidad a sumar. Para ajuste: nuevo stock final.</param>
        /// <param name="idTipoMovimiento">1 = Entrada, 3 = Ajuste</param>
        /// <param name="idReferenciaMovimiento">Referencia obligatoria</param>
        public void RegistrarEntradaOAjuste(int idInventario, int idProducto, int idAlmacen, int nuevoValor, int idTipoMovimiento, int idReferenciaMovimiento)
        {
            // Validación rápida de tipo
            if (idTipoMovimiento != 1 && idTipoMovimiento != 3)
                throw new Exception("Solo puedes registrar Entradas o Ajustes.");

            // Validar referencia obligatoria
            var referencia = _context.ReferenciaMovimiento.FirstOrDefault(r => r.id == idReferenciaMovimiento);
            if (referencia == null)
                throw new Exception("La referencia seleccionada no existe.");

            // Buscar inventario existente
            var inventarioExistente = _context.Inventario
                .FirstOrDefault(i => i.id == idInventario);

            if (idTipoMovimiento == 1) // ENTRADA
            {
                // Crear inventario si no existe
                if (inventarioExistente == null)
                {
                    if (nuevoValor <= 0)
                        throw new Exception("La cantidad de entrada debe ser mayor a 0.");

                    // Validar que no exista un inventario con este producto y almacén
                    var existeMismoProductoAlmacen = _context.Inventario
                        .Any(i => i.idProducto == idProducto && i.idAlmacen == idAlmacen);

                    if (existeMismoProductoAlmacen)
                        throw new Exception("Este producto ya tiene inventario en este almacén.");

                    inventarioExistente = new Inventario
                    {
                        id = idInventario,
                        idProducto = idProducto,
                        idAlmacen = idAlmacen,
                        stockActual = nuevoValor,
                        fechaUltimaActualizacion = DateTime.Now
                    };

                    _context.Inventario.Add(inventarioExistente);
                }
                else
                {
                    // Sumar al stock existente
                    inventarioExistente.stockActual += nuevoValor;
                    inventarioExistente.fechaUltimaActualizacion = DateTime.Now;
                }
            }
            else if (idTipoMovimiento == 3) // AJUSTE ABSOLUTO
            {
                if (inventarioExistente == null)
                    throw new Exception("No puedes ajustar el stock porque todavía no existe inventario registrado para este producto en este almacén.");

                if (nuevoValor < 0)
                    throw new Exception("El stock no puede quedar en negativo.");

                inventarioExistente.stockActual = nuevoValor;
                inventarioExistente.fechaUltimaActualizacion = DateTime.Now;
            }

            // Registrar MovimientoInventario
            int nuevoIdMovimiento = _context.MovimientoInventario.Any()
                ? _context.MovimientoInventario.Max(m => m.id) + 1
                : 1;

            var movimiento = new MovimientoInventario
            {
                id = nuevoIdMovimiento,
                idInventario = inventarioExistente.id,
                idTipoMovimiento = idTipoMovimiento,
                idReferenciaMovimiento= idReferenciaMovimiento,
                cantidad = nuevoValor // Para entrada: cantidad sumada. Para ajuste: nuevo stock final
            };

            _context.MovimientoInventario.Add(movimiento);

            _context.SaveChanges();
        }
    }
}
