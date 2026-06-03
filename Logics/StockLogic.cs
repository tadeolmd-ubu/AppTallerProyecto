using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Logics
{
    internal class StockLogic
    {
        private readonly EF.efAppDbContext _context;
        private readonly InventarioService _inventarioService;
        private readonly ProductoService _productoService;
        private readonly MovimientoInventarioService _movimientoInventarioService;
        private readonly ReferenciaService _referenciaService;
        public StockLogic(EF.efAppDbContext context)
        {
            _context = context;
            _inventarioService = new InventarioService(_context);
            _productoService = new ProductoService(_context);
            _movimientoInventarioService = new MovimientoInventarioService(_context);
            _referenciaService = new ReferenciaService(_context);
        }

        public void RegistrarEntradaOAjuste(
            int idInventario,
            int idProducto,
            int idAlmacen,
            int nuevoValor,
            int idTipoMovimiento,
            int idReferenciaMovimiento)
        {
            // Solo se permite Entradas (1) o Ajustes (3)
            if (idTipoMovimiento != 1 && idTipoMovimiento != 3)
                throw new Exception("Solo se permiten movimientos de tipo Entrada o Ajuste.");

            // Validar referencia
            var referencia = _referenciaService.BuscarReferencia(idReferenciaMovimiento);
            if (referencia == null)
                throw new Exception("La referencia seleccionada no existe.");

            // Obtener inventario
            var inventario = _inventarioService.BuscarInventario(idInventario);

            if (idTipoMovimiento == 1) // ENTRADA
            {
                RegistrarEntrada(ref inventario, idInventario, idProducto, idAlmacen, nuevoValor);
            }
            else if (idTipoMovimiento == 3) // AJUSTE
            {
                RegistrarAjuste(inventario, nuevoValor);
            }

            RegistrarMovimientoInventario( inventario.id, idTipoMovimiento, idReferenciaMovimiento, nuevoValor);
        }

        private void RegistrarEntrada(
            ref Inventario inventario,
            int idInventario,
            int idProducto,
            int idAlmacen,
            int nuevoValor)
        {
            if (nuevoValor <= 0)
                throw new Exception("La cantidad de entrada debe ser mayor a 0.");

            if (inventario == null)
            {
                // Validar que no haya otro inventario para ese producto y almacén
                bool existe = _inventarioService.ExisteInventarioProductoAlmacen(idProducto, idAlmacen);
                if (existe)
                    throw new Exception("Este producto ya tiene inventario en este almacén.");

                inventario = new Inventario
                {
                    id = idInventario,
                    idProducto = idProducto,
                    idAlmacen = idAlmacen,
                    stockActual = nuevoValor,
                    fechaUltimaActualizacion = DateTime.Now
                };

                _inventarioService.CrearInventario(inventario);
            }
            else
            {
                inventario.stockActual += nuevoValor;
                inventario.fechaUltimaActualizacion = DateTime.Now;

                _inventarioService.ModificarInventario(inventario);
            }
        }

        private void RegistrarAjuste(Inventario inventario, int nuevoValor)
        {
            if (inventario == null)
                throw new Exception("No puedes ajustar stock porque no existe inventario para este producto en este almacén.");

            if (nuevoValor < 0)
                throw new Exception("El stock no puede quedar negativo.");

            inventario.stockActual = nuevoValor;
            inventario.fechaUltimaActualizacion = DateTime.Now;

            _inventarioService.ModificarInventario(inventario);
        }

        private void RegistrarMovimientoInventario(int idInventario, int idTipoMovimiento, int idReferenciaMovimiento, int cantidad)
        {
            int nuevoIdMovimiento = _movimientoInventarioService.ObtenerSiguienteId();

            var movimiento = new MovimientoInventario
            {
                id = nuevoIdMovimiento,
                idInventario = idInventario,
                idTipoMovimiento = idTipoMovimiento,
                idReferenciaMovimiento = idReferenciaMovimiento,
                cantidad = cantidad,
                fechaMovimiento = DateTime.Now
            };

            _movimientoInventarioService.CrearMovimientoInventario(movimiento);
        }
    }
}
