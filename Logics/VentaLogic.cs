using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AppTaller.Logics
{
    internal class VentaLogic
    {
        /*
         Pseudocódigo detallado (plan):
         1. Método: CrearVenta(Venta venta, IEnumerable<VentaDetalle> detalles)
            - Validar argumentos (venta != null, detalles != null y al menos 1).
         2. Abrir una transacción de la base de datos para asegurar atomicidad.
         3. Recorrer cada detalle propuesto:
            - Buscar el producto en Inventario por detalle.idInventario.
            - Si no existe -> lanzar excepción indicando producto no encontrado.
            - Si stockActual < detalle.cantidad -> lanzar excepción "Stock insuficiente" indicando producto.
         4. Insertar la entidad 'venta' en la BD y hacer SaveChanges() para obtener su id.
         5. Para cada detalle:
            - Asignar detalle.idVenta = venta.id.
            - Añadir el detalle al contexto (_context.VentaDetalle.Add(detalle)).
            - Reducir producto.stockActual -= detalle.cantidad.
         6. Guardar cambios para persistir detalles y actualizar stock.
         7. Recalcular el importe de cada detalle usando _ventaDetalleService.RecalcularImporte(detalle.id).
         8. Guardar cambios finales y confirmar la transacción.
         9. En caso de error hacer rollback y re-lanzar la excepción.
         10. Devolver la entidad venta creada (con su id).
        */

        private readonly EF.efAppDbContext _context;
        private readonly VentaService _ventaService;
        private readonly VentaDetalleService _ventaDetalleService;
        private readonly InventarioService _inventarioService;
        public VentaLogic(){
            _context = new EF.efAppDbContext();
            _ventaService = new VentaService(_context);
            _ventaDetalleService = new VentaDetalleService(_context);
            _inventarioService = new InventarioService(_context);
        }

        // Crear una venta y descontar stock según los detalles seleccionados
        public Venta CrearVenta(Venta venta, IEnumerable<VentaDetalle> detalles)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta));
            if (detalles == null || !detalles.Any())
                throw new ArgumentException("La venta debe contener al menos un detalle", nameof(detalles));

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Validar stock disponible para todos los detalles antes de modificar nada
                    foreach (var det in detalles)
                    {
                        var producto = _context.Inventario.Find(det.idInventario);
                        if (producto == null)
                            throw new Exception($"Producto con id {det.idInventario} no encontrado");

                        if (producto.stockActual < det.cantidad)
                            throw new Exception($"Stock insuficiente para el producto con id {det.idInventario}");
                    }

                    // Insertar la venta para obtener su id
                    _context.Venta.Add(venta);
                    _context.SaveChanges();

                    // Añadir detalles y descontar stock
                    foreach (var det in detalles)
                    {
                        det.idVenta = venta.id;
                        _context.VentaDetalle.Add(det);

                        var producto = _context.Inventario.Find(det.idInventario);
                        // Revalidación por seguridad
                        if (producto == null)
                            throw new Exception($"Producto con id {det.idInventario} no encontrado");

                        producto.stockActual -= det.cantidad;
                    }

                    _context.SaveChanges();

                    // Recalcular importes de cada detalle si el servicio proporciona esa funcionalidad
                    foreach (var det in detalles)
                    {
                        // Si RecalcularImporte espera el id del detalle, asegurarse de que esté seteado tras SaveChanges
                        _ventaDetalleService.RecalcularImporte(det.id);
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                    return venta;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        //Actualizar cantidad y el importe de una venta
        public void ActualizarCantidad(int idDetalle, int nuevaCantidad)
        {
            var detalle = _context.VentaDetalle.Find(idDetalle);
            if (detalle == null)
                throw new Exception("Detalle no encontrado");

            var diferencia = nuevaCantidad - detalle.cantidad;

            var producto = _context.Inventario.Find(detalle.idInventario);

            if (producto.stockActual < diferencia)
                throw new Exception("Stock insuficiente");

            producto.stockActual -= diferencia;

            detalle.cantidad = nuevaCantidad;
            _ventaDetalleService.RecalcularImporte(detalle.id);

            _context.SaveChanges();
        }
    }
}
